using System;
using Newtonsoft.Json.Linq;
using UnityReader.Types;

namespace UnityReader
{
	public sealed class AssetFileInfo
	{
		private byte[] _data;
		private JObject _newValue;
		private int _fileTypeOrIndex;
		private long _absolutePosition;
		private uint _dataOffset;
		private uint _dataLength;
		public bool Modified { get; private set; }

		public AssetsFile Owner { get; }
		public long Index { get; private set; }
		public int InheritedUnityClass { get; private set; } = 0;//read 16 bit
		public short ScriptIndex { get; private set; } = -1;
		public byte Unknown { get; private set; } = 0;

		public AssetCodes ClassID { get; private set; }

		public AssetFileInfo(AssetsFile file, UnityReader reader)
		{
			Owner = file;
			var version = file.Header.Version;

			ReadData(reader, version);
			ParseData(file, version);
		}

		private void ReadData(UnityReader reader, int version)
		{
			if (version < 14)
			{
				Index = reader.ReadUInt32();
			}
			else
			{
				reader.Align(4);
				Index = reader.ReadInt64();
			}
			_dataOffset = reader.ReadUInt32();
			_dataLength = reader.ReadUInt32();
			_fileTypeOrIndex = reader.ReadInt32();
			if (version < 16)
			{
				ClassID = (AssetCodes)_fileTypeOrIndex;
				InheritedUnityClass = reader.ReadInt16();
			}
			if (version < 17)
			{
				ScriptIndex = reader.ReadInt16();
				Unknown = reader.ReadByte();
			}
		}

		private void ParseData(AssetsFile file, int version)
		{
			if (version > 15)
			{
				if (_fileTypeOrIndex < file.TypeTree.Types.Count)
				{
					var type = file.TypeTree.Types[_fileTypeOrIndex];

					if (type.ScriptIndex == -1)
					{
						ClassID = (AssetCodes)type.ClassID;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = -1;
					}
					else
					{
						ClassID = (AssetCodes)type.ClassID;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = type.ScriptIndex;
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			_absolutePosition = file.Header.AssetsOffset + _dataOffset;
			using (var reader = Owner.CreateReader(_absolutePosition))
			{
				_data = reader.ReadBytes((int)_dataLength);
			}
		}

		public T ParseAssetData<T>() where T : AssetObject
		{
			var json = ParseAssetData();
			var obj = json.ToObject<T>();
			obj.Owner = Owner;
			return obj;
		}

		public JObject ParseAssetData()
		{
			using (var reader = UnityReader.FromByteArray(_data))
			{
				var template = Owner.Context.TypeTable[ClassID];
				var result = new JObject();
				template.Read(reader, Owner.Context, result);
				return result;
			}
		}

		public void StoreAssetData(JObject data)
		{
			if (data == null) throw new ArgumentNullException(nameof(data));
			_newValue = data;
		}
	}
}