SWAP_ENDIAN( endianness, &item )

int __thiscall TypeField_0D::Read(
	TypeField_0D *this,
	unsigned __int64 filePos,
	unsigned __int64 callback(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	bool endianness)
{
	callback(filePos, 2, &this->version, readerPar);
	unsigned long newFilePos = filePos + 2;
	SWAP_ENDIAN( endianness, &this->version );
	callback(newFilePos, 1, &this->depth, readerPar);
	newFilePos += 1;
	callback(newFilePos, 1, &this->isArray, readerPar);
	newFilePos += 1;
	callback(newFilePos, 4, &this->typeStringOffset, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, ? );
	
	callback(newFilePos, 4, &this->nameStringOffset, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, ? );
	
	callback(newFilePos, 4, &this->size, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, ? );
	
	callback(newFilePos, 4, &this->index, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, ? );
	
	callback(newFilePos, 4, &this->flags, readerPar);
	newFilePos += 4;
	SWAP_ENDIAN( endianness, ? );
	
	return newFilePos;
}