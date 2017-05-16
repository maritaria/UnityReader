using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.MonoBehaviour)]
	public class MonoBehaviour : Component
	{
		public bool Enabled { get; set; }
		public AssetReference<MonoScript> Script { get; set; }
		public string Name { get; set; }
	}
}