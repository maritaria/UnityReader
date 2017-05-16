using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.LightmapSettings)]
	public sealed class LightmapSettings : AssetObject
	{
		public EnlightenSceneMapping EnlightenSceneMapping { get; set; }
		public AssetReference<LightProbes> LightProbes { get; set; }
		public ICollection<LightmapData> Lightmaps { get; } = new List<LightmapData>();
		public int LightmapsMode { get; set; }
		public GISettings Settings { get; set; }
		public int RuntimeCpuUsage { get; set; }

	}
}