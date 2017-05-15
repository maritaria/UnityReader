using System;
using System.Collections.Generic;

namespace UnityReader
{
	public class UnityList<Item> where Item : UnityElement, new()
	{
		protected List<Item> Items { get; } = new List<Item>();

		public void Read(UnityBinaryReader reader, int version)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			int count = reader.ReadInt32();
			Items.Capacity = count;
			Items.Clear();
			for (int i = 0; i < count; i++)
			{
				var item = new Item();
				item.Read(reader, version);
				Items.Add(item);
			}
		}
	}
}