namespace UnityReader.Types
{

	public class Component : AssetData
	{
		public AssetReference<GameObject> GameObject { get; set; }

		public virtual void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			GameObject = new AssetReference<GameObject>();
			GameObject.Read(owner, reader);
		}
	}

}