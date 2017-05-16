char __thiscall AssetTypeTemplateField::FromClassDatabase(
	AssetTypeTemplateField *this,
	struct ClassDatabaseFile *pFile,
	struct ClassDatabaseType *pType,
	unsigned __int32 fieldIndex)
{
	struct ClassDatabaseType *pType; // esi@1
	AssetTypeTemplateField *this; // edi@1
	char result; // al@2
	unsigned __int32 field; // ebx@3
	int v8; // eax@4
	unsigned int v9; // edx@5
	char *v10; // eax@9
	unsigned __int32 v11; // eax@16
	int v12; // ecx@17
	unsigned __int8 v13; // cl@18
	bool v14; // cl@26
	int v15; // ecx@28
	void *v16; // eax@29
	unsigned __int32 v17; // ebx@33
	int v18; // eax@34
	unsigned __int8 v19; // cl@35
	unsigned __int32 field; // [sp+Ch] [bp-Ch]@3
	int v21; // [sp+10h] [bp-8h]@34
	unsigned __int32 v22; // [sp+14h] [bp-4h]@16
	int v23; // [sp+14h] [bp-4h]@34
	unsigned __int8 v24; // [sp+27h] [bp+Fh]@16
	unsigned __int32 v25; // [sp+28h] [bp+10h]@17
	char v26; // [sp+2Bh] [bp+13h]@33

	if ( (*((_DWORD *)pType + 5) - pType->fields) >> 5 <= fieldIndex )
	{
		result = 0;
		this->name = NULL;
		this->type = NULL;
		this->valueType = 0;
		this->isArray = false;
		this->align = false;
		this->hasValue = false;
		this->childrenCount = 0;
		this->children = NULL;
		return result;
	}
	ClassDatabaseTypeField *field = pType->fields[fieldIndex];
	this->isArray = (field->isArray != 0);
	if ( field->fieldName->fromStringTable )
	{
		/* Lookup name in string table */
		v9 = *(_DWORD *)(field + 8);
		if ( v9 < *((_DWORD *)pFile + 7) )
			v8 = v9 + *((_DWORD *)pFile + 13);
		else
			v8 = 0;
	}
	else
	{
		v8 = *(_DWORD *)(field + 8);
	}
	this->name = v8;
	if ( *(_BYTE *)(field + 4) )
	{
		/* Lookup name in string table */
		if ( *(_DWORD *)field < *((_DWORD *)pFile + 7) )
			v10 = (char *)(*(_DWORD *)field + *((_DWORD *)pFile + 13));
		else
			v10 = 0;
	}
	else
	{
		v10 = *(char **)field;
	}
	this->type = v10;
	if ( v10 )
		this->valueType = GetValueTypeByTypeName(v10);
	else
		this->valueType = 0;
	*((_BYTE *)this + 13) = (field->flags2 & 0x4000) != 0;
	this->childrenCount = 0;
	v11 = fieldIndex + 1;
	v24 = 0;
	v22 = fieldIndex + 1;
	if ( fieldIndex + 1 < (*((_DWORD *)pType + 5) - pType->fields) >> 5 )
	{
		v12 = 32 * v11;
		v25 = 32 * v11;
		while ( 1 )
		{
			v13 = *(_BYTE *)(v12 + pType->fields + 16);
			if ( v13 <= *(_BYTE *)(field + 16) )
			{
LABEL_24:
				v11 = v22;
				goto LABEL_25;
			}
			if ( !v24 )
				break;
			if ( v13 == v24 )
				goto LABEL_22;
LABEL_23:
			++v11;
			v12 = v25 + 32;
			v25 += 32;
			if ( v11 >= (*((_DWORD *)pType + 5) - pType->fields) >> 5 )
				goto LABEL_24;
		}
		v24 = v13;
LABEL_22:
		++this->childrenCount;
		goto LABEL_23;
	}
LABEL_25:
	if ( (*((_DWORD *)pType + 5) - pType->fields) >> 5 <= v11 )
		v14 = 1;
	else
		v14 = this->childrenCount == 0;
	*((_BYTE *)this + 14) = v14;
	v15 = this->childrenCount;
	if ( v15 )
	{
		v16 = malloc(24 * v15);
		this->children = v16;
		if ( !v16 )
		{
			this->childrenCount = 0;
			return 0;
		}
		v11 = v22;
	}
	else
	{
		this->children = 0;
	}
	v17 = v11;
	v26 = 1;
	if ( v11 < (*((_DWORD *)pType + 5) - pType->fields) >> 5 )
	{
		v18 = 32 * v11;
		v23 = 0;
		v21 = 32 * v17;
		do
		{
			v19 = *(_BYTE *)(v18 + pType->fields + 16);
			if ( v19 <= *(_BYTE *)(field + 16) )
				break;
			if ( v19 == v24 )
			{
				if ( !AssetTypeTemplateField::FromClassDatabase(
								(AssetTypeTemplateField *)(v23 + this->children),
								pFile,
								pType,
								v17) )
					v26 = 0;
				v23 += 24;
				v18 = v21;
			}
			++v17;
			v18 += 32;
			v21 = v18;
		}
		while ( v17 < (*((_DWORD *)pType + 5) - pType->fields) >> 5 );
	}
	return v26;
}