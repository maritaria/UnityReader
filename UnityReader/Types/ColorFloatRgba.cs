using System;

namespace UnityReader.Types
{
	public class ColorFloatRgba : AssetData
	{
		public float R { get; set; }
		public float B { get; set; }
		public float G { get; set; }
		public float A { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			R = reader.ReadFloat();
			B = reader.ReadFloat();
			G = reader.ReadFloat();
			A = reader.ReadFloat();
		}
	}
}