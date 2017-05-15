using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	[UnityType(4)]
	public class Transform : Component
	{
		public Quaternion LocalRotation { get; } = new Quaternion();
		public Vector3 LocalPosition { get; } = new Vector3();
		public Vector3 LocalScale { get; } = new Vector3();
		public ICollection<AssetReference<Transform>> Children { get; } = new List<AssetReference<Transform>>();
		public AssetReference<Transform> Parent { get; set; }

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			base.Read(owner, reader);
			LocalRotation.Read(owner, reader);
			LocalPosition.Read(owner, reader);
			LocalScale.Read(owner, reader);
			reader.ReadArray(owner, Children);
			Parent = reader.Read<AssetReference<Transform>>(owner);
		}
	}
}