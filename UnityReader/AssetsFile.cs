using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UnityReader
{
	public sealed class AssetsFile
	{
		public UnityContext Context { get; }
		public UnityBinaryReader Reader { get; }
		public AssetsFileHeader Header { get; }
		public TypeTree TypeTree { get; }
		public AssetsFileTable Files { get; }
		public PreloadList PreloadList { get; }
		public AssetsFileDependencyList Dependencies { get; }

		public AssetsFile(UnityContext context, UnityBinaryReader reader)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			Reader = reader;
			Header = new AssetsFileHeader(this);
			if (Header.Version < 9)
			{
				//Meta comes after object data
				throw new NotImplementedException();
			}
			TypeTree = new TypeTree(this);

			Files = new AssetsFileTable(this);

			PreloadList = new PreloadList(this);
			Dependencies = new AssetsFileDependencyList(this);
		}

		private static uint GetAssetsFileListLength(UnityBinaryReader reader, int version)
		{
			uint sizeFiles = reader.ReadUInt32();
			uint rax;
			if (version <= 16)
			{
#warning check if this is still needed
				if (true)
				{
					if (version < 17)
					{
						uint rdx3 = 25;
						if (version >= 16)
						{
							rdx3 = 23;
						}
						rax = ((rdx3 + 3) & 0xfffffffc) * ((sizeFiles) - 1) + rdx3;
						return rax;
					}
					else
					{
						rax = 20 * ((sizeFiles) - 1) + 20;
						return rax;
					}
				}
			}
			uint blockSize;
			if (version >= 17)
			{
				blockSize = 20;
			}
			else
			{
				if (version >= 16)
				{
					rax = (sizeFiles) * 23;
					return rax;
				}
				if (version >= 15)
				{
					rax = (sizeFiles) * 25;
					return rax;
				}
				blockSize = 24;
				if (version != 14)
					blockSize = 20;
			}
			return (sizeFiles) * blockSize;
		}

		public void LoadDependencies()
		{
		}
	}

	public class AssetsFileTable
	{
		public List<AssetFileInfo> Items = new List<AssetFileInfo>();

		public AssetsFileTable(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			var reader = file.Reader;
			var version = file.Header.Version;

			int count = reader.ReadInt32();

			if (version >= 14)
			{
				reader.Align(4);
			}

			for (int i = 0; i < count; i++)
			{
				Items.Add(new AssetFileInfo(file, reader));
			}
			long pos = reader.Position;
			foreach (var info in Items)
			{
			}
		}
	}

	public struct AssetFileInfo
	{
		public ulong Index;
		public uint DataOffset;
		public uint FileSize;
		public int FileTypeOrIndex;
		public int InheritedUnityClass;//read 16 bit
		public short ScriptIndex;
		public byte Unknown;

		public int FileType;
		public long AbsolutePos;
		public string Name;

		public AssetFileInfo(AssetsFile file, UnityBinaryReader reader) : this()
		{
			/* Read basic info */

			if (file.Header.Version < 14)
			{
				Index = reader.ReadUInt32();
			}
			else
			{
				Index = reader.ReadUInt64();
			}

			//v1
			/*
			DataOffset = reader.ReadUInt32();
			FileSize = reader.ReadUInt32();
			FileTypeOrIndex = reader.ReadInt32();
			InheritedUnityClass = reader.ReadInt16();
			ScriptIndex = reader.ReadInt16();
			*/
			//v2
			DataOffset = reader.ReadUInt32();
			FileSize = reader.ReadUInt32();
			FileTypeOrIndex = reader.ReadInt32();
			InheritedUnityClass = reader.ReadInt16();
			ScriptIndex = reader.ReadInt16();

			//v3 (first v2)
			Unknown = reader.ReadByte();

			/* Advanced */
			if (file.Header.Version < 16)
			{
				FileType = FileTypeOrIndex;
			}
			else
			{
				if (FileTypeOrIndex < file.TypeTree.Types_0D.Count)
				{
					var type = file.TypeTree.Types_0D[FileTypeOrIndex];

					if (type.ScriptIndex == -1)
					{
						FileType = type.ClassID;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = -1;
					}
					else
					{
						FileType = -1 - type.ScriptIndex;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = type.ScriptIndex;
					}
				}
				else
				{

				}
			}
			AbsolutePos = file.Header.DataOffset + DataOffset;

			if (IsKnownUnityType(FileType) || true)
			{
				long pos = reader.Position;
				try
				{
#error fix this, pls
					reader.Position = AbsolutePos;
					int length = reader.ReadInt32();
					if (length > 100 && false)
					{
						if (Debugger.IsAttached)
						{
							Debugger.Break();
						}
					}
					Name = reader.ReadStringFixed(length);
				}
				finally
				{
					reader.Position = pos;
				}
			}
		}

		public static bool IsKnownUnityType(int type)
		{
			switch (type)
			{
				case 0x15:
				case 0x1B:
				case 0x1C:
				case 0x2B:
				case 0x30:
				case 0x31:
				case 0x3E:
				case 0x48:
				case 0x4A:
				case 0x53:
				case 0x54:
				case 0x59:
				case 0x5A:
				case 0x5B:
				case 0x5D:
				case 0x6D:
				case 0x73:
				case 0x75:
				case 0x79:
				case 0x80:
				case 0x86:
				case 0x8E:
				case 0x96:
				case 0x98:
				case 0x9C:
				case 0x9E:
				case 0xAB:
				case 0xB8:
				case 0xB9:
				case 0xBA:
				case 0xC2:
				case 0xC8:
				case 0xCF:
				case 0xD5:
				case 0xDD:
				case 0xE2:
				case 0xE4:
				case 0xED:
				case 0xEE:
				case 0xF0:
				case 0x102:
				case 0x10F:
				case 0x110:
				case 0x111:
				case 0x112:
					return true;

				default:
					return false;
			}
		}
	}

	public sealed class AssetsFileHeader
	{
		private int _metadataSize;
		private int _fileSize;

		public int Version { get; }
		public bool IsLittleEndian { get; }
		public byte[] Unknown { get; }
		public long DataOffset { get; internal set; }

		public AssetsFileHeader(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			var reader = file.Reader;
			reader.IsLittleEndian = false;
			_metadataSize = reader.ReadInt32();
			_fileSize = reader.ReadInt32();
			Debug.Assert(_fileSize >= 0);
			Version = reader.ReadInt32();
			DataOffset = reader.ReadInt32();
			if (Version >= 9)
			{
				IsLittleEndian = !reader.ReadBool();
				Unknown = reader.ReadBytes(3);
				reader.IsLittleEndian = IsLittleEndian;
			}
		}
	}

	public sealed class TypeTree
	{
		public string UnityVersion { get; set; }//max 25 bytes
		public int TypeVersion { get; set; }
		public bool HasTypeTree { get; set; }

		public List<Type_07> Types_07 { get; } = new List<Type_07>();
		public List<Type_0D> Types_0D { get; } = new List<Type_0D>();

		private int _unknown;
		private int _format;

		public TypeTree(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			var reader = file.Reader;
			Read(reader, file.Header.Version);
		}

		private void Read(UnityBinaryReader reader, int version)
		{
			_format = version;
			HasTypeTree = true;

			if (version <= 6)
			{
				UnityVersion = $"Unsupported Format";
				TypeVersion = 0;
				return;
			}

			UnityVersion = reader.ReadString();
			TypeVersion = reader.ReadInt32();
			if (version >= 13)
			{
				HasTypeTree = reader.ReadBool();
			}

			var fieldCount = reader.ReadInt32();

			if (version > 13)
			{
				for (int i = 0; i < fieldCount; i++)
				{
					Types_0D.Add(new Type_0D(reader, version, TypeVersion, HasTypeTree));
				}
			}
			else
			{
				throw new NotImplementedException();
				for (int i = 0; i < fieldCount; i++)
				{
					//Types_07.Add(new Type_07(reader, format, Version, HasTypeTree));
				}
			}
			if (version < 14)
			{
				_unknown = reader.ReadInt32();
			}
		}

		public class Type_07
		{
			public int ClassID { get; set; }

			public Field Base { get; }

			public class Field
			{
				public string Type { get; set; }
				public string Name { get; set; }
				public int Size { get; set; }
				public int Index { get; set; }
				public int ArrayFlag { get; set; }
				public long Flags { get; set; }

				private int _childrenCount;
				public List<Field> Children = new List<Field>();
			}
		}

		public class Type_0D
		{
			public int ClassID { get; set; }
			private byte _unknown;
			public short ScriptIndex { get; set; }

			public Hash128 ScriptID { get; set; }

			public Hash128 PropertiesHash { get; set; }

			private int _typeFieldsCount;
			public List<Field> TypeFields { get; } = new List<Field>();

			public StringTable Strings { get; } = new StringTable();

			public Type_0D(UnityBinaryReader reader, int version, int typeVersion, bool embedded)
			{
				if (reader == null) throw new ArgumentNullException(nameof(reader));
				if (version < 13) throw new ArgumentOutOfRangeException(nameof(version), "this type does not support the given format");
				ClassID = reader.ReadInt32();

				if (version < 16)
				{
					_unknown = 0;
				}
				else
				{
					_unknown = reader.ReadByte();
				}

				if (version < 17)
				{
					ScriptIndex = -1;
				}
				else
				{
					ScriptIndex = reader.ReadInt16();
				}
				if (ClassID < 0 || ClassID == 114)
				{
					ScriptID = new Hash128();
					ScriptID.Read(reader, version);
				}
				PropertiesHash = new Hash128();
				PropertiesHash.Read(reader, version);
				if (embedded)
				{
					ReadEmbedded(reader, version, typeVersion);
				}
			}

			private void ReadEmbedded(UnityBinaryReader reader, int version, int typeVersion)
			{
				int fieldCount = reader.ReadInt32();
				int stringTableLength = reader.ReadInt32();

				TypeFields.Capacity = fieldCount;
				TypeFields.Clear();

				for (int i = 0; i < fieldCount; i++)
				{
					var field = new Field(reader);
					TypeFields.Add(field);
				}

				ReadStringTable(reader, stringTableLength);
			}

			private void ReadStringTable(UnityBinaryReader reader, int stringTableLength)
			{
				var stringTableBytes = reader.ReadBytes(stringTableLength);
				Strings.Read(stringTableBytes, Encoding.ASCII);

				foreach (Field f in TypeFields)
				{
					f.ApplyStringTable(Strings);
				}
			}

			public sealed class StringTable
			{
				private Dictionary<int, string> _mapping = new Dictionary<int, string>();

				public StringTable()
				{
				}

				public string GetString(int index)
				{
					return _mapping[index];
				}

				public void Read(byte[] buffer, Encoding encoding)
				{
					if (buffer == null) throw new ArgumentNullException(nameof(buffer));
					int start = 0;
					for (int i = 0; i < buffer.Length; i++)
					{
						byte current = buffer[i];
						if (current == 0)
						{
							_mapping.Add(start, encoding.GetString(buffer, start, i - start));
							start = i + 1;
						}
					}
					if (start < buffer.Length)
					{
						_mapping.Add(start, encoding.GetString(buffer, start, buffer.Length - start));
					}
				}
			}

			public sealed class Field
			{
				public static readonly int SerializedSize = 24;

				private int _typeOffset;
				private int _nameOffset;

				public short Version { get; set; }
				public byte Depth { get; set; }
				public bool IsArray { get; set; }
				public int Size { get; set; }
				public int Index { get; set; }
				public int Flags { get; set; }
				public string Type { get; set; }
				public string Name { get; set; }

				public Field(UnityBinaryReader reader)
				{
					Version = reader.ReadInt16();
					Depth = reader.ReadByte();
					IsArray = reader.ReadBool();
					_typeOffset = reader.ReadInt32();
					_nameOffset = reader.ReadInt32();
					Size = reader.ReadInt32();
					Index = reader.ReadInt32();
					Flags = reader.ReadInt32();
				}

				public void ApplyStringTable(StringTable strings)
				{
					Type = strings.GetString(_typeOffset);
					Name = strings.GetString(_nameOffset);
				}
			}
		}
	}

	public class UnityList<Item> where Item : UnityElement, new()
	{
		public List<Item> Items { get; } = new List<Item>();

		protected void Read(UnityBinaryReader reader, int version)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			int count = reader.ReadInt32();
			Items.Capacity = count;
			Items.Clear();
			for (int i = 0; i < count; i++)
			{
				var item = new Item();
				item.Read(reader, version);
				Items.Add(item);
			}
		}
	}

	public interface UnityElement
	{
		void Read(UnityBinaryReader reader, int version);
	}

	public sealed class PreloadList : UnityList<PreloadList.AssetPointer>
	{
		public PreloadList(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			Read(file.Reader, file.Header.Version);
		}

		public struct AssetPointer : UnityElement
		{
			public int FileID;
			public long PathID;

			void UnityElement.Read(UnityBinaryReader reader, int version)
			{
				FileID = reader.ReadInt32();
				PathID = reader.ReadInt64();
			}
		}
	}

	public class AssetsFileDependencyList : UnityList<AssetsFileDependencyList.AssetsFileDependency>
	{
		public AssetsFileDependencyList(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			Read(file.Reader, file.Header.Version);
		}

		public class AssetsFileDependency : UnityElement
		{
			public string BufferedPath { get; set; }
			public Guid128 Guid { get; set; }
			public int Type { get; set; }
			public string AssetPath { get; set; }

			void UnityElement.Read(UnityBinaryReader reader, int version)
			{
				BufferedPath = reader.ReadString();
				Guid = new Guid128();
				Guid.Read(reader, version);
				Type = reader.ReadInt32();
				AssetPath = reader.ReadString();
			}

			public AssetsFile Load(UnityContext context)
			{
				return context.LoadFile(AssetPath);
			}
		}
	}

	public sealed class Guid128 : UnityElement
	{
		public long Lower { get; set; }
		public long Upper { get; set; }

		public void Read(UnityBinaryReader reader, int version)
		{
			bool littleEndian = reader.IsLittleEndian;
			reader.IsLittleEndian = false;
			try
			{
				Upper = reader.ReadInt64();
				Lower = reader.ReadInt64();
			}
			finally
			{
				reader.IsLittleEndian = littleEndian;
			}
		}
	}

	public sealed class Hash128 : UnityElement
	{
		public byte[] Bytes { get; set; }

		public void Read(UnityBinaryReader reader, int version)
		{
			Bytes = reader.ReadBytes(16);
		}

		public override string ToString()
		{
			if (Bytes == null || Bytes.Length == 0)
			{
				return string.Empty;
			}
			else
			{
				return string.Join(" ", Bytes.Select(b => string.Format("{0:X2}", b)));
			}
		}
	}
}