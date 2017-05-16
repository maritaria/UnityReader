using System;
using Newtonsoft.Json.Linq;

namespace UnityReader.Types
{
	public abstract class AssetObject
	{
		public AssetsFile Owner { get; internal set; }
	}
}