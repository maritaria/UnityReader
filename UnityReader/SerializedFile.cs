using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class SerializedFile
	{
		public SerializedFileHeader Header { get; } = new SerializedFileHeader();
		public SerializedFileMetadata Meta { get; } = new SerializedFileMetadata();

		public SerializedFileObjects Objects { get; } = new SerializedFileObjects();

		public void Read(UnityBinaryReader reader)
		{
			reader.IsLittleEndian = false;
			Header.Read(reader);
			if (Header.Version < 9)
			{
				//metadata after object data
				reader.Position = Header.FileSize - Header.MetaSize + 1;
			}
			Meta.Read(reader, Header);
			Objects.Read(reader, Header, Meta);
		}
	}
}