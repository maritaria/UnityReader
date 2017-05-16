using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexParse
{
	public sealed class UnityHeader
	{
		public long DataOffset { get; }
		public int DataSize { get; }
		public long MetadataOffset { get; }
		public int MetadataSize { get; }
		public int Format { get; }
		public bool IsLittleEndian { get; }
		public byte[] Unknown { get; }
	}
}
