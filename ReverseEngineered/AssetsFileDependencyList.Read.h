SWAP_ENDIAN(a)

int __thiscall AssetsFileDependencyList::Read(
	AssetsFileDependencyList this->dependencyCount,
	unsigned __int64 filePos,
	unsigned __int64 (__cdecl *callback)(unsigned __int64, unsigned __int64, void *, __int32),
	__int32 readerArg,
	unsigned __int32 version,
	bool endianness)
{
	callback(filePos, 4, &(this->dependencyCount), readerArg);
	int newFilePos = filePos + 4;
	if ( endianness )
		SWAP_ENDIAN(this->dependencyCount);
	if ( this->dependencyCount )
	{
		this->pDependencies = malloc(sizeof(AssetsFileDependency) * this->dependencyCount);
		v14 = 0;
		if ( this->pDependencies )
		{
			for (long i = 0; i < this->pDependencies; i++) {
				newFilePos = this->pDependencies[i]::Read(newFilePos, callback, readerArg, endianness);
			}
		}
		else
		{
			for (long i = 0; i < this->pDependencies; i++) {
				AssetsFileDependency tempDep();
				newFilePos = tempDep::Read(newFilePos, callback, readerArg, endianness);
			}
		}
	}
	else
	{
		this->pDependencies = NULL;
	}
	return newFilePos;
}