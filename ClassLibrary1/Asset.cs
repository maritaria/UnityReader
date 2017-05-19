using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ClassLibrary1
{
	public class AssetDepot
	{
		private List<Asset> _assets = new List<Asset>();
	}

	public class Asset
	{
		public AssetDepot Depot { get; }

		public int ClassID { get; set; }

		public bool IsPreload { get; set; }

		public JObject Data { get; set; }

		public Asset(AssetDepot depot)
		{
			Depot = depot;
		}
	}
}