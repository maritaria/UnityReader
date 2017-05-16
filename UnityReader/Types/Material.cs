using System;
using System.Collections.Generic;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Material)]
	public sealed class Material : AssetObject
	{
		public string Name { get; set; }
		public AssetReference<Shader> Shader { get; set; }
		public string ShaderKeywords { get; set; }
		public uint LightmapFlags { get; set; }
		public int CustomRenderQueue { get; set; }
		public IDictionary<string, string> StringTagMap { get; } = new Dictionary<string, string>();
		public UnityPropertySheet SavedProperties { get; set; }
	}
}