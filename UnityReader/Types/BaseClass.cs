using System;

namespace UnityReader.Types
{
	public abstract class BaseClass
	{
		protected BinarySection _raw;
		public int ClassID { get; set; }
	}

	public abstract class BaseClass<Table, Tree> : BaseClass where Table : ClassTable where Tree : TypeTreeV1, new()
	{
		public Tree TypeTree { get; } = new Tree();
		public void Read(UnityBinaryReader reader, SerializedFileHeader header, Table table)
		{
			using (_raw = reader.StartSection())
			{
				ReadCore(reader, header, table);
			}
		}
		protected abstract void ReadCore(UnityBinaryReader reader, SerializedFileHeader header, Table table);
	}
}