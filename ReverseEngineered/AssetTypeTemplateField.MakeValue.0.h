int __cdecl sub_10013BF0(
	AssetTypeTemplateField *this,
	void (__cdecl *callback)(unsigned int, unsigned int, signed int, _DWORD, unsigned int *, int),
	int readerPar,
	int *filePos,
	int *a5,
	int *a_numPointers,
	int *a_offset,
	bool bigEndian)
{
	int arraySize;
	bool v13; // zf@1
	int childType; // edx@3
	int readerPar; // ebx@6
	unsigned int i; // edi@6
	int v17; // eax@11
	int this->valueType; // edi@13
	int stringSize; // edx@14
	__int64 *filePos + 4; // kr08_8@14
	int this->childrenCount; // edx@21
	unsigned int i; // edi@28
	int delete_me; // ebx@29
	int v24; // eax@30
	int delete_me25; // edx@34
	int result; // eax@34
	unsigned int v28; // [sp+1Ch] [bp-14h]@7
	int result; // [sp+20h] [bp-10h]@1

	result = 1;
	if ( this->isArray && this->childrenCount == 2 )
	{
		//Array type: 2 children, 1st is size, 2nd is data
		*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
		childType = this->children[0]->valueType;
		if ( childType != ValueType_Int32 && childType != ValueType_UInt32 )
		{
			MessageBoxW(0, L"Invalid array value type!", L"ERROR", 0x10u);
			delete_me = HIDWORD(*filePos);
			*filePos = *filePos;
			goto LABEL_RETURN;
		}
		callback(*filePos, 4, &arraySize, readerPar);
		*filePos += 4;
		SWAP_ENDIAN( bigEndian, &arraySize );
		if ( !stricmp(this->type, "TypelessData") )//if equal
		{
			*a_offset += arraySize;
			*filePos += arraySize;
			goto LABEL_ALIGN_RETURN;
		}
		*a_numPointers += 4 * arraySize;
		for(int i = 0; i < arraySize; i++);
		{
			result += this->children[1]->sub_10013BF0(callback, readerPar, &*filePos, &*a5, &*a_numPointers, &*a_offset, bigEndian);
		}
		goto LABEL_ALIGN_RETURN;
	}
	if ( this->valueType == ValueType_String )
	{
		callback(*filePos, 4, &stringSize, readerPar);
		SWAP_ENDIAN( bigEngian, &stringSize );
		*filePos += stringSize + 4;
		*a5 += stringSize + 17;
		goto LABEL_ALIGN_RETURN;
	}
	else
	{
		if ( this->childrenCount == 0 )
		{
			switch ( this->valueType )
			{
				case ValueType_Bool:
				case ValueType_Int8:
				case ValueType_UInt8:
					*filePos += 1;
					*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
					break;
				case ValueType_Int16:
				case ValueType_UInt16:
					*filePos += 2;
					*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
					break;
				case ValueType_Int64:
				case ValueType_Int64:
				case ValueType_Float:
					*filePos += 4;
					*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
					break;
				case ValueType_Int64:
				case ValueType_UInt64:
				case ValueType_Double:
					*filePos += 8;
					*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
					break;
				default:
					*a5 += sizeof(AssetTypeValueField | AssetTypeValue);
					break;
			}
			goto LABEL_ALIGN_RETURN;
		}
		*a_numPointers += (this->childrenCount * 4);
		for(int i = 0; i < this->childrenCount; i++ );
		{
			result += this->children[i]->sub_10013BF0(callback, readerPar, &*filePos, &*a5, &*a_numPointers, &*a_offset, bigEndian);
		}
		goto LABEL_ALIGN_RETURN;
	}
	
LABEL_ALIGN_RETURN:
	if ( this->align || (this->childrenCount && this->children[0]->align) )
	{
		//Rounding file pos for 4 byte alignment
		*filePos = (*filePos + 3) & 0xFFFFFFFC;
	}
	goto LABEL_RETURN;
	
LABEL_RETURN:
	return result;
}