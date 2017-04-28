using System;
using System.Linq;

namespace UnityReader
{
	public class UnityHash
	{
		private byte[] data;

		public UnityHash(byte[] data)
		{
			this.data = data;
		}

		public override string ToString()
		{
			return string.Join(" ", data.Select(b => string.Format("{0:X2}", b)));
		}
	}
}