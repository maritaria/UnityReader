using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.ObjectIdentifiers
{
	public class ObjectIdentifier
	{
		public int SerializedFileIndex { get; set; }
		public long IdentifierInFile { get; set; }

		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			SerializedFileIndex = reader.ReadInt32();
			IdentifierInFile = reader.ReadInt64();
		}
	}
}
