using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{

	public class Vector3 : Vector2
	{
		public float Z { get; set; }

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			base.Read(owner, reader);
			Z = reader.ReadFloat();
		}
	}

}
