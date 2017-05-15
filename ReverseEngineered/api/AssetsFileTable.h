#pragma once
#include "AssetsFileFormat.h"

#include "defines.h"

class AssetFileInfo //size: 32
{
	public:
		unsigned __int64 index;			//0x00 00 //little-endian //version < 0x0E : only DWORD
		DWORD offs_curFile;				//0x08 08 //little-endian
		DWORD curFileSize;				//0x0C 12 //little-endian
		DWORD curFileTypeOrIndex;		//0x10 16 //little-endian //starting with version 0x10, this is an index into the type tree
		//inheritedUnityClass : for Unity classes, this is curFileType; for MonoBehaviours, this is 114
		//version < 0x0B : inheritedUnityClass is DWORD, no scriptIndex exists
		WORD inheritedUnityClass;		//0x14 20 //little-endian (MonoScript)//only version < 0x10
		//scriptIndex : for Unity classes, this is 0xFFFF; for MonoBehaviours, this is the index of the script in the MonoManager asset
		WORD scriptIndex;				//0x16 22 //little-endian//only version <= 0x10
		BYTE unknown1;					//0x18 24 //only 0x0F <= version <= 0x10 //with alignment always a DWORD
		ASSETSTOOLS_API static DWORD GetSize(DWORD version);
		ASSETSTOOLS_API QWORD Read(DWORD version, QWORD pos, AssetsFileReader reader, LPARAM readerPar, bool bigEndian);
		ASSETSTOOLS_API QWORD Write(DWORD version, QWORD pos, AssetsFileWriter writer, LPARAM writerPar);
};

class AssetFileInfoEx : public AssetFileInfo //size: 152
{
	//Alignment: cpp.sh/5r7a
	public:
		//AssetsHeader format < 0x10 : equals curFileTypeOrIndex
		//AssetsHeader format >= 0x10 : equals TypeTree.pTypes_Unity5[curFileTypeOrIndex].classId or (DWORD)-2 if the index is out of bounds
		DWORD curFileType;//0x1A 32
		QWORD absolutePos;//0x1E 40
		char name[100];//0x26 48
};

//File tables make searching assets easier
//The names are always read from the asset itself if possible,
//	see AssetBundleFileTable or ResourceManagerFile for more/better names
class AssetsFileTable
{
	AssetsFile *pFile;//0x00
	AssetsFileReader reader;//0x04
	LPARAM readerPar;//0x08

	public:
		AssetFileInfoEx *pAssetFileInfo;//0x0C
		unsigned int assetFileInfoCount;//0x10

	public:
		//Reading names requires a high random access, set readNames to false if you don't need them
		ASSETSTOOLS_API AssetsFileTable(AssetsFile *pFile, bool readNames=true);
		ASSETSTOOLS_API ~AssetsFileTable();

		ASSETSTOOLS_API AssetFileInfoEx *getAssetInfo(const char *name);
		ASSETSTOOLS_API AssetFileInfoEx *getAssetInfo(const char *name, DWORD type);
		ASSETSTOOLS_API AssetFileInfoEx *getAssetInfo(QWORD pathId);
		
		ASSETSTOOLS_API AssetsFile *getAssetsFile();
		ASSETSTOOLS_API AssetsFileReader getReader();
		ASSETSTOOLS_API LPARAM getReaderPar();
};
