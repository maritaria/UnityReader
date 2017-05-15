using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	[UnityType(150)]
	public sealed class PreloadData : NamedAssetData
	{
		private List<string> _dependencies = new List<string>();
		public string Name { get; set; }
		public ICollection<AssetReference<AssetObject>> Assets { get; } = new List<AssetReference<AssetObject>>();

		public ICollection<string> Dependencies => _dependencies;

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Name = reader.ReadStringFixed(reader.ReadInt32());
			reader.ReadArray(owner, Assets);
			int depCount = reader.ReadInt32();
			_dependencies.Clear();
			_dependencies.Capacity = depCount;
			for (int i = 0; i < depCount; i++)
			{
				_dependencies.Add(reader.ReadStringFixed(reader.ReadInt32()));
			}
		}
	}
}
