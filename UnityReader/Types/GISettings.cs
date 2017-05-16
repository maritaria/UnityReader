using System;

namespace UnityReader.Types
{
	public sealed class GISettings
	{
		public float BounceScale { get; set; }
		public float IndirectOutputScale { get; set; }
		public float AlbedoBoost { get; set; }
		public float TemporalCoherenceThreshold { get; set; }
		public uint EnvironemntLightingMode { get; set; }
		public bool EnableBakedLightmaps { get; set; }
		public bool EnableRealtimeLightmaps { get; set; }
	}
}