using System;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Texture2D)]
	public class Texture2D : Texture
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public int CompleteImageSize { get; set; }
		public Format TextureFormat { get; set; }
		public int MipCount { get; set; }
		public bool IsReadable { get; set; }
		public int ImageCount { get; set; }
		public int TextureDimension { get; set; }
		public GLTextureSettings TextureSettings { get; set; }
		public int LightmapFormat { get; set; }
		public int ColorSpace { get; set; }
		public byte[] ImageData { get; set; }
		public StreamingInfo StreamData { get; set; }

		public enum Format
		{

		}

		public sealed class GLTextureSettings
		{
			public int FilterMode { get; set; }
			public int Aniso { get; set; }
			public int MipBias { get; set; }
			public int WrapMode { get; set; }
			public void Read(AssetsFile owner, UnityReader reader)
			{
				FilterMode = reader.ReadInt32();
				Aniso = reader.ReadInt32();
				MipBias = reader.ReadInt32();
				WrapMode = reader.ReadInt32();
			}
		}

		public sealed class StreamingInfo
		{
			public uint Offset { get; set; }
			public uint Size { get; set; }
			public string Path { get; set; }

			public void Read(AssetsFile owner, UnityReader reader)
			{
				Offset = reader.ReadUInt32();
				Size = reader.ReadUInt32();
				Path = reader.ReadStringFixed(reader.ReadInt32());
			}
		}
	}
}