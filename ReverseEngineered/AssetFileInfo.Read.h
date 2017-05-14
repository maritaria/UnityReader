unsigned __int64 __thiscall AssetFileInfo::Read(AssetFileInfo *this, unsigned __int32 a2, unsigned __int64 a3, unsigned __int64 (__cdecl *a4)(unsigned __int64, unsigned __int64, void *, __int32), __int32 a5, bool a6)
{
	unsigned __int32 v6; // ebx@1
	unsigned int v7; // edi@1
	AssetFileInfo *v8; // esi@1
	int v9; // ecx@1
	signed int v10; // eax@5
	signed int v11; // eax@12
	size_t v12; // edi@17
	int v13; // edx@19
	int v14; // ST2C_4@19
	int v15; // ST28_4@19
	int v16; // edx@19
	int v17; // edx@22
	unsigned int v18; // edx@24
	unsigned int v19; // ecx@26
	signed int v20; // edi@26
	__int16 v21; // cx@29
	__int16 v22; // ST28_2@30
	__int16 v23; // cx@33
	__int16 v24; // ST28_2@34
	unsigned __int64 result; // rax@37
	int v26; // [sp+20h] [bp-20h]@10
	int v27; // [sp+24h] [bp-1Ch]@24
	int v28; // [sp+28h] [bp-18h]@26
	unsigned __int64 v29; // [sp+4Ch] [bp+Ch]@17

	v6 = a2;
	v7 = a3;
	v8 = this;
	v9 = HIDWORD(a3);
	if ( a2 >= 0xE )
	{
		v9 = (a3 + 3) >> 32;
		HIDWORD(a3) = (a3 + 3) >> 32;
		v7 = (a3 + 3) & 0xFFFFFFFC;
	}
	if ( a2 >= 0x11 )
		goto LABEL_9;
	if ( a2 < 0x10 )
	{
		if ( a2 >= 0xF )
		{
			v10 = 25;
			goto LABEL_10;
		}
		v10 = 24;
		if ( a2 == 14 )
			goto LABEL_10;
LABEL_9:
		v10 = 20;
		goto LABEL_10;
	}
	v10 = 23;
LABEL_10:
	((void (__cdecl *)(unsigned int, int, signed int, _DWORD, int *, __int32))a4)(v7, v9, v10, 0, &v26, a5);
	if ( a2 >= 0x11 )
		goto LABEL_42;
	if ( a2 >= 0x10 )
	{
		v11 = 23;
		goto LABEL_17;
	}
	if ( a2 >= 0xF )
	{
		v11 = 25;
		goto LABEL_17;
	}
	v11 = 24;
	if ( a2 != 14 )
LABEL_42:
		v11 = 20;
LABEL_17:
	v29 = __PAIR__(HIDWORD(a3), v11) + v7;
	*(_DWORD *)v8 = 0;
	*((_DWORD *)v8 + 1) = 0;
	v12 = a2 < 0xE ? 4 : 8;
	memcpy((void *)v8, &v26, v12);
	if ( a6 )
	{
		if ( a2 < 0xE )
		{
			v16 = *((_BYTE *)v8 + 3)
					+ ((*(_DWORD *)v8 >> 8) & 0xFF00)
					+ (((*(_DWORD *)v8 & 0xFF00) + (*(_DWORD *)v8 << 16)) << 8);
		}
		else
		{
			v13 = *((_DWORD *)v8 + 1);
			BYTE2(v14) = (unsigned __int16)*(_DWORD *)v8 >> 8;
			BYTE3(v14) = *(_DWORD *)v8;
			BYTE1(v14) = *(_DWORD *)v8 >> 16;
			LOBYTE(v14) = *(_DWORD *)v8 >> 24;
			BYTE3(v15) = *((_DWORD *)v8 + 1);
			LOBYTE(v15) = *((_DWORD *)v8 + 1) >> 24;
			BYTE2(v15) = BYTE1(v13);
			BYTE1(v15) = *((_DWORD *)v8 + 1) >> 16;
			v16 = v15;
			*((_DWORD *)v8 + 1) = v14;
		}
		*(_DWORD *)v8 = v16;
	}
	v17 = *(int *)((char *)&v26 + v12);
	*((_DWORD *)v8 + 2) = v17;
	if ( a6 )
		*((_DWORD *)v8 + 2) = *((_BYTE *)v8 + 11)
												+ ((*((_DWORD *)v8 + 2) >> 8) & 0xFF00)
												+ (((v17 & 0xFF00) + (v17 << 16)) << 8);
	v18 = *(int *)((char *)&v27 + v12);
	*((_DWORD *)v8 + 3) = v18;
	if ( a6 )
	{
		v6 = a2;
		*((_DWORD *)v8 + 3) = ((v18 >> 8) & 0xFF00) + *((_BYTE *)v8 + 15) + (((v18 & 0xFF00) + (v18 << 16)) << 8);
	}
	v19 = *(int *)((char *)&v28 + v12);
	v20 = a2 < 0xE ? 16 : 20;
	*((_DWORD *)v8 + 4) = v19;
	if ( a6 )
	{
		v6 = a2;
		*((_DWORD *)v8 + 4) = ((v19 >> 8) & 0xFF00) + *((_BYTE *)v8 + 19) + (((v19 & 0xFF00) + (v19 << 16)) << 8);
	}
	if ( v6 >= 0x10 )
	{
		*((_WORD *)v8 + 10) = 0;
	}
	else
	{
		v21 = *(_WORD *)((char *)&v26 + v20);
		v20 = a2 < 0xE ? 18 : 22;
		*((_WORD *)v8 + 10) = v21;
		if ( a6 )
		{
			LOBYTE(v22) = HIBYTE(v21);
			HIBYTE(v22) = v21;
			*((_WORD *)v8 + 10) = v22;
		}
	}
	if ( v6 - 11 > 5 )
	{
		*((_WORD *)v8 + 11) = -1;
		if ( v6 < 0xB )
			v20 += 2;
	}
	else
	{
		v23 = *(_WORD *)((char *)&v26 + v20);
		v20 += 2;
		*((_WORD *)v8 + 11) = v23;
		if ( a6 )
		{
			LOBYTE(v24) = HIBYTE(v23);
			HIBYTE(v24) = v23;
			*((_WORD *)v8 + 11) = v24;
		}
	}
	result = v29;
	if ( v6 - 15 > 1 )
		*((_BYTE *)v8 + 24) = 0;
	else
		*((_BYTE *)v8 + 24) = *((_BYTE *)&v26 + v20);
	return result;
}