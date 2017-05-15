using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	[UnityType(5)]
	public sealed class TimeManager : GlobalGameManager
	{
		public float FixedTimestep { get; set; }
		public float MaximumAllowedTimestep { get; set; }
		public float TimeScale { get; set; }
		public float MaximumParticleTimestep { get; set; }

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			FixedTimestep = reader.ReadFloat();
			MaximumAllowedTimestep = reader.ReadFloat();
			TimeScale = reader.ReadFloat();
			MaximumParticleTimestep = reader.ReadFloat();
		}
	}
}
