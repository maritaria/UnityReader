signed __int64 __thiscall ClassDatabaseTypeField::Read(
	ClassDatabaseTypeField *this,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int64 filePos,
	int version)
{

	this->typeName->fromStringTable = 1;
	callback(filePos, 4, this->typeName->str, readerPar);
	this->fieldName->fromStringTable = 1;
	callback(filePos + 4, 4, this->fieldName->str, readerPar);
	callback(filePos + 8, 1, this->depth, readerPar);
	callback(filePos + 9, 1, this->isArray, readerPar);
	callback(filePos + 10, 4, this->size, readerPar);
	unsigned int newFilePos = filePos + 14;
	this->version = 1;
	if ( version >= 1 )
	{
		if ( version >= 3 )
		{
			callback(newFilePos, 2, this->version, readerPar);
			newFilePos += 2;
		}
	}
	else
	{
		int unused;
		callback(newFilePos, 4, &unused, readerPar);
		newFilePos += 4;
		if ( unused & 0x80000000 )
		{
			callback(newFilePos, 2, this->version, readerPar);
			newFilePos += 2;
		}
	}
	callback(newFilePos, 4, this->flags2, readerPar);
	return newFilePos + 4;
}