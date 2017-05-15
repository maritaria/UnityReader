using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	[UnityType(21)]
	public sealed class Material : NamedAssetData
	{
		public string Name { get; set; }
		public AssetReference<Shader> Shader { get; set; }
		public string ShaderKeywords { get; set; }
		public uint LightmapFlags { get; set; }
		public int CustomRenderQueue { get; set; }
		public IDictionary<string, string> StringTagMap { get; } = new Dictionary<string, string>();

		public UnityPropertySheet SavedProperties { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Name = reader.ReadStringFixed(reader.ReadInt32());
			Shader = reader.Read<AssetReference<Shader>>(owner);
			ShaderKeywords = reader.ReadStringFixed(reader.ReadInt32());
			LightmapFlags = reader.ReadUInt32();
			CustomRenderQueue = reader.ReadInt32();

			int count = reader.ReadInt32();
			StringTagMap.Clear();
			for (int i = 0; i < count; i++)
			{
				string key = reader.ReadStringFixed(reader.ReadInt32());
				string value = reader.ReadStringFixed(reader.ReadInt32());
				StringTagMap.Add(key, value);
			}

			SavedProperties = reader.Read<UnityPropertySheet>(owner);
		}
	}
}