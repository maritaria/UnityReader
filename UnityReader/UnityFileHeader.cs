using System;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class UnityFileHeader
	{
		private int _reservedCount = 3;
		public int MetaSize { get; set; }
		public uint FileSize { get; set; }
		public int Version { get; set; }
		public uint DataOffset { get; set; }
		public bool IsLittleEndian { get; set; }

		public async Task ReadAsync(BinaryReader reader)
		{
			reader.IsLittleEndian = false;
			MetaSize = await reader.ReadInt32Async();
			FileSize = await reader.ReadUInt32Async();
			Version = await reader.ReadInt32Async();
			DataOffset = await reader.ReadUInt32Async();
			if (Version >= 9)
			{
				IsLittleEndian = !(await reader.ReadBoolAsync());
				await reader.ReadBytesAsync(_reservedCount);
			}
		}
	}
}