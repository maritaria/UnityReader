const char *__thiscall ClassDatabaseFileString::GetString(
	ClassDatabaseFileString *this,
	struct ClassDatabaseFile *pFile)
{
	const char *result; // eax@2
	const char *v3; // ecx@3

	if ( this->fromStringTable )
	{
		v3 = *(const char **)this;
		if ( (unsigned int)v3 < *((_DWORD *)pFile + 7) )
		{
			result = &v3[*((_DWORD *)pFile + 13)];
		}
		else
		{
			return NULL;
		}
	}
	else
	{
		return this->str.string;
	}
}