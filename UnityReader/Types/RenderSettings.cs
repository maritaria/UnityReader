using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.RenderSettings)]
	public sealed class RenderSettings : AssetObject
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