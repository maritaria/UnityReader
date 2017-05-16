using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlexParse
{
	public sealed class FlexReader : IDisposable
	{
		public Stream BaseStream { get; }
		public bool IsLittleEndian { get; set; } = BitConverter.IsLittleEndian;

		public FlexReader(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (!stream.CanRead) throw new ArgumentException("Stream is not readable", nameof(stream));
			BaseStream = stream;
		}

		public void Dispose()
		{
			BaseStream.Dispose();
		}

		public byte[] ReadBytes(int count)
		{
			byte[] result = new byte[count];
			int remaining = count;
			while (remaining > 0)
			{
				remaining -= BaseStream.Read(result, count - remaining, remaining);
			}
			return result;
		}

		public byte[] ReadBytesWithEndianSwap(int count)
		{
			byte[] result = ReadBytes(count);
			if (IsLittleEndian != BitConverter.IsLittleEndian)
			{
				System.Array.Reverse(result);
			}
			return result;
		}

		public bool ReadBool()
		{
			return ReadByte() != 0x00;
		}

		public byte ReadByte()
		{
			return ReadBytes(1)[0];
		}

		public short ReadInt16()
		{
			byte[] bytes = ReadBytesWithEndianSwap(8);
			return BitConverter.ToInt16(bytes, 0);
		}

		public int ReadInt32()
		{
			byte[] bytes = ReadBytesWithEndianSwap(4);
			return BitConverter.ToInt32(bytes, 0);
		}

		public long ReadInt64()
		{
			byte[] bytes = ReadBytesWithEndianSwap(8);
			return BitConverter.ToInt64(bytes, 0);
		}

		public float ReadFloat()
		{
			byte[] bytes = ReadBytesWithEndianSwap(4);
			return BitConverter.ToSingle(bytes, 0);
		}

		public double ReadDouble()
		{
			byte[] bytes = ReadBytesWithEndianSwap(8);
			return BitConverter.ToDouble(bytes, 0);
		}

		public string ReadFixedString(int length)
		{
			byte[] buffer = ReadBytes(length);
			return Encoding.ASCII.GetString(buffer);
		}

		public string ReadNullTerminatedString()
		{
			List<byte> buffer = new List<byte>();
			while (true)
			{
				byte current = ReadByte();
				if (current == 0x00)
				{
					break;
				}
				buffer.Add(current);
			}
			return Encoding.ASCII.GetString(buffer.ToArray());
		}

		public string ReadLengthSuffixedString()
		{
			return ReadFixedString(ReadInt32());
		}

		public void Align(int blockSize)
		{
			int remainingBlock = blockSize - (int)(BaseStream.Position % blockSize);
			if (BaseStream.CanSeek)
			{
				BaseStream.Position += remainingBlock;
			}
			else
			{
				ReadBytes(remainingBlock);
			}
		}

		public void Skip(int amount)
		{
			if (BaseStream.CanSeek)
			{
				BaseStream.Position += amount;
			}
			else
			{
				ReadBytes(amount);
			}
		}
	}
}