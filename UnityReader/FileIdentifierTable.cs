using System;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class FileIdentifierTable : UnityDataTable<FileIdentifier>
	{
	}

	public class FileIdentifier : UnityData
	{
		public int Type { get; set; }
		public string FilePath { get; set; }
		public UnityGuid Guid { get; set; } = new UnityGuid();

		public string AssetPath { get; set; }

		public override async Task Read(BinaryReader reader)
		{
			//v2
			AssetPath = await reader.ReadNullStringAsync();
			//v1
			await Guid.Read(reader);
			Type = await reader.ReadInt32Async();
			FilePath = await reader.ReadNullStringAsync();
		}
	}
}