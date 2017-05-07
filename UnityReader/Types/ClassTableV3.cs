using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	public class ClassTableV3 : ClassTableV2
	{
		public bool Embedded { get; set; }
		protected override void ReadCore(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Signature = reader.ReadString();
			Flags = (Attributes)reader.ReadInt32();
			Embedded = reader.ReadBool();
			ReadBaseClasses(reader, header);
		}

		private void ReadBaseClasses(UnityBinaryReader reader, SerializedFileHeader header)
		{
			Classes.Clear();
			int classCount = reader.ReadInt32();
			for (int i = 0; i < classCount; i++)
			{
				var baseClass = ReadBaseClass(reader, header);

				Console.WriteLine($"New: {baseClass.ClassID} {baseClass.AlternateCount} {baseClass.unknown}");
				Console.WriteLine($"\t{baseClass.OldTypeHash}");
				Console.WriteLine($"\t{baseClass.ScriptID}");
				Classes.Add(baseClass.ClassID, baseClass);
				if (baseClass.AlternateCount != -1)
				{
					for (int j = 0; j < baseClass.AlternateCount; j++)
					{
						var alt = ReadBaseClass(reader, header);
						Console.WriteLine($"\tAlt: {alt.ClassID} {alt.AlternateCount} {alt.unknown}");
						Console.WriteLine($"\t\t{alt.OldTypeHash}");
						Console.WriteLine($"\t\t{alt.ScriptID}");
						baseClass.Alternatives.Add(alt);
						i++;
					}
				}
			}
		}

		private BaseClassV2 ReadBaseClass(UnityBinaryReader reader, SerializedFileHeader header)
		{
			var baseClass = new BaseClassV2();
			baseClass.Read(reader, header, this);
			return baseClass;
		}
	}
}