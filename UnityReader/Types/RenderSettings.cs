using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	[UnityType(104)]
	public sealed class RenderSettings : AssetData
	{
		public bool FogEnabled { get; set; }
		public ColorFloatRgba FogColor { get; set; }
		public FogSettings FogMode { get; set; }
		public float FogDensity { get; set; }
		public float LinearFogStart { get; set; }
		public float LinearFogEnd { get; set; }
		public ColorFloatRgba AmbientSkyColor { get; set; }
		public ColorFloatRgba AmbientEquatorColor { get; set; }
		public ColorFloatRgba AmbientGroundColor { get; set; }
		public float AmbientIntensity { get; set; }
		public AmbientSettings AmbientMode { get; set; }
		public AssetReference<Material> SkyboxMaterial { get; set; }
		public float HaloStrength { get; set; }
		public float FlareStrength { get; set; }
		public float FlareFadeSpeed { get; set; }
		public AssetReference<Texture2D> HaloTexture { get; set; }
		public AssetReference<Texture2D> SpotCookie { get; set; }
		public ReflectionMode DefaultReflectionMode { get; set; }
		public int DefaultReflectionResolution { get; set; }
		public int ReflectionBounces { get; set; }
		public float ReflectionIntensity { get; set; }
		public AssetReference<Cubemap> CustomReflection { get; set; }
		public float[] SphericalHarmonicsL2 { get; set; }
		public AssetReference<Cubemap> GeneratedSkyboxReflection { get; set; }
		public AssetReference<Light> Sun { get; set; }
		public ColorFloatRgba IndirectSpecularColor { get; set; }
		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			FogEnabled = reader.ReadBool();
			FogColor = reader.Read<ColorFloatRgba>(owner);
			FogMode = (FogSettings)reader.ReadInt32();
			FogDensity = reader.ReadFloat();
			LinearFogStart = reader.ReadFloat();
			LinearFogEnd = reader.ReadFloat();

			AmbientSkyColor = reader.Read<ColorFloatRgba>(owner);
			AmbientEquatorColor = reader.Read<ColorFloatRgba>(owner);
			AmbientGroundColor = reader.Read<ColorFloatRgba>(owner);
			AmbientIntensity = reader.ReadFloat();
			AmbientMode = (AmbientSettings)reader.ReadInt32();

			SkyboxMaterial = reader.Read<AssetReference<Material>>(owner);
			HaloStrength = reader.ReadFloat();
			FlareStrength = reader.ReadFloat();
			FlareFadeSpeed = reader.ReadFloat();
			HaloTexture = reader.Read<AssetReference<Texture2D>>(owner);
			SpotCookie = reader.Read<AssetReference<Texture2D>>(owner);

			DefaultReflectionMode = (ReflectionMode)reader.ReadInt32();
			DefaultReflectionResolution = reader.ReadInt32();
			ReflectionBounces = reader.ReadInt32();
			ReflectionIntensity = reader.ReadFloat();

			CustomReflection = reader.Read<AssetReference<Cubemap>>(owner);
			SphericalHarmonicsL2 = new float[27];
			for (int i = 0; i < SphericalHarmonicsL2.Length; i++)
			{
				SphericalHarmonicsL2[i] = reader.ReadFloat();
			}
			GeneratedSkyboxReflection = reader.Read<AssetReference<Cubemap>>(owner);
			Sun = reader.Read<AssetReference<Light>>(owner);
			IndirectSpecularColor = reader.Read<ColorFloatRgba>(owner);
		}

		public enum FogSettings
		{
		}

		public enum AmbientSettings
		{
		}

		public enum ReflectionMode
		{
		}
	}
}