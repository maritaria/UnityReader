using System;
using System.Collections.Generic;
using System.Linq;
using FlexParse;
using Newtonsoft.Json.Linq;
using UnityParse.BakedFiles;

namespace UnityParse
{
	public sealed class Asset
	{
		public AssetCollection AssetsFile { get; }
		public AssetCode ClassID { get; }

		public byte[] Data { get; set; }
		public bool IsPreloaded { get; set; }

		public Asset(AssetCollection file, AssetCode classID, byte[] data)
		{
			AssetsFile = file;
			ClassID = classID;
			Data = data;
		}
	}
}