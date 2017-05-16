using System;

namespace UnityReader.Types
{
	public sealed class LightmapData
	{
		public AssetReference<Texture2D> Lightmap { get; set; }
		public AssetReference<Texture2D> IndirectLightmap { get; set; }
	}
}