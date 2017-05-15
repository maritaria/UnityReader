using System;

namespace UnityReader.Types
{
	public sealed class UnityTexEnv : AssetData
	{
		public AssetReference<Texture> Texture { get; set; }
		public Vector2 Scale { get; set; }
		public Vector2 Offset { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Texture = reader.Read<AssetReference<Texture>>(owner);
			Scale = reader.Read<Vector2>(owner);
			Offset = reader.Read<Vector2>(owner);
		}
	}
}