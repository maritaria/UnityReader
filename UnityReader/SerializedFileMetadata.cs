using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Objects;
using UnityReader.Types;

namespace UnityReader
{
	public sealed class SerializedFileMetadata
	{
		public ClassTable ClassTable { get; private set; }
		public ObjectInfoTable ObjectTable { get; } = new ObjectInfoTable();
		public List<FileIdentifier> Externals { get; } = new List<FileIdentifier>();

		internal void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			if (header.Version > 5)
			{
				reader.IsLittleEndian = true;
			}
			else
			{
				reader.IsLittleEndian = false;
			}
			if (header.Version > 13)
			{
				ClassTable = new ClassTableV3();
			}
			else
			{
				ClassTable = new ClassTableV2();
			}
			ClassTable.Read(reader, header);
			ObjectTable.Read(reader, header);
			ReadExternals(reader);
		}

		private void ReadExternals(UnityBinaryReader reader)
		{
			Externals.Clear();
			uint count = reader.ReadUInt32();
			for (int i = 0; i < count; i++)
			{
				FileIdentifier external = new FileIdentifier();
				external.Read(reader);
				Externals.Add(external);
			}
		}
	}
}