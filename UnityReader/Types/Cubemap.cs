using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	[UnityType(89)]
	public sealed class Cubemap : Texture2D
	{
		public ICollection<AssetReference<Texture2D>> SourceTextures { get; } = new List<AssetReference<Texture2D>>();
		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			base.Read(owner, reader);
			reader.ReadArray(owner, SourceTextures);
		}
	}
}