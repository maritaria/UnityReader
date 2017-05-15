int __thiscall AssetTypeTemplateField::MakeValue(
	AssetTypeTemplateField *this,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerPar,
	unsigned __int64 filePos,
	struct AssetTypeValueField **ppValueField,
	int bigEndian)
{
	int v7; // eax@1
	int v8; // edi@1
	int result; // eax@2
	int v12; // [sp+Ch] [bp-24h]@1
	int v13; // [sp+10h] [bp-20h]@1
	int v8; // [sp+14h] [bp-1Ch]@1
	int v15; // [sp+18h] [bp-18h]@1
	char *v16; // [sp+1Ch] [bp-14h]@3
	char *v17; // [sp+20h] [bp-10h]@3
	int v18; // [sp+24h] [bp-Ch]@3
	unsigned __int64 filePos; // [sp+28h] [bp-8h]@1

	v8 = 0;
	v12 = 0;
	v13 = 0;
	v7 = this->sub_10013BF0(callback, readerPar, &filePos, &v8, &v12, &v13, bigEndian);
	v15 = v7 * sizeof(AssetTypeValueField);
	*ppValueField = (struct AssetTypeValueField *)malloc(v8 + v12 + v13 + v15);
	if ( !*ppValueField )
	{
		*ppValueField = NULL;
		return filePos;
	}
	v17 = (char *)*ppValueField + v15;
	v16 = (char *)*ppValueField + v15 + v8;
	v15 += (int)*ppValueField + v12 + v8;
	v18 = 0;
	return this->sub_10013EA0(callback, readerPar, filePos,
		*ppValueField, 
		&v18,
		&v17,
		(int *)&v16,
		(void **)&v15,
		bigEndian);
}