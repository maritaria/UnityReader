using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.TimeManager)]
	public sealed class TimeManager : GlobalGameManager
	{
		public float FixedTimestep { get; set; }
		public float MaximumAllowedTimestep { get; set; }
		public float TimeScale { get; set; }
		public float MaximumParticleTimestep { get; set; }
	}
}