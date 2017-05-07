using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.ObjectIdentifiers
{
	public class ObjectIdentifierTable
	{
		private List<ObjectIdentifier> _items = new List<ObjectIdentifier>();

		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			int itemCount = reader.ReadInt32();
			for (int i = 0; i < itemCount; i++)
			{
				ObjectIdentifier item = ReadItem(reader, header);
				Console.WriteLine($"ObjectIdentifier {item.SerializedFileIndex} {item.IdentifierInFile}");
				_items.Add(item);
			}
		}

		private ObjectIdentifier ReadItem(UnityBinaryReader reader, SerializedFileHeader header)
		{
			var result = new ObjectIdentifier();
			result.Read(reader, header);
			return result;
		}
	}
}
