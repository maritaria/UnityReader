using System;

namespace UnityReader.Types
{
	[UnityType(28)]
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

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Name = reader.ReadStringFixed(reader.ReadInt32());
			Width = reader.ReadInt32();
			Height = reader.ReadInt32();
			CompleteImageSize = reader.ReadInt32();
			TextureFormat = (Format)reader.ReadInt32();
			MipCount = reader.ReadInt32();
			IsReadable = reader.ReadBool();
			ImageCount = reader.ReadInt32();
			TextureDimension = reader.ReadInt32();
			TextureSettings = reader.Read<GLTextureSettings>(owner);
			LightmapFormat = reader.ReadInt32();
			ColorSpace = reader.ReadInt32();
			ImageData = reader.ReadBytes(reader.ReadInt32());
			StreamData = reader.Read<StreamingInfo>(owner);
		}

		public enum Format
		{

		}

		public sealed class GLTextureSettings : AssetData
		{
			public int FilterMode { get; set; }
			public int Aniso { get; set; }
			public int MipBias { get; set; }
			public int WrapMode { get; set; }
			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				FilterMode = reader.ReadInt32();
				Aniso = reader.ReadInt32();
				MipBias = reader.ReadInt32();
				WrapMode = reader.ReadInt32();
			}
		}

		public sealed class StreamingInfo : AssetData
		{
			public uint Offset { get; set; }
			public uint Size { get; set; }
			public string Path { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				Offset = reader.ReadUInt32();
				Size = reader.ReadUInt32();
				Path = reader.ReadStringFixed(reader.ReadInt32());
			}
		}
	}
}