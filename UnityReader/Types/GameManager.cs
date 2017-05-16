using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.GameManager)]
	public abstract class GameManager : AssetObject
	{
	}

	[UnityType(AssetCodes.GlobalGameManager)]
	public abstract class GlobalGameManager : GameManager
	{
	}

	[UnityType(AssetCodes.LevelGameManager)]
	public abstract class LevelGameManager : GameManager
	{
	}
}