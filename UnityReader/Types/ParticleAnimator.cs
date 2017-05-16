using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.ParticleAnimator)]
	public sealed class ParticleAnimator : AssetObject
	{
		public AssetReference<GameObject> GameObject { get; set; }
		public bool AnimatesColor { get; set; }
#warning Must be 5 elements long exactly
		public ColorByteRgba[] AnimationColors { get; set; }
		public Vector3 WorldRotationAxis { get; set; }
		public Vector3 LocalRotationAxis { get; set; }
		public float SizeGrow { get; set; }
		public Vector3 RandomForce { get; set; }
		public Vector3 Force { get; set; }
		public float Damping { get; set; }
		public bool StopSimulation { get; set; }
		public bool AutoDestruct { get; set; }
	}
}