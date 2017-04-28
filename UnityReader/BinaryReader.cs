using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class BinaryReader
	{
		private Stream _stream;
		public long Position
		{
			get { return _stream.Position; }
			set { _stream.Position = value; }
#warning Build system to load stuff in memory instead
		}
		public bool IsLittleEndian { get; set; } = BitConverter.IsLittleEndian;

		public BinaryReader(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			_stream = stream;
		}

		public async Task<bool> ReadBoolAsync()
		{
			return (await ReadByteAsync()) != 0x00;
		}

		public async Task<short> ReadInt16Async()
		{
			byte[] buffer = await ReadBytesAsync(2);
			return BitConverter.ToInt16(buffer, 0);
		}

		public async Task<char> ReadCharAsync()
		{
			byte[] buffer = await ReadBytesAsync(2);
			return BitConverter.ToChar(buffer, 0);
		}

		public async Task<int> ReadInt32Async()
		{
			IEnumerable<byte> buffer = await ReadBytesAsync(4);
			return BitConverter.ToInt32(buffer.ToArray(), 0);
		}

		public async Task<uint> ReadUInt32Async()
		{
			byte[] buffer = await ReadBytesAsync(4);
			return BitConverter.ToUInt32(buffer, 0);
		}

		public async Task<long> ReadInt64Async()
		{
			byte[] buffer = await ReadBytesAsync(8);
			return BitConverter.ToInt64(buffer, 0);
		}

		public async Task<byte> ReadByteAsync()
		{
			return (await ReadBytesAsync(1))[0];
		}

		public async Task<byte[]> ReadBytesAsync(int count)
		{
			byte[] result = new byte[count];
			int read = 0;
			while (read < count)
			{
				read += await _stream.ReadAsync(result, read, count - read);
			}
			if (IsLittleEndian != BitConverter.IsLittleEndian)
			{
				Array.Reverse(result);
			}
			return result;
		}

		public async Task<string> ReadNullStringAsync(int limit, Encoding encoding)
		{
			List<byte> data = new List<byte>();
			for (int i = 0; i < limit; i++)
			{
				byte raw = await ReadByteAsync();
				if (raw == 0x00)
				{
					break;
				}
				data.Add(raw);
			}
			return encoding.GetString(data.ToArray());
		}

		public Task<string> ReadNullStringAsync(int limit)
		{
			return ReadNullStringAsync(limit, Encoding.ASCII);
		}

		public Task<string> ReadNullStringAsync()
		{
			return ReadNullStringAsync(256);
		}

		public void Align(int blockSize)
		{
			if (blockSize == 0)
			{
				return;
			}
			long pos = Position;
			long blockProgress = pos % blockSize;
			if (blockProgress != 0)
			{
				int remaining = (int)(blockSize - blockProgress);
				_stream.Position += remaining;
			}
		}

	}
}