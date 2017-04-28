using System;

namespace UnityReader.Objects
{
	public class ObjectInfo
	{
		private uint _start;
		private uint _size;

		public long ObjectID { get; set; }
		public int TypeID { get; set; }
		public short ClassID { get; set; }
		public short ScriptTypeIndex { get; set; }
		public bool Stripped { get; set; }


		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			ObjectID = reader.ReadInt64();
			_start = reader.ReadUInt32();
			_size = reader.ReadUInt32();
			TypeID = reader.ReadInt32();
			ClassID = reader.ReadInt16();
			if (header.Version < 14)
			{
				Stripped = reader.ReadInt16() != 0;
			}
			else
			{
				ScriptTypeIndex = reader.ReadInt16();
				Stripped = reader.ReadBool();
			}

		}
	}
}