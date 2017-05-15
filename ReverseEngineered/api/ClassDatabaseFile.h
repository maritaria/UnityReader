#pragma once
#include "AssetsFileFormat.h"
#include "defines.h"
#include <vector>

//custom file type to store Unity type information
#define ClassDatabaseFileVersion 3
#define ClassDatabaseCompressionType 1 //LZ4 compress by default
#define ClassDatabasePackageVersion 0

class ClassDatabaseFile;
struct ClassDatabaseFileString
{
	union {
		DWORD stringTableOffset;
		const char *string;
	} str;
	bool fromStringTable;//this+4
	ASSETSTOOLS_API const char *GetString(ClassDatabaseFile *pFile);
	ASSETSTOOLS_API QWORD Read(AssetsFileReader reader, LPARAM readerPar, QWORD filePos);
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos);
};
struct ClassDatabaseTypeField
{
	ClassDatabaseFileString typeName;//this+0
	ClassDatabaseFileString fieldName;//this+8
	BYTE depth;//this+16
	BYTE isArray;//this+17
	DWORD size;//this+20
	//DWORD index;
	WORD version;//this+24
	DWORD flags2;//this+28
	
	ASSETSTOOLS_API ClassDatabaseTypeField();
	ASSETSTOOLS_API ClassDatabaseTypeField(const ClassDatabaseTypeField& other);
	ASSETSTOOLS_API QWORD Read(AssetsFileReader reader, LPARAM readerPar, QWORD filePos, int version); //reads version 0,1,2,3
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos, int version); //writes version 1,2,3
};
class ClassDatabaseType
{
public:
	int classId;
	int baseClass;
	ClassDatabaseFileString name;

	std::vector<ClassDatabaseTypeField> fields;
	//DWORD fieldCount;
	//ClassDatabaseTypeField *fields;
	ASSETSTOOLS_API ClassDatabaseType();
	ASSETSTOOLS_API ClassDatabaseType(const ClassDatabaseType& other);
	ASSETSTOOLS_API ~ClassDatabaseType();
	ASSETSTOOLS_API QWORD Read(AssetsFileReader reader, LPARAM readerPar, QWORD filePos, int version);
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos, int version);
	ASSETSTOOLS_API Hash128 MakeTypeHash(ClassDatabaseFile *pDatabaseFile); 
};
struct ClassDatabaseFileHeader
{
	char header[4];
	BYTE fileVersion;

	BYTE compressionType; //version 2; 0 = none, 1 = LZ4
	DWORD compressedSize, uncompressedSize;  //version 2
	//BYTE assetsVersionCount; //version 0 only
	//BYTE *assetsVersions; //version 0 only

	BYTE unityVersionCount;//this+16
	char **pUnityVersions;//this+20 // array of strings with length prefix byte


	DWORD stringTableLen;//this+24
	DWORD stringTablePos;//this+28
	ASSETSTOOLS_API QWORD Read(AssetsFileReader reader, LPARAM readerPar, QWORD filePos);
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos);
	//DWORD _tmp; //used only if assetsVersions == NULL; version 0 only
};
//all classes that override Component : prepend PPtr<GameObject> m_GameObject
//Transform : add vector m_Children {Array Array {int size; PPtr<Transform> data}}
class ClassDatabaseFile
{
	bool valid;//this+0 [0]
public:
	//Only for internal use, otherwise this could create a memory leak!
	bool dontFreeStringTable;//this+0 [1]
	ClassDatabaseFileHeader header;//this + 4

	std::vector<ClassDatabaseType> classes;//this+44
	//DWORD classCount;
	//ClassDatabaseType *classes;

	char *stringTable;//this+52

public:
	
	ASSETSTOOLS_API QWORD Read(AssetsFileReader reader, LPARAM readerPar, QWORD filePos);
	ASSETSTOOLS_API bool Read(AssetsFileReader reader, LPARAM readerPar);
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos, int optimizeStringTable=1, DWORD compress=1, bool writeStringTable=true);
	ASSETSTOOLS_API bool IsValid();

	ASSETSTOOLS_API bool InsertFrom(ClassDatabaseFile *pOther, ClassDatabaseType *pType);
	ASSETSTOOLS_API void Clear();
	
	ASSETSTOOLS_API ClassDatabaseFile();
	ASSETSTOOLS_API ClassDatabaseFile(const ClassDatabaseFile& other);
	ASSETSTOOLS_API ~ClassDatabaseFile();
};
typedef ClassDatabaseFile* PClassDatabaseFile;