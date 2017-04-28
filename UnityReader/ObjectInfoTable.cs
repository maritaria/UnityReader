using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class ObjectInfoTable
	{
		public Dictionary<long, ObjectInfo> objects = new Dictionary<long, ObjectInfo>();

		public async Task Read(BinaryReader reader)
		{
			int entries = await reader.ReadInt32Async();
			for (int i = 0; i < entries; i++)
			{
				reader.Align(4);
				long pathID = await reader.ReadInt64Async();
				ObjectInfo info = new ObjectInfo();
				await info.Read(reader);
				objects.Add(pathID, info);
			}
		}
	}
}