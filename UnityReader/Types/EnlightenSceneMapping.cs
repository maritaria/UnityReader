using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public sealed class EnlightenSceneMapping
	{
		public ICollection<EnlightenRenderer> Renderers { get; } = new List<EnlightenRenderer>();
		public ICollection<EnlightenSystem> Systems { get; } = new List<EnlightenSystem>();
		public ICollection<Hash128> ProbeSets { get; } = new List<Hash128>();
		public ICollection<Atlas> SystemAtlasses { get; } = new List<Atlas>();
		public ICollection<ChunkInfo> TerrainChunks { get; } = new List<ChunkInfo>();

		public sealed class EnlightenRenderer
		{
			public AssetReference<AssetObject> Renderer { get; set; }
			public Vector4 DynamicLightmapSTInSystem { get; set; }
			public int SystemID { get; set; }
			public Hash128 InstanceHash { get; set; }
		}

		public sealed class EnlightenSystem
		{
			public uint RenderIndex { get; set; }
			public uint RenderSize { get; set; }
			public int AtlasIndex { get; set; }
			public int AtlasOffsetX { get; set; }
			public int AtlasOffsetY { get; set; }
			public Hash128 InputSystemHash { get; set; }
			public Hash128 RadiositySystemHash { get; set; }
		}

		public sealed class Atlas
		{
			public int Size { get; set; }
			public Hash128 Hash { get; set; }
		}

		public sealed class ChunkInfo
		{
			public int FirstSystemID { get; set; }
			public int NumChunksInX { get; set; }
			public int NumChunksInY { get; set; }
		}
	}
}