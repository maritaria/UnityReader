SWAP_ENDIAN( trigger , &addr );

unsigned int __thiscall Type_0D::Read(
	Type_0D *this,
	bool hasTypeTree,
	unsigned __int64 filePos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int32 version,
	unsigned __int32 typeVersion,
	bool endianness)
{
	unsigned int newFilePos; // esi@2
	char *storage; // eax@20
	unsigned int result; // eax@21
	void *fieldBuffer; // eax@22
	int storageSize; // ecx@23
	unsigned __int64 fieldBufferIndex; // rax@sizeof(TypeField_0D)
	char *finalStringTable; // eax@28
	size_t this->stringTableLen; // [sp+10h] [bp-248h]@3
	unsigned int fieldCount; // [sp+1Ch] [bp-23Ch]@15
	unsigned int fieldStorageSize; // [sp+2Ch] [bp-22Ch]@20
	
	/* Alignment is per 2 bytes */
	
	if ( version < 0xD )
	{
		//Safety
		char v48[0x220u];
		memset((void *)this, 0, sizeof(Type_0D));
		memset(v48, 0, sizeof(v48));
		result = Type_07::Read((Type_07 *)&v48, hasTypeTree, filePos, callback, readerPar, version, typeVersion, endianness);
		return result;
	}
	
	callback(filePos, 4, &this->classId, readerPar);
	newFilePos = filePos + 4;
	SWAP_ENDIAN( endianness, &this->classId );
	if ( version < 0x10 )
	{
		this->unknown16_1 = 0;
	}
	else
	{
		callback(newFilePos, 1, &this->unknown16_1, readerPar);
		newFilePos += 1;
	}
	if ( version < 0x11 )
	{
		this->scriptIndex = -1;
	}
	else
	{
		callback(newFilePos, 2, &this->scriptIndex, readerPar);
		newFilePos += 2;
		SWAP_ENDIAN( endianness, &this->scriptIndex );
	}
	if ( this->classId < 0 || this->classId == 114 )
	{
		callback(newFilePos, 16, &this->unknown1, readerPar);
		newFilePos += 16;
	}
	callback(newFilePos, 16, &this->unknown5, readerPar);
	newFilePos += 16;
	this->typeFieldsExCount = 0;
	this->pTypeFieldsEx = 0;
	this->stringTableLen = 0;
	this->pStringTable = 0;
	if ( !hasTypeTree )
	{
		return newFilePos;
	}
	callback(newFilePos, 4, &fieldCount, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, &fieldCount);
	callback(newFilePos, 4, &this->stringTableLen, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, &this->stringTableLen);
	
	fieldStorageSize = sizeof(TypeField_0D) * fieldCount;
	storageSize = fieldStorageSize + this->stringTableLen;
	char *storage = malloc(storageSize + 1);
	if ( !storage )
	{
		return newFilePos + storageSize;
	}
	callback(newFilePos, storageSize, storage, readerPar);
	newFilePos += storageSize;
	
	struct buff_t {
		int unknown,
		int size,
		char *buffer,
	}
	
	buff_t fieldBuffer = malloc(12);
	if ( fieldBuffer )
	{
		fieldBuffer->unknown = 0;
		fieldBuffer->size = storageSize;
		fieldBuffer->buffer = storage;
		this->pTypeFieldsEx  = (TypeField_0D *)malloc(sizeof(TypeField_0D) * fieldCount);
		if ( this->pTypeFieldsEx  )
		{
			fieldBufferIndex = 0;
			for (int i = 0; i < fieldCount; i++)
			{
				fieldBufferIndex = this->pTypeFieldsEx[i]::Read(
					fieldBufferIndex,
					AssetsReaderFromMemory,
					fieldBuffer,
					endianness);
			}
			this->typeFieldsExCount = fieldCount;
			LOBYTE(storageSize) = storage[storageSize - 1] != 0;
			finalStringTable = malloc(this->stringTableLen + ((_BYTE)storageSize != 0));
			if ( finalStringTable )
			{
				memcpy(finalStringTable, (char *)storage + fieldStorageSize, this->stringTableLen);
				if ( (_BYTE)storageSize )
					*(_BYTE *)(finalStringTable + this->stringTableLen) = 0;
				this->pStringTable = finalStringTable;
				this->stringTableLen = this->stringTableLen;
				this->pStringTable = this->pStringTable;
			}
		}
		free(fieldBuffer);
	}
	free(storage);
	
	return newFilePos;
}