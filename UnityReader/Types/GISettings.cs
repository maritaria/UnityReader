using System;

namespace UnityReader.Types
{
	public sealed class GISettings : AssetData
	{
		public float BounceScale { get; set; }
		public float IndirectOutputScale { get; set; }
		public float AlbedoBoost { get; set; }
		public float TemporalCoherenceThreshold { get; set; }
		public uint EnvironemntLightingMode { get; set; }
		public bool EnableBakedLightmaps { get; set; }
		public bool EnableRealtimeLightmaps { get; set; }


		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			BounceScale = reader.ReadFloat();
			IndirectOutputScale = reader.ReadFloat();
			AlbedoBoost = reader.ReadFloat();
			TemporalCoherenceThreshold = reader.ReadFloat();
			EnvironemntLightingMode = reader.ReadUInt32();
			EnableBakedLightmaps = reader.ReadBool();
			EnableRealtimeLightmaps = reader.ReadBool();
		}
	}
}