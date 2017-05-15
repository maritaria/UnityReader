using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnityReader.Types
{
	[DebuggerDisplay("FileID = {FileID}, PathID = {PathID}")]
	public class AssetReference<T> : AssetData where T : AssetData
	{
		public AssetsFile Owner { get; set; }
		public int FileID { get; set; }
		public long PathID { get; set; }

		internal T DebugView => GetAsset();

		public T GetAsset()
		{
			AssetsFile source;
			if (FileID == 0)
			{
				source = Owner;
			}
			else
			{
				source = Owner.Dependencies.GetFile(FileID);
			}
			return source.Assets[PathID].GetAsset<T>();
		}

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Owner = owner;
			FileID = reader.ReadInt32();
			PathID = reader.ReadInt64();
		}
	}

}