using System;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class RootType
	{
		public int ClassID { get; set; }
		public Hash ScriptID { get; set; } = new Hash();
		public Hash OldTypeHash { get; set; } = new Hash();
		public Node RootNode { get; set; } = new Node();

		public RootType()
		{
		}

		public async Task Read(BinaryReader reader, bool embedded)
		{
			ClassID = await reader.ReadInt32Async();
			if (ClassID < 0)
			{
				await ScriptID.Read(reader);
			}
			await OldTypeHash.Read(reader);
			if (embedded)
			{
				await RootNode.Read(reader);
			}
		}
	}
}