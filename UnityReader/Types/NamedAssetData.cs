using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	public interface NamedAssetData : AssetData
	{
		string Name { get; set; }
	}
}
