bool __thiscall AssetsFile::GetAssetFile(
	AssetsFile *this,
	unsigned __int64 fileInfoOffset,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	struct AssetFile *pBuf,
	__int32 readerPar)
{
	unsigned int format; // eax@3
	signed int byteCount; // eax@5
	bool v9; // zf@8
	__int64 v10; // rax@10
	unsigned int v16; // ecx@12
	int v12; // eax@14
	int v13; // edx@14
	
	int v14[8];

	if ( !callback )
	{
		return false;
	}
	int format = this->header->format;
	if ( format >= 17 )
	{
		byteCount = 20;
	}
	else if ( format == 16 )
	{
		byteCount = 23;
	}
	else if ( format == 15 )
	{
		byteCount = 25;
	}
	else if ( format == 14 )
	{
		byteCount = 24;
	}
	else
	{
		byteCount = 20;
	}
	v10 = callback(fileInfoOffset, byteCount, &v14, readerPar);
	if ( !v10 )
		return 0;
	
	
	SWAP_ENDIAN( this->header->endianness, &v16);
	
	v12 = callback(this->header->offs_firstFile + v16, v17, pBuf, readerPar);
	return v12;
}