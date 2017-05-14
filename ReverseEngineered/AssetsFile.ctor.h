void AssetsFile::SelfDestruct()
{
	this->preloadTable->len = 0;
	this->preloadTable->items = NULL;
	this->AssetTablePos = 0;
	this->this->AssetCount = 0;
	this->typeTree->fieldCount = 0;
	this->dependencies->len = 0;
	this->dependencies->items = NULL;
}

AssetsFileHeader *__thiscall AssetsFile::AssetsFile(
	AssetsFileHeader *this,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar)
{
	callback(0, 0, 0, readerPar); // opens file?
	this->reader = callback;
	this->readerPar = readerPar;
	unsigned long filePos = this->header::Read(0, callback, readerPar);
	if ( !this->version || this->version > 64 )
	{
		this->SelfDestruct();
		return this;
	}
	if ( this->version < 9 )
	{
		filePos = this->header->fileSize - this->header->metadataSize;
	}
	filePos = this->typeTree::Read(filePos, callback, readerPar, this->version, this->header->endianness);
	if ( this->typeTree->unityVersion < '0' || this->typeTree->unityVersion > '9' )
	{
		this->SelfDestruct();
		return this;
	}
	this->AssetTablePos = filePos;
	this->AssetCount = 0;
	callback(filePos, 4, &this->AssetCount, readerPar);
	SWAP_ENDIAN( this->header->endianness), this->AssetCount );
	filePos += 4;
	this->AssetCount;
	if ( this->version >= 14 && this->AssetCount )
	{
		//Rounding
		filePos = (filePos + 3) & 0xFFFFFFFC;
	}
	filePos += AssetFileList::GetSizeBytes((AssetFileList *)&this->AssetCount, this->version);
	if ( this->version < 0xB )
	{
		this->preloadTable->len = 0;
		this->preloadTable->items = NULL;
	}
	else
	{
		filePos = this->preloadTable::Read(filePos, callback, readerPar, this->version, this->header->endianness);
	}
	this->dependencies::Read(filePos, callback, readerPar, this->version, this->header->endianness);
	
	return this;
}