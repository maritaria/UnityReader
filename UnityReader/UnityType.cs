using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class UnityType
	{
		public int Version { get; set; }
		public byte TreeLevel { get; set; }
		public bool IsArray { get; set; }
		public int TypeOffset { get; set; }
		public int NameOffset { get; set; }
		public int Size { get; set; }
		public int Index { get; set; }
		public int MetaFlags { get; set; }

		public string Name { get; set; }
		public string Type { get; set; }

		public async Task Read(BinaryReader reader)
		{
			Version = await reader.ReadInt16Async();
			TreeLevel = await reader.ReadByteAsync();
			IsArray = await reader.ReadBoolAsync();
			TypeOffset = await reader.ReadInt32Async();
			NameOffset = await reader.ReadInt32Async();
			Size = await reader.ReadInt32Async();
			Index = await reader.ReadInt32Async();
			MetaFlags = await reader.ReadInt32Async();
		}
	}
}
