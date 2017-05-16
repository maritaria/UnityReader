using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityReader
{
	public sealed class UnityWriter : IDisposable
	{
		public Stream _stream;
		public bool IsLittleEndian { get; set; } = BitConverter.IsLittleEndian;

		public UnityWriter(Stream target)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			_stream = target;
		}

		public static UnityWriter CreateNullWriter()
		{
			return new UnityWriter(new MemoryStream());
		}

		public void Write(byte value)
		{
			Write(new byte[] { value });
		}

		public void Write(short value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(ushort value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(int value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(uint value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(long value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(ulong value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(byte[] bytes)
		{
			if (IsLittleEndian != BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void WriteString(string str)
		{
			Write(Encoding.ASCII.GetBytes(str));
		}

		public void Align(int blockSize = 4)
		{
			int padding = (int)(_stream.Position % blockSize);
			_stream.Position += padding;
		}

		public void Dispose()
		{
			_stream.Dispose();
		}
	}
}