using System;

namespace UnityReader.Types
{
	public sealed class UnityTexEnv
	{
		public AssetReference<Texture> Texture { get; set; }
		public Vector2 Scale { get; set; }
		public Vector2 Offset { get; set; }
	}
}