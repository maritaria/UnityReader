namespace UnityReader
{

	public sealed class PreloadList : UnityList<PreloadList.AssetPointer>
	{
		public struct AssetPointer : UnityElement
		{
			public int FileID;
			public long PathID;

			void UnityElement.Read(UnityReader reader, int version)
			{
				FileID = reader.ReadInt32();
				PathID = reader.ReadInt64();
			}
		}
	}

}