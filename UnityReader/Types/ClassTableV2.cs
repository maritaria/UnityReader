using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	public class ClassTableV2 : ClassTable
	{
		public string Signature { get; set; }
		public Attributes Flags { get; set; }

		protected override void ReadCore(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Signature = reader.ReadString();
			Flags = (Attributes)reader.ReadInt32();
			ReadBaseClasses(reader, header);
			reader.ReadInt32();
		}

		private void ReadBaseClasses(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Classes.Clear();
			int classCount = reader.ReadInt32();
			for (int i = 0; i < classCount; i++)
			{
				var baseClass = ReadBaseClass(reader, header);
				Classes.Add(baseClass.ClassID, baseClass);
			}
		}

		private BaseClass ReadBaseClass(UnityBinaryReader reader, SerializedFileHeader header)
		{
			var baseClass = new BaseClassV1();
			baseClass.Read(reader, header, this);
			return baseClass;
		}

		[Flags]
		public enum Attributes
		{
		}
	}
}