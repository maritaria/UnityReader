using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	[UnityType(114)]
	public class MonoBehaviour : Component
	{
		public bool Enabled { get; set; }
		public AssetReference<MonoScript> Script { get; set; }
		public string Name { get; set; }
		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			base.Read(owner, reader);
			Enabled = reader.ReadBool();
			Script = new AssetReference<MonoScript>();
			Script.Read(owner, reader);
			Name = reader.ReadStringFixed(reader.ReadInt32());
		}
	}

}
