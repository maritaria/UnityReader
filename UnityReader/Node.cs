using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityReader
{
	public class Node
	{
		private HashSet<Node> _children = new HashSet<Node>();
		private Node _parent;
		private UnityType _data;

		public async Task Read(BinaryReader reader)
		{
			int numFields = await reader.ReadInt32Async();
			int stringTableLength = await reader.ReadInt32Async();

			List<UnityType> types = new List<UnityType>(numFields);

			for (int i = 0; i < numFields; i++)
			{
				UnityType t = new UnityType();
				await t.Read(reader);
				types.Add(t);
			}

			var stringTable = await ReadStringTable(reader, stringTableLength);

			foreach (UnityType field in types)
			{
				string name = stringTable[field.NameOffset];
				field.Name = name;

				string type = stringTable[field.TypeOffset];
				field.Type = type;
			}

			Node previous = null;
			foreach (UnityType current in types)
			{
				if (previous == null)
				{
					_data = current;
				}
				else
				{
					Node node = new Node();
					node._data = current;
					int levels = previous._data.TreeLevel - current.TreeLevel;
					if (levels >= 0)
					{
						for (int i = 0; i < levels; i++)
						{
							previous = previous._parent;
						}
						previous._parent.Add(node);
					}
					else
					{
						previous.Add(node);
					}
				}
				previous = this;
			}
		}

		public void Add(Node child)
		{
			_children.Add(child);
		}

		private async Task<Dictionary<int, string>> ReadStringTable(BinaryReader reader, int length)
		{
			var result = new Dictionary<int, string>();

			long start = reader.Position;
			long end = start + length;
			while (reader.Position < end)
			{
				int index = (int)(reader.Position - start);
				string s = await reader.ReadNullStringAsync();
				result.Add(index, s);
			}
			return result;
		}
	}
}