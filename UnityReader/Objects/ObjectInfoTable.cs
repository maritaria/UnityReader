using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Objects
{
	public sealed class ObjectInfoTable
	{
		public List<ObjectInfo> Objects { get; } = new List<ObjectInfo>();
		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Objects.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				ObjectInfo obj = new ObjectInfo();
				obj.Read(reader, header);
				if (header.Version > 14)
				{
					reader.Align(4);
				}
				Console.WriteLine($"ObjectInfo: {obj.ObjectID}\t{obj.Offset}+{obj.Size}\t?{obj.unknown}");
				Objects.Add(obj);
			}
		}
	}
}
