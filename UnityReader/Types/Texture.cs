using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.Texture)]
	public abstract class Texture : AssetObject
	{
		public virtual string Name { get; set; }
	}
}