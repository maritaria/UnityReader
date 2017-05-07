using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.ObjectIdentifiers;
using UnityReader.Objects;
using UnityReader.Types;

namespace UnityReader
{
	public sealed class SerializedFileMetadata
	{
		private BinarySection _raw;
		public ObjectIdentifierTable ObjectIdentifierTable { get; } = new ObjectIdentifierTable();
		public ClassTable ClassTable { get; private set; }
		public ObjectInfoTable ObjectInfoTable { get; } = new ObjectInfoTable();
		public List<FileIdentifier> Externals { get; } = new List<FileIdentifier>();

		internal void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			using (_raw = reader.StartSection())
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
				ObjectInfoTable.Read(reader, header);

				if (header.Version > 10)
				{
					ReadObjectIdentifiers(reader, header);
				}

				ReadExternals(reader);
			}
		}

		private void ReadObjectIdentifiers(UnityBinaryReader reader, SerializedFileHeader header)
		{
			ObjectIdentifierTable.Read(reader, header);
		}

		private void ReadExternals(UnityBinaryReader reader)
		{
			Externals.Clear();
			uint count = reader.ReadUInt32();
			for (int i = 0; i < count; i++)
			{
				FileIdentifier external = new FileIdentifier();
				external.Read(reader);
				Console.WriteLine($"External: {external.Type} {external.Guid}");
				Console.WriteLine($"\tAsset: {external.AssetPath}");
				Console.WriteLine($"\tFile: {external.FilePath}");
				Externals.Add(external);
			}
		}
	}
}