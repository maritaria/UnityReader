int __thiscall TypeTree::Read(
	TypeTree *this,
	unsigned __int64 filePos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int32 version,
	bool endianness)
{
	/* Alignment is per 4 bytes */
	
	this->_fmt = version;
	this->hasTypeTree = true;
	
	if ( version <= 6 )
	{
		this->unityVersion = "Unsupported v6";
		this->version = 0;
		if ( version == 6 )
		{
			this->unityVersion = "Unsupported v6";
		}
		else if ( version == 5 )
		{
			this->unityVersion = "Unsupported v5";
		}
		else
		{
			this->unityVersion = "Unsupported Unknown";
		}
		this->fieldCount = 0;
		return filePos;
	}
	
	callback(filePos, 25, this->unityVersion, readerPar);
	this->unityVersion[24] = 0;
	unsigned long newFilePos = filePos + strlen((const char *)this->unityVersion) + 1;
	char first = this->unityVersion[0];
	if ( first < '0' || first > '9' )
	{
		this->fieldCount = 0;
		return newFilePos;
	}
	
	callback(newFilePos, 4, &this->version, readerPar);
	newFilePos += 4;
	if ( endianness )
	{
		SWAP_ENDIAN(this->version)
	}
	if ( version >= 0xD )
	{
		callback(newFilePos, 1, &this->hasTypeTree, readerPar);
		newFilePos += 1;
	}
	callback(newFilePos, 4, this->fieldCount, readerPar);
	newFilePos += 4;
	if ( endianness )
	{
		SWAP_ENDIAN(this->fieldCount);
	}
	if ( this->fieldCount > 0 )
	{
		if ( version >= 0xD )
		{
			this->pTypes_Unity5 = malloc(sizeof(Type_0D) * this->fieldCount);
			if ( this->pTypes_Unity5 )
			{
				for(int i = 0; i < this->fieldCount; i++)
				{
					newFilePos = this->pTypes_Unity5[i]::Read(
						this->hasTypeTree,
						newFilePos,
						callback,
						readerPar,
						version,
						this->version,
						endianness);
				}
			}
			else
			{
				for(int i = 0; i < this->fieldCount; i++)
				{
					memset(&v29, 0, 0x38u);
					Type_0D temp();
					newFilePos = temp::Read(
						this->hasTypeTree,
						newFilePos,
						callback,
						readerPar,
						version,
						this->version,
						endianness);
				}
				this->fieldCount = 0;
			}
		}
		else
		{
			this->pTypes_Unity4 = malloc(sizeof(Type_07) * this->fieldCount);
			if ( this->pTypes_Unity5 )
			{
				for(int i = 0; i < this->fieldCount; i++)
				{
					newFilePos = this->pTypes_Unity4[i]::Read(
						this->hasTypeTree,
						newFilePos,
						callback,
						readerPar,
						version,
						this->version,
						endianness);
				}
			}
			else
			{
				for(int i = 0; i < this->fieldCount; i++)
				{
					memset(&v29, 0, 0x38u);
					Type_07 temp();
					newFilePos = temp::Read(
						this->hasTypeTree,
						newFilePos,
						callback,
						readerPar,
						version,
						this->version,
						endianness);
				}
				this->fieldCount = 0;
			}
		}
	}
	else
	{
		this->pTypes_Unity4 = NULL;
		this->pTypes_Unity5 = NULL;
	}
	if ( version < 0xE )
	{
		callback(newFilePos, 4, this->dwUnknown, readerPar);
		newFilePos += 4;
		if ( endianness )
			SWAP_ENDIAN(this->dwUnknown);
	}
	return newFilePos;
	
}