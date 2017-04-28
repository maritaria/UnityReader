using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityReader
{
	public class BinarySection : IDisposable
	{
		private Stream _stream;
		public long Start { get; }
		public int Length { get; private set; }
		public byte[] Data { get; private set; }

		public BinarySection(Stream stream)
		{
			_stream = stream;
			Start = stream.Position;
		}

		void IDisposable.Dispose()
		{
			Length = (int)(_stream.Position - Start);
			_stream.Position = Start;
			Data = new byte[Length];
			_stream.Read(Data, 0, Length);
		}
	}
}