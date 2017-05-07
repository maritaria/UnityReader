using System;
using System.Collections.Generic;
using UnityReader.Objects;
using UnityReader.Types;

namespace UnityReader
{
	public class SerializedFileObjects
	{
		public List<SerializedObjectData> Items = new List<SerializedObjectData>();

		public SerializedFileObjects()
		{
		}

		public void Read(UnityBinaryReader reader, SerializedFileHeader header, SerializedFileMetadata meta)
		{
			foreach (var info in meta.ObjectInfoTable.Objects)
			{
				var obj = new SerializedObjectData();
				obj.Read(reader, header, meta, info);
				Console.WriteLine($"SerializedObjectData {obj.Class?.ClassID.ToString() ?? "null"} {obj.Data} {obj.Data.Length}");
			}
		}

		public class SerializedObjectData
		{
			public BaseClass Class { get; private set; }
			public byte[] Data { get; set; }

			public SerializedObjectData()
			{
			}

#warning solve for extremely large data lengths when the last bit for the uint is high
			public void Read(UnityBinaryReader reader, SerializedFileHeader header, SerializedFileMetadata meta, ObjectInfo info)
			{
				reader.Position = header.DataOffset + info.Offset;
				Data = reader.ReadBytes((int)info.Size);
				if (meta.ClassTable.Classes.ContainsKey(info.TypeID))
				{
					Class = meta.ClassTable.Classes[info.TypeID];
				}
			}
		}
	}
}