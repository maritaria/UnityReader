struct AssetFileInfoEx *__thiscall AssetsFileTable::getAssetInfo(
	AssetsFileTable *this,
	unsigned __int64 pathId)
{
	if ( !this->assetFileInfoCount )
	{
		return 0;
	}
	for (int i = 0; i < this->assetFileInfoCount; i++)
	{
		if (this->pAssetFileInfo[i]->index == pathId)
		{
			return &this->pAssetFileInfo[i];
		}
	}
}