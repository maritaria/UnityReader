using System;

namespace UnityReader
{
	public sealed class FileIdentifier
	{
		public string AssetPath { get; set; }
		public UnityHash Guid { get; set; }
		public int Type { get; set; }
		public string FilePath { get; set; }

		internal void Read(UnityBinaryReader reader)
		{
			AssetPath = reader.ReadString();
			Guid = reader.ReadHash();
			Type = reader.ReadInt32();
			FilePath = reader.ReadString();
		}
	}
}