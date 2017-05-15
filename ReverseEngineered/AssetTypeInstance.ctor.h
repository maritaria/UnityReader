int __thiscall AssetTypeInstance::AssetTypeInstance(
	AssetTypeInstance *this,
	int baseFieldCount,
	AssetTypeTemplateField **ppBaseFields,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	bool bigEndian,
	unsigned __int64 filePos)
{
	this->baseFields = NULL;
	this->allocationCount = NULL;
	this->allocationBufLen = NULL;
	this->memoryToClear = NULL;
	
	this->baseFieldCount = baseFieldCount;
	if ( !baseFieldCount )
	{
		return this;
	}
	
	this->baseFields = malloc(4 * baseFieldCount);
	if ( !this->baseFields )
	{
		this->baseFieldCount = 0;
		return this;
	}
	
	this->memoryToClear = malloc(4 * baseFieldCount);
	this->allocationBufLen = baseFieldCount;
	this->allocationCount = baseFieldCount;
	for (int i = 0; i < baseFieldCount; i++)
	{
		filePos = ppBaseFields[i]->MakeValue(callback,
						readerPar,
						filePos,
						(struct AssetTypeValueField *)this->baseFields[i],
						bigEndian);
		this->memoryToClear[i] = this->baseFields[i];
	}
	return this;
}