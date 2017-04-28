using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityReader
{
	public class UnityDataTable<T> where T : UnityData, new()
	{
		public List<T> Items { get; set; } = new List<T>();

		public virtual async Task Read(BinaryReader reader)
		{
			int entries = await reader.ReadInt32Async();
			for (int i = 0; i < entries; i++)
			{
				Items.Add(await ReadItem(reader));
			}
		}

		private async Task<T> ReadItem(BinaryReader reader)
		{
			var result = new T();
			await result.Read(reader);
			return result;
		}
	}

	public class UnityData
	{
		public virtual async Task Read(BinaryReader reader)
		{
		}
	}
}