using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace UnityReader.Types
{
	[DebuggerDisplay("FileID = {FileID}, PathID = {PathID}")]
	public sealed class AssetReference<T> where T : AssetObject
	{
#warning "figure out how to get owner"
		public AssetsFile Owner { get; set; }
		public int FileID { get; set; }
		public long PathID { get; set; }

		public AssetFileInfo GetAssetInfo()
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
			return source.Assets[PathID];
		}

		public T GetAssetObject()
		{
			return GetAssetInfo().ParseAssetData<T>();
		}
	}

}