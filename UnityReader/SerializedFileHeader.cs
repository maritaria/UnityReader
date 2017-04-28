using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader
{
	public sealed class SerializedFileHeader
	{
		private BinarySection _raw;
		public uint MetaSize { get; set; }
		public uint FileSize { get; set; }
		public uint Version { get; set; }
		public uint DataOffset { get; set; }

		//Version >= 9
		public byte[] Unused { get; set; }//4 bytes

		public void Read(UnityBinaryReader reader)
		{
			using (_raw = reader.StartSection())
			{
				reader.IsLittleEndian = false;
				MetaSize = reader.ReadUInt32();
				FileSize = reader.ReadUInt32();
				Version = reader.ReadUInt32();
				DataOffset = reader.ReadUInt32();

				if (Version >= 9)
				{
					Unused = reader.ReadBytes(4);
				}
			}
		}
	}
}