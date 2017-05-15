using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public sealed class EnlightenSceneMapping : AssetData
	{
		public ICollection<EnlightenRenderer> Renderers { get; } = new List<EnlightenRenderer>();
		public ICollection<EnlightenSystem> Systems { get; } = new List<EnlightenSystem>();
		public ICollection<Hash128> ProbeSets { get; } = new List<Hash128>();
		public ICollection<Atlas> SystemAtlasses { get; } = new List<Atlas>();
		public ICollection<ChunkInfo> TerrainChunks { get; } = new List<ChunkInfo>();

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			reader.ReadArray(owner, Renderers);
			reader.ReadArray(owner, Systems);
			reader.ReadArray(owner, ProbeSets);
			reader.ReadArray(owner, SystemAtlasses);
			reader.ReadArray(owner, TerrainChunks);
		}

		public sealed class EnlightenRenderer : AssetData
		{
			public AssetReference<AssetData> Renderer { get; set; }
			public Vector4 DynamicLightmapSTInSystem { get; set; }
			public int SystemID { get; set; }
			public Hash128 InstanceHash { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				Renderer = reader.Read<AssetReference<AssetData>>(owner);
				DynamicLightmapSTInSystem = reader.Read<Vector4>(owner);
				SystemID = reader.ReadInt32();
				InstanceHash = reader.Read<Hash128>(owner);
			}
		}

		public sealed class EnlightenSystem : AssetData
		{
			public uint RenderIndex { get; set; }
			public uint RenderSize { get; set; }
			public int AtlasIndex { get; set; }
			public int AtlasOffsetX { get; set; }
			public int AtlasOffsetY { get; set; }
			public Hash128 InputSystemHash { get; set; }
			public Hash128 RadiositySystemHash { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				RenderIndex = reader.ReadUInt32();
				RenderSize = reader.ReadUInt32();
				AtlasIndex = reader.ReadInt32();
				AtlasOffsetX = reader.ReadInt32();
				AtlasOffsetY = reader.ReadInt32();
				InputSystemHash = reader.Read<Hash128>(owner);
				RadiositySystemHash = reader.Read<Hash128>(owner);
			}
		}

		public sealed class Atlas : AssetData
		{
			public int Size { get; set; }
			public Hash128 Hash { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				Size = reader.ReadInt32();
				Hash = reader.Read<Hash128>(owner);
			}
		}

		public sealed class ChunkInfo : AssetData
		{
			public int FirstSystemID { get; set; }
			public int NumChunksInX { get; set; }
			public int NumChunksInY { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				FirstSystemID = reader.ReadInt32();
				NumChunksInX = reader.ReadInt32();
				NumChunksInY = reader.ReadInt32();
			}
		}

	}
}