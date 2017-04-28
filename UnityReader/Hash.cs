using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class Hash
	{
		private byte[] _hash;
		public Hash()
		{

		}
		public async Task Read(BinaryReader reader)
		{
			_hash = await reader.ReadBytesAsync(16);
		}
	}
}
