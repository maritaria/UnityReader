int __thiscall ClassDatabaseFile::Read(
	ClassDatabaseFile *this,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	void *readerPar,
	unsigned __int64 filePos)
{
	ClassDatabaseFile *this; // ebx@1
	int newFilePos; // esi@1
	signed __int64 version; // rax@1
	int delete_me; // edi@1
	unsigned __int8 version; // al@2
	_BYTE *v9; // eax@4
	int v10; // eax@5
	int v11; // edx@5
	size_t v12; // eax@9
	void *buffer; // eax@9
	char version; // cl@11
	char *v15; // eax@12
	unsigned int v16; // edx@15
	_DWORD *memoryArg; // eax@20
	unsigned int delete_me; // edi@26
	int v19; // ebx@26
	unsigned int v20; // eax@27
	int v21; // edx@28
	int v22; // ecx@29
	char *v23; // esi@29
	unsigned int v24; // eax@30
	unsigned int v25; // eax@31
	unsigned int v26; // ecx@31
	unsigned int v27; // ecx@33
	int v28; // ecx@38
	unsigned int v29; // esi@38
	int v30; // ecx@41
	unsigned int v31; // eax@42
	unsigned int v32; // eax@43
	unsigned int v33; // ecx@43
	unsigned int v34; // ecx@45
	int v35; // ecx@50
	void *v36; // eax@56
	int v37; // esi@61
	int v38; // edx@61
	void *buffer; // [sp+20h] [bp-64h]@1
	ClassDatabaseFile *this; // [sp+24h] [bp-60h]@1
	_BYTE *v42; // [sp+28h] [bp-5Ch]@4
	int v43; // [sp+2Ch] [bp-58h]@5
	char *v44; // [sp+30h] [bp-54h]@11
	unsigned int i; // [sp+34h] [bp-50h]@15
	int v46; // [sp+38h] [bp-4Ch]@5
	unsigned int classCount; // [sp+40h] [bp-44h]@26
	int v48; // [sp+44h] [bp-40h]@9
	int v49; // [sp+48h] [bp-3Ch]@5
	int newFilePos; // [sp+4Ch] [bp-38h]@2
	int v51; // [sp+50h] [bp-34h]@2
	int v52_classId; // [sp+54h] [bp-30h]@27
	int v52_baseClass; // [sp+58h] [bp-2Ch]@27
	void *v52_fields; // [sp+64h] [bp-20h]@27
	int v55; // [sp+68h] [bp-1Ch]@27
	int v56; // [sp+6Ch] [bp-18h]@27
	int v57; // [sp+80h] [bp-4h]@27
	unsigned __int64 bufferPos; // [sp+94h] [bp+10h]@2

	this->valid = 0;
	newFilePos = this->header::Read(callback, readerPar, filePos);
	if ( newFilePos == filePos )
	{
		return newFilePos;
	}
	version = this->header->fileVersion;
	bufferPos = __PAIR__(HIDWORD(version), newFilePos);
	v51 = HIDWORD(version);
	if ( version < 3u )
	{
		v9 = malloc(this->header->compressedSize);
		v42 = v9;
		if ( !v9 )
		{
LABEL_8:
			LODWORD(version) = newFilePos;
			return version;
		}
		v43 = this->header->compressedSize;
		v49 = 0;
		v10 = callback(newFilePos, v43, v9, readerPar);
		v46 = v11;
		if ( v10 != v43 || v46 != v49 )
		{
			free(v42);
			goto LABEL_8;
		}
		newFilePos = newFilePos + v43;
		v12 = this->header->uncompressedSize + 1;
		v51 = delete_me + __CFADD__(newFilePos, v43) + v49;
		v48 = v12;
		buffer = malloc(v12);
		if ( !buffer )
		{
			free(v42);
			LODWORD(version) = newFilePos;
			return version;
		}
		v44 = 0;
		if ( version == 1 )
		{
			v15 = sub_100225A0(v48, v42, buffer, v43);
		}
		else
		{
			if ( version != 2 )
				goto LABEL_18;
			if ( (unsigned int)v43 <= 5 )
				goto LABEL_18;
			v16 = this->header->uncompressedSize;
			v48 = v43 - 5;
			i = v16;
			if ( sub_100248F0(&v43, v42 + 5, (int *)&i, (int)buffer, (unsigned int *)&v48, (int)v42) )
				goto LABEL_18;
			v15 = (char *)i;
		}
		v44 = v15;
LABEL_18:
		free(v42);
		i = this->header->uncompressedSize;
		if ( v44 != (char *)i )
		{
			free(buffer);
			LODWORD(version) = newFilePos;
			return version;
		}
		memoryArg = malloc(0xCu);
		if ( memoryArg )
		{
			memoryArg[1] = i;
			*memoryArg = 0;
			memoryArg[2] = buffer;
			readerPar = memoryArg;
		}
		else
		{
			readerPar = 0;
		}
		callback = AssetsReaderFromMemory;
		if ( !readerPar )
		{
			free(buffer);
			LODWORD(version) = newFilePos;
			return version;
		}
		bufferPos = 0;
	}
	/* Read decompressed area */
	callback(bufferPos, 4, &classCount, readerPar);
	bufferPos = bufferPos + 4;
	sub_10019D80((void *)this->classes, classCount);//std::vector ctor?
	i = 0;
	
	while ( i < classCount );
	{
		v57 = 0;
		v52_baseClass = -1;
		v52_classId = -1;
		v52_fields = 0;
		v55 = 0;
		v56 = 0;
		LOBYTE(v57) = 0;
		sub_10019C00(1u, (int)&v52_fields);//v57-v52_fields stack allocated object
		v57 = 2;
		bufferPos = ClassDatabaseType::Read(
						(ClassDatabaseType *)&v52_classId,
						callback,
						(__int32)readerPar,
						__PAIR__(delete_me, (unsigned int)bufferPos),
						*((_BYTE *)this + 8));
		v20 = *(_DWORD *)(this->classes + 4);
		if ( (unsigned int)&v52_classId >= v20 || (v21 = *(_DWORD *)this->classes, *(_DWORD *)this->classes > (unsigned int)&v52_classId) )
		{
			v30 = *(_DWORD *)(this->classes + 8);
			if ( v20 == v30 )
			{
				v31 = (signed int)(v20 - *(_DWORD *)this->classes) >> 5;
				if ( v31 > 0x7FFFFFE )
				{
LABEL_60:
					std::_Xlength_error("vector<T> too long");
					goto LABEL_61;
				}
				v32 = v31 + 1;
				v33 = (v30 - *(_DWORD *)this->classes) >> 5;
				if ( v32 > v33 )
				{
					if ( 0x7FFFFFF - (v33 >> 1) >= v33 )
						v34 = (v33 >> 1) + v33;
					else
						v34 = 0;
					if ( v34 < v32 )
						v34 = v32;
					sub_10019D80((void *)this->classes, v34);
				}
			}
			v35 = *(_DWORD *)(this->classes + 4);
			LOBYTE(v57) = 4;
			if ( v35 )
				((void (__stdcall *)(int *))ClassDatabaseType::ClassDatabaseType)(&v52_classId);
			*(_DWORD *)(this->classes + 4) += 32;
		}
		else
		{
			v22 = *(_DWORD *)(this->classes + 8);
			v23 = (char *)&v52_classId - v21;
			if ( v20 == v22 )
			{
				v24 = (signed int)(v20 - v21) >> 5;
				if ( v24 > 0x7FFFFFE )
					goto LABEL_60;
				v25 = v24 + 1;
				v26 = (v22 - v21) >> 5;
				if ( v25 > v26 )
				{
					if ( 0x7FFFFFF - (v26 >> 1) >= v26 )
						v27 = (v26 >> 1) + v26;
					else
						v27 = 0;
					if ( v27 < v25 )
						v27 = v25;
					sub_10019D80((void *)this->classes, v27);
				}
			}
			v28 = *(_DWORD *)(this->classes + 4);
			v29 = *(_DWORD *)this->classes + ((unsigned int)v23 & 0xFFFFFFE0);
			LOBYTE(v57) = 3;
			if ( v28 )
				((void (__stdcall *)(unsigned int))ClassDatabaseType::ClassDatabaseType)(v29);
			*(_DWORD *)(this->classes + 4) += 32;
		}
		v57 = -1;
		if ( v52_fields )
			operator delete(v52_fields);
		delete_me = HIDWORD(bufferPos);
		v52_fields = 0;
		v55 = 0;
		v56 = 0;
		++i;
	}
	
	this = (int)this;
	v36 = malloc(this->header->stringTableLen + 1);
	*((_DWORD *)this + 13) = v36;
	if ( !v36 )
	{
		if ( buffer )
		{
			free(readerPar);
			free(buffer);
		}
LABEL_59:
		LODWORD(version) = bufferPos;
		return version;
	}
LABEL_61:
	v37 = *(_DWORD *)(this + 28);
	if ( callback(this->header->stringTablePos, v37, v36, readerPar) != v37
		|| v38 )
	{
		if ( buffer )
		{
			free(readerPar);
			free(buffer);
			LODWORD(version) = bufferPos;
			return version;
		}
		goto LABEL_59;
	}
	*(_BYTE *)(v37 + *((_DWORD *)this + 13)) = 0;
	if ( buffer )
	{
		free(readerPar);
		free(buffer);
		LODWORD(version) = newFilePos;
	}
	else
	{
		LODWORD(version) = bufferPos;
	}
	*(_BYTE *)this = 1;
	return version;
}