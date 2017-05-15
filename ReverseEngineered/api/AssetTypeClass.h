#pragma once
#include "AssetsFileFormat.h"
#include "ClassDatabaseFile.h"

#ifndef DWORD
typedef unsigned long DWORD;
#endif
#ifndef QWORD
typedef unsigned long long QWORD;
#endif
#ifndef __AssetsTools_AssetsFileFunctions_Read
#define __AssetsTools_AssetsFileFunctions_Read
typedef QWORD(_cdecl *AssetsFileReader)(QWORD pos, QWORD count, void *pBuf, LPARAM par);
typedef void(_cdecl *AssetsFileVerifyLogger)(char *message);
#endif
#ifndef __AssetsTools_AssetsFileFunctions_Write
#define __AssetsTools_AssetsFileFunctions_Write
typedef QWORD(_cdecl *AssetsFileWriter)(QWORD pos, QWORD count, const void *pBuf, LPARAM par);
#endif

#ifdef ASSETSTOOLS_EXPORTS
#define ASSETSTOOLS_API __declspec(dllexport) 
#else
#define ASSETSTOOLS_API __declspec(dllimport) 
#endif

class AssetTypeValueField;
//class to read asset files using type information
struct AssetTypeArray
{
	DWORD size;
	//AssetTypeValueField *dataField;
};
struct AssetTypeByteArray
{
	DWORD size;
	BYTE *data;
};

enum EnumValueTypes
{
	ValueType_None,//0
	ValueType_Bool,//1
	ValueType_Int8,//2
	ValueType_UInt8,//3
	ValueType_Int16,//4
	ValueType_UInt16,//5
	ValueType_Int32,//6
	ValueType_UInt32,//7
	ValueType_Int64,//8
	ValueType_UInt64,//9
	ValueType_Float,//10
	ValueType_Double,//11
	ValueType_String,//12
	ValueType_Array,//13
	ValueType_ByteArray//14
};

class AssetTypeValue//size:16
{
	//bool freeValue;
	EnumValueTypes type;//this+0
	union ValueTypes
	{
		AssetTypeArray asArray;
		AssetTypeByteArray asByteArray;

		bool asBool;

		char asInt8;
		unsigned char asUInt8;

		short asInt16;
		unsigned short asUInt16;

		int asInt32;
		unsigned int asUInt32;

		long long int asInt64;
		unsigned long long int asUInt64;

		float asFloat;
		double asDouble;

		char *asString;
	} value;//this+4
public:
	//Creates an AssetTypeValue.
	//type : the value type which valueContainer stores
	//valueContainer : the buffer for the value type
	//freeIfPointer : should the value get freed if value type is Array/String
	ASSETSTOOLS_API AssetTypeValue(EnumValueTypes type, void *valueContainer);
	ASSETSTOOLS_API ~AssetTypeValue();
	inline EnumValueTypes GetType()
	{
		return type;
	}
	ASSETSTOOLS_API void Set(void *valueContainer);
	inline AssetTypeArray *AsArray()
	{
		return (type == ValueType_Array) ? &value.asArray : NULL; 
	}
	inline AssetTypeByteArray *AsByteArray()
	{
		return (type == ValueType_ByteArray) ? &value.asByteArray : NULL; 
	}
	inline char *AsString()
	{
		return (type == ValueType_String) ? value.asString : NULL; 
	}
	inline bool AsBool()
	{
		switch (type)
		{
		case ValueType_Float:
		case ValueType_Double:
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return false;
		default:
			return value.asBool;
		}
	}
	inline int AsInt()
	{
		switch (type)
		{
		case ValueType_Float:
			return (int)value.asFloat;
		case ValueType_Double:
			return (int)value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		case ValueType_Int8:
			return (int)value.asInt8;
		case ValueType_Int16:
			return (int)value.asInt16;
		case ValueType_Int64:
			return (int)value.asInt64;
		default:
			return value.asInt32;
		}
	}
	inline unsigned int AsUInt()
	{
		switch (type)
		{
		case ValueType_Float:
			return (unsigned int)value.asFloat;
		case ValueType_Double:
			return (unsigned int)value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		default:
			return value.asUInt32;
		}
	}
	inline long long int AsInt64()
	{
		switch (type)
		{
		case ValueType_Float:
			return (long long int)value.asFloat;
		case ValueType_Double:
			return (long long int)value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		case ValueType_Int8:
			return (long long int)value.asInt8;
		case ValueType_Int16:
			return (long long int)value.asInt16;
		case ValueType_Int32:
			return (long long int)value.asInt32;
		default:
			return value.asInt64;
		}
	}
	inline unsigned long long int AsUInt64()
	{
		switch (type)
		{
		case ValueType_Float:
			return (unsigned int)value.asFloat;
		case ValueType_Double:
			return (unsigned long long int)value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		default:
			return value.asUInt64;
		}
	}
	inline float AsFloat()
	{
		switch (type)
		{
		case ValueType_Float:
			return value.asFloat;
		case ValueType_Double:
			return (float)value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		case ValueType_Int8:
			return (float)value.asInt8;
		case ValueType_Int16:
			return (float)value.asInt16;
		case ValueType_Int32:
			return (float)value.asInt32;
		default:
			return (float)value.asUInt64;
		}
	}
	inline double AsDouble()
	{
		switch (type)
		{
		case ValueType_Float:
			return (double)value.asFloat;
		case ValueType_Double:
			return value.asDouble;
		case ValueType_String:
		case ValueType_ByteArray:
		case ValueType_Array:
			return 0;
		case ValueType_Int8:
			return (double)value.asInt8;
		case ValueType_Int16:
			return (double)value.asInt16;
		case ValueType_Int32:
			return (double)value.asInt32;
		default:
			return (double)value.asUInt64;
		}
	}
};

class AssetTypeValueField;
class AssetTypeTemplateField
{
public:
	const char *name;//this+0
	const char *type;//this+4
	EnumValueTypes valueType;//this+8
	bool isArray;//this+12
	bool align;//this+13
	bool hasValue;//this+14
	DWORD childrenCount;//this+16
	AssetTypeTemplateField *children;//this+20
	
public:
	ASSETSTOOLS_API AssetTypeTemplateField();
	ASSETSTOOLS_API ~AssetTypeTemplateField();
	ASSETSTOOLS_API void Clear();
	ASSETSTOOLS_API bool From0D(Type_0D *pU5Type, DWORD fieldIndex);
	ASSETSTOOLS_API bool FromClassDatabase(ClassDatabaseFile *pFile, ClassDatabaseType *pType, DWORD fieldIndex);
	ASSETSTOOLS_API bool From07(TypeField_07 *pTypeField);
	ASSETSTOOLS_API QWORD MakeValue(AssetsFileReader reader, LPARAM readerPar, QWORD filePos, AssetTypeValueField **ppValueField, bool bigEndian);

	ASSETSTOOLS_API AssetTypeTemplateField *SearchChild(const char* name);
};
ASSETSTOOLS_API void ClearAssetTypeValueField(AssetTypeValueField *pValueField);
class AssetTypeValueField
{
protected:
	AssetTypeTemplateField *templateField;//this+0
	
	DWORD childrenCount;//this+4
	AssetTypeValueField **pChildren;//this+8
	AssetTypeValue *value;//this+12 //pointer so it may also have no value (NULL)
public:

	ASSETSTOOLS_API void Read(AssetTypeValue *pValue, AssetTypeTemplateField *pTemplate, DWORD childrenCount, AssetTypeValueField **pChildren);
	ASSETSTOOLS_API QWORD Write(AssetsFileWriter writer, LPARAM writerPar, QWORD filePos);

	//ASSETSTOOLS_API void Clear();

	//get a child field by its name
	ASSETSTOOLS_API AssetTypeValueField* operator[](const char* name);
	//get a child field by its index
	ASSETSTOOLS_API AssetTypeValueField* operator[](DWORD index);

	inline AssetTypeValueField* Get(const char* name) { return (*this)[name]; }
	inline AssetTypeValueField* Get(unsigned int index) { return (*this)[index]; }

	inline const char *GetName() { return templateField->name; }
	inline const char *GetType() { return templateField->type; }
	inline AssetTypeValue *GetValue() { return value; }
	inline AssetTypeTemplateField *GetTemplateField() { return templateField; }
	inline AssetTypeValueField **GetChildrenList() { return pChildren; }
	inline void SetChildrenList(AssetTypeValueField **pChildren, DWORD childrenCount) { this->pChildren = pChildren; this->childrenCount = childrenCount; }

	inline DWORD GetChildrenCount() { return childrenCount; }

	ASSETSTOOLS_API bool IsDummy();

	ASSETSTOOLS_API QWORD GetByteSize(QWORD filePos = 0);
};
ASSETSTOOLS_API EnumValueTypes GetValueTypeByTypeName(const char *type);
ASSETSTOOLS_API AssetTypeValueField* GetDummyAssetTypeField();

class AssetTypeInstance
{
	DWORD baseFieldCount;//this+0
	AssetTypeValueField **baseFields;//this+4
	DWORD allocationCount;//this+8
	DWORD allocationBufLen;//this+12
	void **memoryToClear;//this+16
public:
	//The reader is at the beginning of the asset data (near _Uscript in hex)
	ASSETSTOOLS_API AssetTypeInstance(DWORD baseFieldCount, AssetTypeTemplateField **ppBaseFields, AssetsFileReader reader, LPARAM readerPar, bool bigEndian, QWORD filePos = 0);
	ASSETSTOOLS_API bool SetChildList(AssetTypeValueField *pValueField, AssetTypeValueField **pChildrenList, DWORD childrenCount, bool freeMemory = true);
	ASSETSTOOLS_API bool AddTempMemory(void *pMemory);
	ASSETSTOOLS_API ~AssetTypeInstance();

	inline AssetTypeValueField *GetBaseField(DWORD index = 0)
	{
		if (index >= baseFieldCount)
			return GetDummyAssetTypeField();
		return baseFields[index];
	}
};