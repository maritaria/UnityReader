using System;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class UnityGuid
	{
		public Guid Value { get; set; }

		public async Task Read(BinaryReader reader)
		{
			bool endian = reader.IsLittleEndian;
			try
			{
				byte[] bytes = await reader.ReadBytesAsync(16);
				Value = new Guid(bytes);
			}
			finally
			{
				reader.IsLittleEndian = endian;
			}
		}
	}
}