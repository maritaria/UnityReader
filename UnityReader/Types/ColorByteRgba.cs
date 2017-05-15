using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	public sealed class ColorByteRgba : AssetData
	{
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
		public byte A { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			R = reader.ReadByte();
			G = reader.ReadByte();
			B = reader.ReadByte();
			A = reader.ReadByte();
		}
	}
}
