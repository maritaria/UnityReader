using System;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class ObjectInfo
	{
		public bool Stripped { get; set; }
		public uint BufferStart { get; set; }
		public uint BufferLength { get; set; }
		public int TypeID { get; set; }
		public short ClassID { get; set; }
		public short ScriptTypeIndex { get; set; }

		public async Task Read(BinaryReader reader)
		{
			//v2
			BufferStart = await reader.ReadUInt32Async();
			BufferLength = await reader.ReadUInt32Async();
			TypeID = await reader.ReadInt32Async();
			ClassID = await reader.ReadInt16Async();
			ScriptTypeIndex = await reader.ReadInt16Async();
			//v3
			Stripped = await reader.ReadBoolAsync();
		}
	}
}