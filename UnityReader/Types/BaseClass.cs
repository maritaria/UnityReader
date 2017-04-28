using System;

namespace UnityReader.Types
{
	public abstract class BaseClass
	{
		public int ClassID { get; set; }
	}

	public abstract class BaseClass<Table, Tree> : BaseClass where Table : ClassTable where Tree : TypeTreeV1, new()
	{
		public Tree TypeTree { get; } = new Tree();
		public abstract void Read(UnityBinaryReader reader, SerializedFileHeader header, Table table);
	}
}