using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlexParse
{
	public sealed class FlexWriter : IDisposable
	{
		public Stream BaseStream { get; }
		public bool IsLittleEndian { get; set; } = BitConverter.IsLittleEndian;

		public FlexWriter(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (!stream.CanWrite) throw new ArgumentException("Stream is not writable", nameof(stream));
			BaseStream = stream;
		}

		public void Dispose()
		{
			BaseStream.Dispose();
		}

		public void WriteBytes(byte[] buffer)
		{
			BaseStream.Write(buffer, 0, buffer.Length);
		}

		public void WriteBytesWithEndianSwap(byte[] buffer)
		{
			if (IsLittleEndian != BitConverter.IsLittleEndian)
			{
				System.Array.Reverse(buffer);
			}
			WriteBytes(buffer);
		}

		public void Write(bool value)
		{
			WriteBytes(BitConverter.GetBytes(value));
		}

		public void Write(byte value)
		{
			WriteBytesWithEndianSwap(new byte[] { value });
		}

		public void Write(short value)
		{
			WriteBytesWithEndianSwap(BitConverter.GetBytes(value));
		}

		public void Write(int value)
		{
			WriteBytesWithEndianSwap(BitConverter.GetBytes(value));
		}

		public void Write(long value)
		{
			WriteBytesWithEndianSwap(BitConverter.GetBytes(value));
		}

		public void Write(float value)
		{
			WriteBytesWithEndianSwap(BitConverter.GetBytes(value));
		}

		public void Write(double value)
		{
			WriteBytesWithEndianSwap(BitConverter.GetBytes(value));
		}

		public void Write(string value)
		{
			WriteBytes(Encoding.ASCII.GetBytes(value));
		}

		public void Align(int blockSize)
		{
			int remainingBlock = (int)(BaseStream.Position % blockSize);
			if (BaseStream.CanSeek)
			{
				BaseStream.Position += remainingBlock;
			}
			else
			{
				byte[] block = new byte[remainingBlock];
				BaseStream.Write(block, 0, block.Length);
			}
		}
	}
}