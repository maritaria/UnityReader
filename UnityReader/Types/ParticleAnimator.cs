using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	[UnityType(12)]
	public sealed class ParticleAnimator : AssetData
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

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			GameObject = reader.Read<AssetReference<GameObject>>(owner);
			AnimatesColor = reader.ReadBool();
			AnimationColors = new ColorByteRgba[5];
			AnimationColors[0] = reader.Read<ColorByteRgba>(owner);
			AnimationColors[1] = reader.Read<ColorByteRgba>(owner);
			AnimationColors[2] = reader.Read<ColorByteRgba>(owner);
			AnimationColors[3] = reader.Read<ColorByteRgba>(owner);
			AnimationColors[4] = reader.Read<ColorByteRgba>(owner);
			WorldRotationAxis = reader.Read<Vector3>(owner);
			LocalRotationAxis = reader.Read<Vector3>(owner);
			SizeGrow = reader.ReadFloat();
			RandomForce = reader.Read<Vector3>(owner);
			Force = reader.Read<Vector3>(owner);
			Damping = reader.ReadFloat();
			StopSimulation = reader.ReadBool();
			AutoDestruct = reader.ReadBool();
		}
	}
}