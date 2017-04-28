using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class ObjectIdentifierTable : UnityDataTable<ObjectIdentifier>
	{
		public override async Task Read(BinaryReader reader)
		{
			await base.Read(reader);
			reader.Align(4);
		}
	}
	public sealed class ObjectIdentifier : UnityData
	{
		public int FileIndex { get; set; }
		public long IdentifierInFile { get; set; }
		public override async Task Read(BinaryReader reader)
		{
			FileIndex = await reader.ReadInt32Async();
			IdentifierInFile = await reader.ReadInt64Async();
		}
	}
}