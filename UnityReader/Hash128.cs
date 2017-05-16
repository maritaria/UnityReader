using System;
using System.Linq;

namespace UnityReader
{
	public sealed class Hash128
	{
		public byte[] Bytes { get; set; }

		public void Read(UnityReader reader, int version)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			Bytes = reader.ReadBytes(16);
		}

		public override string ToString()
		{
			if (Bytes == null || Bytes.Length == 0)
			{
				return string.Empty;
			}
			else
			{
				return string.Join(" ", Bytes.Select(b => string.Format("{0:X2}", b)));
			}
		}
	}
}