using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using LZ4;

namespace UnityReader
{
	public sealed class DerPopoClassDatabase
	{
		private string _stringTable;
		public DatabaseHeader Header { get; private set; }

		public IEnumerable<TypeClass> Types => Classes;

		public TypeClass[] Classes;

		public void Read(Stream data)
		{
			if (data == null) throw new ArgumentNullException(nameof(data));
			var reader = new UnityReader(data);
			Header = new DatabaseHeader(reader);

			byte[] buffer = reader.ReadBytes(Header.CompressedSize);
			byte[] uncompressed = LZ4Codec.Decode(buffer, 0, buffer.Length, Header.UncompressedSize);
			using (var ms = new MemoryStream(uncompressed))
			{
				UnityReader lz4Reader = new UnityReader(ms);
				int classCount = lz4Reader.ReadInt32();
				Classes = new TypeClass[classCount];
				for (int i = 0; i < classCount; i++)
				{
					Classes[i] = new TypeClass(lz4Reader, this);
				}
				lz4Reader.Position = Header.StringTablePosition;
				_stringTable = lz4Reader.ReadStringFixed(Header.StringTableLength, Encoding.ASCII);
			}
		}

		public void Write()
		{
			XmlDocument doc = new XmlDocument();
			var rootNode = doc.CreateElement("UnityTypeDatabase");
			doc.AppendChild(rootNode);
			foreach (TypeClass cls in Classes.OrderBy(c => c.ClassID))
			{
				var classNode = doc.CreateElement("UnityType");
				classNode.SetAttribute("assetCode", cls.ClassID.ToString());
				classNode.SetAttribute("name", cls.Name);
				if (cls.BaseClass != null)
				{
					classNode.SetAttribute("base", cls.BaseClass.Name);
				}
				WriteFields(doc, classNode, cls);
				rootNode.AppendChild(classNode);
			}
			using (var writer = XmlWriter.Create("test.xml"))
			{
				doc.WriteTo(writer);
			}
		}

		private void WriteFields(XmlDocument doc, XmlElement classNode, TypeClass cls)
		{
			if (!cls.Fields.Any()) { return; }
			Stack<KeyValuePair<TypeField, XmlElement>> stack = new Stack<KeyValuePair<TypeField, XmlElement>>();
			stack.Push(new KeyValuePair<TypeField, XmlElement>(cls.Fields.First(), classNode));
			foreach (TypeField field in cls.Fields.Skip(1))
			{
				var fieldNode = doc.CreateElement("Field");
				fieldNode.SetAttribute("Align", field.Aligned ? "1" : "0");
				fieldNode.SetAttribute("Type", field.TypeName);
				fieldNode.SetAttribute("Size", field.Size.ToString());
				fieldNode.SetAttribute("Name", field.Name);

				int previousDepth = stack.Peek().Key.Depth;
				if (previousDepth > field.Depth)
				{
					//Closed leaf
					while (stack.Peek().Key.Depth > field.Depth)
					{
						stack.Pop();
					}
					stack.Pop();
				}
				else if (previousDepth == field.Depth)
				{
					//Sibling
					stack.Pop();
				}
				stack.Peek().Value.AppendChild(fieldNode);
				stack.Push(new KeyValuePair<TypeField, XmlElement>(field, fieldNode));
				previousDepth = field.Depth;
			}
		}

		public string ResolveString(int pos)
		{
			for (int i = pos; i < _stringTable.Length; i++)
			{
				if (_stringTable[i] == 0x00)
				{
					return _stringTable.Substring(pos, (i - pos));
				}
			}
			return _stringTable.Substring(pos);
		}

		public TypeClass GetType(string name)
		{
			return Types.FirstOrDefault(t => t.Name == name);
		}

		public TypeClass GetType(int classID)
		{
			return Types.FirstOrDefault(t => t.ClassID == classID);
		}

		public sealed class DatabaseHeader
		{
			public byte Version;

			public CompressionType Compression;
			public int CompressedSize;
			public int UncompressedSize;

			public string[] UnityVersions;

			public int StringTableLength;
			public int StringTablePosition;

			public DatabaseHeader(UnityReader reader)
			{
				if (reader == null) throw new ArgumentNullException(nameof(reader));
				string magic = reader.ReadStringFixed(4);
				if (magic != "cldb")
				{
					throw new ArgumentException(nameof(reader));
				}

				Version = reader.ReadByte();
				if (Version < 2)
				{
					Compression = CompressionType.None;
					CompressedSize = 0;
					UncompressedSize = 0;
				}
				else
				{
					Compression = (CompressionType)reader.ReadByte();
					CompressedSize = reader.ReadInt32();
					UncompressedSize = reader.ReadInt32();
				}
				if (Version == 0)
				{
					int skipCount = reader.ReadByte();
					reader.ReadBytes(skipCount);
				}
				else
				{
					int labelCount = reader.ReadByte();
					UnityVersions = new string[labelCount];
					for (int i = 0; i < labelCount; i++)
					{
						int stringLength = reader.ReadByte();
						string name = reader.ReadStringFixed(stringLength);
						UnityVersions[i] = name;
					}
				}
				StringTableLength = reader.ReadInt32();
				StringTablePosition = reader.ReadInt32();
			}
		}

		public enum CompressionType
		{
			None = 0x00,
			LZ4 = 0x01,
		}

		[DebuggerDisplay("Name = {Name}")]
		public sealed class TypeClass
		{
			private DerPopoClassDatabase _database;
			private int _namePointer;
			public int ClassID { get; }
			public int BaseClassID;
			private TypeField[] _fields;

			public TypeClass BaseClass => _database.GetType(BaseClassID);

			public IEnumerable<TypeField> Fields => _fields;

			public string Name => _database.ResolveString(_namePointer);

			public TypeClass(UnityReader reader, DerPopoClassDatabase database)
			{
				_database = database;

				ClassID = reader.ReadInt32();
				BaseClassID = reader.ReadInt32();
				_namePointer = reader.ReadInt32();
				int fieldCount = reader.ReadInt32();
				_fields = new TypeField[fieldCount];
				int depth = -1;

				Stack<TypeField> branchStack = new Stack<TypeField>();
				for (int i = 0; i < fieldCount; i++)
				{
					var field = new TypeField(reader, database);
					_fields[i] = field;

					if (field.Depth > depth)
					{
						//New child
						TypeField parent = branchStack.LastOrDefault();
						branchStack.Push(field);
					}
					else if (field.Depth < depth)
					{
						//End of leaf, merge back to other branch
						while (branchStack.Count > 0)
						{
							var branch = branchStack.Pop();
							if (branch.Depth == field.Depth)
							{
								break;
							}
							if (branch.Depth < field.Depth)
							{
								throw new NotImplementedException();
							}
						}
						branchStack.Push(field);
					}
					else
					{
						//sibling
						branchStack.Pop();
						var parent = branchStack.Count > 0 ? branchStack.Peek() : null;

						branchStack.Push(field);
					}

					depth = field.Depth;
				}
			}

			public TypeField GetField(string name)
			{
				return Fields.FirstOrDefault(f => f.Name == name);
			}
		}

		[DebuggerDisplay("{_database.ResolveString(_typeNamePointer),nq} : {Name,nq}{IsArray ? \"[]\" : \"\",nq} {Depth}")]
		public class TypeField
		{
			private DerPopoClassDatabase _database;
			private int _typeNamePointer;
			private int _fieldNamePointer;

			public byte Depth;
			public bool IsArray { get; private set; }
			public int Size;
			private short Version;
			private int Flags;

			public bool Aligned => (Flags & 0x4000) > 0;

			public TypeClass ValueType => _database.GetType(TypeName);

			public string Name => _database.ResolveString(_fieldNamePointer);

			public string TypeName => _database.ResolveString(_typeNamePointer);

			public TypeField(UnityReader reader, DerPopoClassDatabase database)
			{
				_database = database;
				_typeNamePointer = reader.ReadInt32();
				_fieldNamePointer = reader.ReadInt32();
				Depth = reader.ReadByte();
				IsArray = reader.ReadBool();
				Size = reader.ReadInt32();
				if (_database.Header.Version < 1)
				{
					throw new NotImplementedException();
				}
				if (_database.Header.Version >= 3)
				{
					Version = reader.ReadInt16();
				}
				Flags = reader.ReadInt32();
			}
		}
	}
}