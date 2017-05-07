using System;

namespace UnityReader.Objects
{
	public class ObjectInfo
	{

		public long ObjectID { get; set; }
		public int TypeID { get; set; }
		public int unknown { get; set; }
		public uint Offset { get; set; }
		public uint Size { get; set; }

		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			reader.Align(4);
			ObjectID = reader.ReadInt64();
			Offset = reader.ReadUInt32();
			Size = reader.ReadUInt32();
			unknown = reader.ReadInt32();
			if (unknown != 0)
			{

			}
		}
	}
}