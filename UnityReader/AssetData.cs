using System.Diagnostics;

namespace UnityReader
{
	public interface AssetData
	{
		void Read(AssetsFile owner, UnityBinaryReader reader);
	}
}