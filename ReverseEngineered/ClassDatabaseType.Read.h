__int64 __thiscall ClassDatabaseType::Read(
	ClassDatabaseType *this,
	AssetsFileReader reader,
	LPARAM readerPar,
	QWORD filePos,
	int version)
{
	classId = reader.read(4);
	baseClass = reader.read(4);
	name = reader.read(4);
	fieldcount = reader.read(4)
	for(fieldCount)
	{
		fields.add();
	}
	
	
	
}