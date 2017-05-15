using System;

namespace UnityReader
{
	public sealed class AssetFileInfo
	{
		private byte[] _buffer;
		private int _fileTypeOrIndex;
		private long _absolutePosition;
		public AssetsFile Owner { get; }
		public long Index { get; set; }
		public uint DataOffset { get; set; }
		public uint AssetSize { get; set; }
		public int InheritedUnityClass { get; set; }//read 16 bit
		public short ScriptIndex { get; set; }
		public byte Unknown { get; set; }

		public int ClassID { get; set; }

		public AssetFileInfo(AssetsFile file, UnityBinaryReader reader)
		{
			Owner = file;
			var version = file.Header.Version;

			ReadData(reader, version);
			ParseData(file, version);
		}

		private void ReadData(UnityBinaryReader reader, int version)
		{
			if (version >= 14)
			{
				reader.Align(4);
			}

			if (version < 14)
			{
				Index = reader.ReadUInt32();
			}
			else
			{
				Index = reader.ReadInt64();
			}
			DataOffset = reader.ReadUInt32();
			AssetSize = reader.ReadUInt32();
			_fileTypeOrIndex = reader.ReadInt32();
			if (version >= 16)
			{
				InheritedUnityClass = 0;
			}
			else
			{
				InheritedUnityClass = reader.ReadInt16();
			}
			if (version > 16)
			{
				ScriptIndex = -1;
				Unknown = 0;
			}
			else
			{
				ScriptIndex = reader.ReadInt16();
				Unknown = reader.ReadByte();
			}
		}

		private void ParseData(AssetsFile file, int version)
		{
			if (version < 16)
			{
				ClassID = _fileTypeOrIndex;
			}
			else
			{
				if (_fileTypeOrIndex < file.TypeTree.Types.Count)
				{
					var type = file.TypeTree.Types[_fileTypeOrIndex];

					if (type.ScriptIndex == -1)
					{
						ClassID = type.ClassID;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = -1;
					}
					else
					{
						ClassID = type.ClassID;
						InheritedUnityClass = type.ClassID;
						ScriptIndex = type.ScriptIndex;
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			_absolutePosition = file.Header.AssetsOffset + DataOffset;
		}

		public T GetAsset<T>() where T : AssetData
		{
			using (var reader = Owner.CreateReader(_absolutePosition))
			{
				long oldPos = reader.Position;
				reader.Position = _absolutePosition;
				AssetData result = Owner.Context.SerializationTypes.CreateInstance<T>(ClassID);
				result.Read(Owner, reader);
				return (T)result;
			}
		}
	}
}