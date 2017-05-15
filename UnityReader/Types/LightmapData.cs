using System;

namespace UnityReader.Types
{
	public sealed class LightmapData : AssetData
	{
		public AssetReference<Texture2D> Lightmap { get; set; }
		public AssetReference<Texture2D> IndirectLightmap { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Lightmap = reader.Read<AssetReference<Texture2D>>(owner);
			IndirectLightmap = reader.Read<AssetReference<Texture2D>>(owner);
		}
	}
}