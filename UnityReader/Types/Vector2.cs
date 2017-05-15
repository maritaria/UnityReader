namespace UnityReader.Types
{
	public class Vector2 : AssetData
	{
		public float X { get; set; }
		public float Y { get; set; }

		public virtual void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			X = reader.ReadFloat();
			Y = reader.ReadFloat();
		}
	}
}