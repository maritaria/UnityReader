using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public class TypeTreeV2 : TypeTreeV1
	{
		private byte _treeLevel;
		private int _typeIndex;
		private int _nameIndex;

		public void ApplyStringTable(Dictionary<int, string> stringTable)
		{
			Type = stringTable[_typeIndex];
			Name = stringTable[_nameIndex];
		}

		public override void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Version = reader.ReadInt16();
			_treeLevel = reader.ReadByte();
			_isArray = reader.ReadBool();
			_typeIndex = reader.ReadInt32();
			_nameIndex = reader.ReadInt32();
			_size = reader.ReadUInt32();
			_index = reader.ReadUInt32();
			MetaFlags = (Attributes)reader.ReadInt32();
		}
	}
}