using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Transform)]
	public class Transform : Component
	{
		public Quaternion LocalRotation { get; } = new Quaternion();
		public Vector3 LocalPosition { get; } = new Vector3();
		public Vector3 LocalScale { get; } = new Vector3();
		public ICollection<AssetReference<Transform>> Children { get; } = new List<AssetReference<Transform>>();
		public AssetReference<Transform> Parent { get; set; }
	}
}