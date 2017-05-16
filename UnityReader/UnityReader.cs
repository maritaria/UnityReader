using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityReader
{
	public sealed class UnityReader : IDisposable
	{
		public static UnityReader FromByteArray(byte[] array)
		{
			var ms = new MemoryStream(array);
			return new UnityReader(ms);
		}

		private Stream _stream;

		public long Position
		{
			get { return _stream.Position; }
			set { _stream.Position = value; }
		}

		public string PositionHex => string.Format("0x{0:X4}", Position);

		public bool IsLittleEndian { get; set; } = BitConverter.IsLittleEndian;

		public UnityReader(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			_stream = stream;
		}

		public bool ReadBool()
		{
			return (ReadByte()) != 0x00;
		}

		public short ReadInt16()
		{
			byte[] buffer = ReadBytes(2);
			return BitConverter.ToInt16(buffer, 0);
		}

		public ushort ReadUInt16()
		{
			byte[] buffer = ReadBytes(2);
			return BitConverter.ToUInt16(buffer, 0);
		}

		public char ReadChar()
		{
			byte[] buffer = ReadBytes(2);
			return BitConverter.ToChar(buffer, 0);
		}

		public int ReadInt32()
		{
			IEnumerable<byte> buffer = ReadBytes(4);
			return BitConverter.ToInt32(buffer.ToArray(), 0);
		}

		public uint ReadUInt32()
		{
			byte[] buffer = ReadBytes(4);
			return BitConverter.ToUInt32(buffer, 0);
		}

		public long ReadInt64()
		{
			byte[] buffer = ReadBytes(8);
			return BitConverter.ToInt64(buffer, 0);
		}

		public ulong ReadUInt64()
		{
			byte[] buffer = ReadBytes(8);
			return BitConverter.ToUInt64(buffer, 0);
		}

		public byte ReadByte()
		{
			return (ReadBytes(1))[0];
		}

		public byte[] ReadBytes(int count)
		{
			byte[] result = new byte[count];
			int readAlready = 0;
			while (readAlready < count)
			{
				int read = _stream.Read(result, readAlready, count - readAlready);
				if (read == 0)
				{
					throw new EndOfStreamException();
				}
				readAlready += read;
			}
			if (IsLittleEndian != BitConverter.IsLittleEndian)
			{
				Array.Reverse(result);
			}
			return result;
		}

		public string ReadString(Encoding encoding)
		{
			List<byte> data = new List<byte>();
			while (true)
			{
				byte raw = ReadByte();
				if (raw == 0x00)
				{
					break;
				}
				data.Add(raw);
			}
			return encoding.GetString(data.ToArray());
		}

		public string ReadString()
		{
			return ReadString(Encoding.ASCII);
		}

		public string ReadStringFixed(int length, Encoding encoding)
		{
			byte[] data = ReadBytes(length);
			return encoding.GetString(data);
		}

		public string ReadStringFixed(int length)
		{
			return ReadStringFixed(length, Encoding.ASCII);
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

		public float ReadFloat()
		{
			byte[] bytes = ReadBytes(4);
			return BitConverter.ToSingle(bytes, 0);
		}

		public double ReadDouble()
		{
			byte[] bytes = ReadBytes(8);
			return BitConverter.ToDouble(bytes, 0);
		}

		public void Dispose()
		{
			((IDisposable)_stream).Dispose();
		}
	}
}