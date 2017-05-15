using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{

	public class Quaternion : AssetData
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float W { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			X = reader.ReadFloat();
			Y = reader.ReadFloat();
			Z = reader.ReadFloat();
			W = reader.ReadFloat();
		}
	}

}
