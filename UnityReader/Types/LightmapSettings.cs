using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	[UnityType(157)]
	public class LightmapSettings : LevelGameManager
	{
		public EnlightenSceneMapping EnlightenSceneMapping { get; set; }
		public AssetReference<LightProbes> LightProbes { get; set; }
		public ICollection<LightmapData> Lightmaps { get; } = new List<LightmapData>();
		public int LightmapsMode { get; set; }
		public GISettings Settings { get; set; }
		public int RuntimeCpuUsage { get; set; }

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			EnlightenSceneMapping = reader.Read<EnlightenSceneMapping>(owner);
			LightProbes = reader.Read<AssetReference<LightProbes>>(owner);
			reader.ReadArray(owner, Lightmaps);
			LightmapsMode = reader.ReadInt32();
			Settings = reader.Read<GISettings>(owner);
			RuntimeCpuUsage = reader.ReadInt32();
		}
	}
}