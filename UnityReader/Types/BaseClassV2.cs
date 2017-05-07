using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public sealed class BaseClassV2 : BaseClass<ClassTableV3, TypeTreeV2>
	{
		public UnityHash ScriptID { get; set; }
		public UnityHash OldTypeHash { get; set; }
		public byte unknown;
		public short AlternateCount { get; set; } //-1 ignore

		public HashSet<BaseClassV2> Alternatives { get; } = new HashSet<BaseClassV2>();

		protected override void ReadCore(UnityBinaryReader reader, SerializedFileHeader header, ClassTableV3 table)
		{
			ClassID = reader.ReadInt32();
			unknown = reader.ReadByte();
			AlternateCount = reader.ReadInt16();
			if (AlternateCount != -1)
			{
				ScriptID = reader.ReadHash();
			}
			OldTypeHash = reader.ReadHash();
			if (table.Embedded)
			{
				ReadEmbeddedTree(reader, header, table);
			}
		}

		private void ReadEmbeddedTree(UnityBinaryReader reader, SerializedFileHeader header, ClassTableV3 table)
		{
			int typeCount = reader.ReadInt32();
			int stringTableLength = reader.ReadInt32();

			var types = ReadTypes(reader, header, table, typeCount);
			var stringTable = ReadStringTable(reader, table, stringTableLength);

			ApplyStringTable(types, stringTable);
			BuildTree(reader, header, table, types);
		}

		private List<TypeTreeV2> ReadTypes(UnityBinaryReader reader, SerializedFileHeader header, ClassTableV3 table, int fieldCount)
		{
			List<TypeTreeV2> types = new List<TypeTreeV2>();
			for (int i = 0; i < fieldCount; i++)
			{
				TypeTreeV2 cls = new TypeTreeV2();
				cls.Read(reader, header);
				types.Add(cls);
			}
			return types;
		}

		private Dictionary<int, string> ReadStringTable(UnityBinaryReader reader, ClassTableV3 table, int length)
		{
			var result = new Dictionary<int, string>();
			long startPos = reader.Position;
			long endPos = startPos + length;
			while (reader.Position < endPos)
			{
				int index = (int)(reader.Position - startPos);
				string str = reader.ReadString();
				result.Add(index, str);
			}
			return result;
		}

		private void ApplyStringTable(List<TypeTreeV2> types, Dictionary<int, string> stringTable)
		{
			foreach (TypeTreeV2 type in types)
			{
				type.ApplyStringTable(stringTable);
			}
		}

		private void BuildTree(UnityBinaryReader reader, SerializedFileHeader header, ClassTableV3 table, List<TypeTreeV2> types)
		{
			TypeTreeV2 previous = null;
			foreach (TypeTreeV2 current in types)
			{
				if (previous == null)
				{

				}
			}
		}
	}
}