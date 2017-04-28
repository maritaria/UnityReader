using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public class TypeTreeV1
	{
		protected uint _size;
		protected uint _index;
		protected bool _isArray;

		public string Type { get; set; }
		public string Name { get; set; }
		public Attributes MetaFlags { get; set; }
		public int Version { get; set; }
		public HashSet<TypeTreeV1> Children { get; } = new HashSet<TypeTreeV1>();

		public virtual void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Type = reader.ReadString();
			Name = reader.ReadString();
			_size = reader.ReadUInt32();
			_index = reader.ReadUInt32();
			_isArray = reader.ReadUInt32() != 0;
			Version = reader.ReadInt32();
			MetaFlags = (Attributes)reader.ReadUInt32();
			ReadChildren(reader, header);
		}

		private void ReadChildren(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Children.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var child = ReadChild(reader, header, this);
				Children.Add(child);
			}
		}

		private TypeTreeV1 ReadChild(UnityBinaryReader reader, SerializedFileHeader header, TypeTreeV1 parent)
		{
			var item = new TypeTreeV1();
			item.Read(reader, header);
			return item;
		}

		[Flags]
		public enum Attributes
		{
			FieldValueAlwaysAligned = 0x4000,
		}
	}
}