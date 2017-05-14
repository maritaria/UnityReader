#define MAX_STRINGLEN (255)
int __thiscall AssetsFileDependency::Read(
	AssetsFileDependency *this,
	unsigned __int64 filePos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	bool endianness)
{
	// bufferedPath
	callback(filePos, MAX_STRINGLEN, this->bufferedPath, readerPar);
	this->bufferedPath[MAX_STRINGLEN] = 0;
	int pathLength = strlen((const char *)this->bufferedPath);
	unsigned long newFilePos = filePos + MAX_STRINGLEN;
	if ( pathLength == MAX_STRINGLEN )
	{
		do
		{
			char buffer[16] = { 0 };
			callback(newFilePos, 16, buffer, readerPar);
			unsigned int charsRead = strlen(buffer);
			newFilePos += charsRead;
		}
		while ( charsRead >= 16 );
		newFilePos += 1;
	}
	
	// guid
	newFilePos = this->guid::Read(newFilePos, callback, readerPar);
	
	// type
	callback(newFilePos, 4, &(this->type), readerPar);
	newFilePos += 4;
	if ( endianness )
	{
		SWAP_ENDIAN(this->type);
	}
	
	// assetPath
	callback(newFilePos, MAX_STRINGLEN, this->assetPath, readerPar);
	this->assetPath[MAX_STRINGLEN] = 0;
	int assetPathLength = strlen((const char *)this->assetPath);
	newFilePos += assetPathLength;
	if ( assetPathLength == MAX_STRINGLEN )
	{
		do
		{
			callback(newFilePos, 16, &create_local_buffer, readerPar);
			unsigned int charsRead = strlen(&create_local_buffer);
			newFilePos += charsRead;
		}
		while ( charsRead >= 16 );
	}
	newFilePos += 1;
	
	return newFilePos;
}