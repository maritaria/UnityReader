using System;
using System.Collections.Generic;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Cubemap)]
	public sealed class Cubemap : Texture2D
	{
		public ICollection<AssetReference<Texture2D>> SourceTextures { get; } = new List<AssetReference<Texture2D>>();
	}
}