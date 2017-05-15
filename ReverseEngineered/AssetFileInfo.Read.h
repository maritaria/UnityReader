unsigned __int64 __thiscall AssetFileInfo::Read(
	AssetFileInfo *this,
	unsigned __int32 version,
	unsigned __int64 pos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	bool bigEndian)
{
	if ( version >= 14 )
	{
		//Aligning
		pos = (pos + 3) & 0xFFFFFFFC;
	}
	
	int numBytes;
	switch( version )
	{
		case 14: numBytes = 24; break;
		case 15: numBytes = 25; break;
		case 16: numBytes = 23; break;
		default: numBytes = 20; break;
	}
	char buffer[30] = { 0 };
	callback(pos, numBytes, buffer, readerPar);
	
	this->index = 0;
	size_t indexByteCount = version < 14 ? 4 : 8;
	memcpy(&this->index, buffer, indexByteCount);
	
	if ( version < 14 )
	{
		//Endian swap 32 bit
		SWAP_ENDIAN( bigEndian, &this->index );
	}
	else
	{
		//Endian swap 64 bit
		SWAP_ENDIAN( bigEndian, &this->index );
	}
	
	this->offs_curFile = *(int *)buffer[indexByteCount];
	SWAP_ENDIAN( bigEndian, &this->offs_curFile );
	
	this->curFileSize = *(int *)buffer[indexByteCount + 4];
	SWAP_ENDIAN( bigEndian, &this->curFileSize );
	
	this->curFileTypeOrIndex = *(int *)buffer[indexByteCount + 8];
	SWAP_ENDIAN( bigEndian, &this->curFileTypeOrIndex );
	
	offset_halfway = (version < 14) ? 16 : 20;
	if ( version >= 16 )
	{
		this->inheritedUnityClass = 0;
	}
	else
	{
		this->inheritedUnityClass = *(_WORD *)buffer[offset_halfway];
		SWAP_ENDIAN( bigEndian, this->inheritedUnityClass );
		offset_halfway += 2;
	}
	if ( version > 16 )
	{
		this->scriptIndex = -1;
		this->unknown1 = 0;
	}
	else
	{
		this->scriptIndex = *(_WORD *)buffer[offset_halfway];
		SWAP_ENDIAN( bigEndian, &this->scriptIndex );
		offset_halfway += 2;
		
		this->unknown1 = buffer[offset_halfway];
	}
	return pos + numBytes;
}