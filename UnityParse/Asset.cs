using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityParse.BakedFiles;

namespace UnityParse
{
	public sealed class Asset
	{
		public AssetCollection AssetsFile { get; }
		public AssetCode ClassID { get; }
		public JObject Data { get; }
		public bool IsPreloaded { get; set; }

		public Asset(AssetCollection file, AssetCode classID, JObject data)
		{
			AssetsFile = file;
			ClassID = classID;
			Data = data;
		}
	}
}