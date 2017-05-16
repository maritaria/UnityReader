using System;
using System.Collections.Generic;
using System.Text;

namespace UnityReader
{
	public class Unity5Type
	{
		private byte _unknown;
		private List<Field> _fields = new List<Field>();

		public int ClassID { get; set; }
		public short ScriptIndex { get; set; }
		public Hash128 ScriptHash { get; set; }
		public Hash128 PropertiesHash { get; set; }
		public ICollection<Field> TypeFields => _fields;
		public StringTable Strings { get; } = new StringTable();

		public Unity5Type(UnityReader reader, int version, bool embedded)
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
				ScriptHash = new Hash128();
				ScriptHash.Read(reader, version);
			}
			PropertiesHash = new Hash128();
			PropertiesHash.Read(reader, version);
			if (embedded)
			{
				ReadEmbedded(reader, version);
			}
		}

		private void ReadEmbedded(UnityReader reader, int version)
		{
			int fieldCount = reader.ReadInt32();
			int stringTableLength = reader.ReadInt32();

			_fields.Capacity = fieldCount;
			_fields.Clear();

			for (int i = 0; i < fieldCount; i++)
			{
				var field = new Field(reader);
				_fields.Add(field);
			}

			ReadStringTable(reader, stringTableLength);
		}

		private void ReadStringTable(UnityReader reader, int stringTableLength)
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

			public Field(UnityReader reader)
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