using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class TypeTree
	{
		private Dictionary<int, RootType> types = new Dictionary<int, RootType>();
		public string UnityVersion { get; set; }
		public int Attributes { get; set; }
		public bool Embedded { get; set; }

		public TypeTree()
		{
		}

		public async Task Read(BinaryReader reader)
		{
			UnityVersion = await reader.ReadNullStringAsync(255);
			Attributes = await reader.ReadInt32Async();
			Embedded = await reader.ReadBoolAsync();
			int baseClassCount = await reader.ReadInt32Async();

			for (int i = 0; i < baseClassCount; i++)
			{
				var root = await ReadBaseClass(reader);
				types.Add(root.ClassID, root);
			}
		}

		private async Task<RootType> ReadBaseClass(BinaryReader reader)
		{
			var rootType = new RootType();
			await rootType.Read(reader, Embedded);
			return rootType;
		}
	}
}