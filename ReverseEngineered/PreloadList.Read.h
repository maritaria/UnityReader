FLIP_ENDIAN(item)

int __thiscall PreloadList::Read(
	PreloadList *this,
	unsigned __int64 filePos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int32 version,
	bool endianness)
{
	callback(filePos, 4, &(this->len), readerPar);
	unsigned long newFilePos = filePos + 4;
	if ( endianness )
		FLIP_ENDIAN(this->len);
	if ( this->len > 0 )
		this->items = malloc(sizeof(AssetPPtr) * this->len);
	if ( this->items )
	{
		for (unsigned int i = 0; i < this->len;i++) 
		{
			callback(newFilePos, 4, &(this->items[i].fileID), readerPar);
			newFilePos += 4;
			if ( endianness )
			{
				FLIP_ENDIAN(this->items[i].fileID);
			}
			if ( version < 0xE )
			{
				callback(newFilePos, 4, &(this->items[i].pathID), readerPar);
				newFilePos += 4;
				if ( endianness )
				{
					SWAP_ENDIAN(this->items[i].pathID);
				}
				//Note possibly truncate pathID to 32bit (?)
			}
			else
			{
				//Alignment
				newFilePos = (newFilePos + 3) & 0xFFFFFFFC;
				callback(newFilePos, 8, &(this->items[i].pathID), readerPar);
				newFilePos += 8;
				if ( endianness )
				{
					SWAP_ENDIAN(this->items[i].pathID);
				}
			}
		}
		return newFilePos;
	}
	else
	{
		if ( version < 0xE )
		{
			newFilePos += 8 * this->len;
		}
		else if ( this->len > 0 )
		{
			this->len = 0;
			return 12 * this->len + ((filePos + 7) & 0xFFFFFFFC);
		}
		this->len = 0;
		return newFilePos;
	}
}