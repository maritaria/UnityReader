namespace UnityReader.Types
{
	[UnityType(9)]
	public abstract class GameManager : AssetData
	{
		public abstract void Read(AssetsFile owner, UnityBinaryReader reader);
	}

	[UnityType(6)]
	public abstract class GlobalGameManager : GameManager
	{
	}

	[UnityType(3)]
	public abstract class LevelGameManager : GameManager
	{
	}
}