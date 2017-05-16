using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Component)]
	public class Component : AssetObject
	{
		public AssetReference<GameObject> GameObject { get; set; }
	}
}