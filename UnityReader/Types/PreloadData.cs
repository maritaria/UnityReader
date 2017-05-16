using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.PreloadData)]
	public sealed class PreloadData : AssetObject
	{
		public string Name { get; set; }
		public ICollection<AssetReference<AssetObject>> Assets { get; } = new List<AssetReference<AssetObject>>();
		public ICollection<string> Dependencies { get; }
	}
}
