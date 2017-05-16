using System.Collections.Generic;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.GameObject)]
	public class GameObject : AssetObject
	{
		public ICollection<AssetReference<Component>> Components { get; } = new List<AssetReference<Component>>();
		public uint Layer { get; set; }
		public string Name { get; set; }
		public ushort Tag { get; set; }
		public bool IsActive { get; set; }
	}

}