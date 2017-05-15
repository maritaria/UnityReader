namespace UnityReader.Types
{
	public class Vector4 : Vector3
	{
		public float W { get; set; }
		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			base.Read(owner, reader);
			W = reader.ReadFloat();
		}
	}
}