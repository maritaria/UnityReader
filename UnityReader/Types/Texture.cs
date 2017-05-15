namespace UnityReader.Types
{
	public abstract class Texture : NamedAssetData
	{
		public virtual string Name { get; set; }

		public abstract void Read(AssetsFile owner, UnityBinaryReader reader);
	}
}