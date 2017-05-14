int __thiscall AssetsFileTable::AssetsFileTable(AssetsFileTable* this, int pFile, bool readNames)
{
	this->pFile = pFile;
	this->reader = pFile->reader;
	this->readerPar = pFile->readerPar;
	
	int numItems;
	this->reader(pFile->AssetTablePos, 4, &numItems, pFile->readerPar);
	unsigned long newFilePos = pFile->AssetTablePos + 4;
	SWAP_ENDIAN(pFile->header->endian, numItems);
	
	if ( pFile->header->format >= 14 )
	{
		//Rounding
		newFilePos = (newFilePos + 3) & 0xFFFFFFFFFFFFFFFCui64;
	}
	this->assetFileInfoCount = numItems;
	this->AssetFileInfoEx = unknown_libname_4(152 * (_DWORD)numItems | -(152 * (unsigned __int64)(unsigned int)numItems >> 32 != 0));
	
	for (int i = 0; i < numItems; i++)
	{
		AssetFileInfoEx info = this->AssetFileInfoEx[i];
		newFilePos = info.Read(
			pFile->header->format,
			newFilePos,
			this->reader,
			this->readerPar,
			pFile->header->endian);
		
		if ( pFile->header->format < 16 )
		{
			info->curFileType = info->curFileTypeOrIndex;
		}
		else
		{
			TypeTree tree = pFile->typeTree;
			Type_0D* types = tree->pTypes_Unity5;
			if ( info->curFileTypeOrIndex < tree->fieldCount )//Bounds check
			{
				Type_0D type = types[info->curFileTypeOrIndex];
				if ( type->scriptIndex == -1 )
				{
					info->curFileType = type->classId;
					info->inheritedUnityClass = type->classId;
					info->scriptIndex = -1;
				}
				else
				{
					info->curFileType = -1 - type->scriptIndex;
					info->inheritedUnityClass = type->classId;//Should be 114
					info->scriptIndex = type->scriptIndex;
				}
			}
			else
			{
				//Out of bounds
				info->curFileType = 2147483648; //Uint16.maxvalue - 2
				info->inheritedUnityClass = -1;
			}
		}
		info->absolutePos = pFile->header->offs_firstFile + info->offs_curFile;
		info->name = NULL;
	}
	
	if ( readNames )
	{
		struct NameBuffer {
			int64_t length,
			char[116] text;
		}
		
		void *nameBuffer = malloc(120);
		for (int i = 0; i < numItems;i++)
		{
			AssetFileInfoEx info = this->AssetFileInfoEx[i];
			switch ( info->curFileType )
			{
				case 0x15:
				case 0x1B:
				case 0x1C:
				case 0x2B:
				case 0x30:
				case 0x31:
				case 0x3E:
				case 0x48:
				case 0x4A:
				case 0x53:
				case 0x54:
				case 0x59:
				case 0x5A:
				case 0x5B:
				case 0x5D:
				case 0x6D:
				case 0x73:
				case 0x75:
				case 0x79:
				case 0x80:
				case 0x86:
				case 0x8E:
				case 0x96:
				case 0x98:
				case 0x9C:
				case 0x9E:
				case 0xAB:
				case 0xB8:
				case 0xB9:
				case 0xBA:
				case 0xC2:
				case 0xC8:
				case 0xCF:
				case 0xD5:
				case 0xDD:
				case 0xE2:
				case 0xE4:
				case 0xED:
				case 0xEE:
				case 0xF0:
				case 0x102:
				case 0x10F:
				case 0x110:
				case 0x111:
				case 0x112:
					break;
				default:
					continue;
			}
			
			this->reader(info->absolutePos, 120, nameBuffer, this->readerPar);
			
			if ( nameBuffer->length > 0 )
			{
				for (int j = 0; j < nameBuffer->length; j++)
				{
					char current = nameBuffer->text[j];
					if (current < 32) break;
				}
			}
			if ( (unsigned int)(nameBuffer->length + 1) <= 100 )
			{
				memcpy(info->name, nameBuffer->text, nameBuffer->length);
				info->name[nameBuffer->length] = 0;
			}
			else
			{
				//LOL
				memcpy(info->name, nameBuffer->text, 96);
				info->name[96] = '.';
				info->name[97] = '.';
				info->name[98] = '.';
				info->name[99] = 0x00;
			}
		}
		free(nameBuffer);
	}
	return this;
}