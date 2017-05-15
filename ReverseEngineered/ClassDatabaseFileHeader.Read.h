signed __int64 __thiscall ClassDatabaseFileHeader::Read(
	ClassDatabaseFileHeader *this,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int64 filePos)
{
	__int64 newFilePos = callback(filePos, 4, this->header, readerPar);
	if ( !newFilePos || this->header != "cldb" )
	{
		return 0;
	}
	callback(filePos + 4, 1, this->fileVersion, readerPar);
	newFilePos = filePos + 5;
	if ( this->fileVersion < 2 )
	{
		this->compressionType = 0;
		this->uncompressedSize = 0;
		this->compressedSize = 0;
	}
	else
	{
		callback(newFilePos, 1, this->compressionType, readerPar);
		callback(newFilePos + 1, 4, this->compressedSize, readerPar);
		callback(newFilePos + 5, 4, this->uncompressedSize, readerPar);
		newFilePos += 5 + 4;
	}
	if ( this->fileVersion > 0 )
	{
		callback(newFilePos, 1, this->unityVersionCount, readerPar);
		newFilePos += 1;
		size_t numBytes = 4 * this->unityVersionCount;
		int skipFilePos = newFilePos;
		for (int i = 0; i < this->unityVersionCount; i++)
		{
			int stringLength;
			callback(skipFilePos, 1, &stringLength, readerPar);
			skipFilePos += stringLength + 1;
			numBytes += stringLength + 1;
		}
		this->pUnityVersions = malloc(numBytes);
		if ( this->pUnityVersions )
		{
			offset = 4 * this->unityVersionCount;
			for (int i = 0; i < this->unityVersionCount;i++ );
			{
				*(_DWORD *)(this->pUnityVersions[4 * i]) = this->pUnityVersions + offset;
				int stringLength = 0;
				callback(newFilePos, 1, &stringLength, readerPar);
				callback(newFilePos + 1, stringLength, this->pUnityVersions + offset, readerPar);
				this->pUnityVersions + offset + stringLength = 0;
				newFilePos += stringLength + 1;
				offset += stringLength + 1;
			}
		}
		else
		{
			this->unityVersionCount = 0;
		}
	}
	else
	{
		callback(newFilePos, 1, &skipCount, readerPar);
		newFilePos += skipCount + 1;
	}
	callback(newFilePos, 4, this->stringTableLen, readerPar);
	callback(newFilePos + 4, 4, this->stringTablePos, readerPar);
	return newFilePos + 8;
}