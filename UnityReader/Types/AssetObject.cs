using System;

namespace UnityReader.Types
{
	public abstract class AssetObject : AssetData
	{
		public abstract void Read(AssetsFile owner, UnityBinaryReader reader);
	}
}