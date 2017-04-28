using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class UnityFileMeta
	{
		public TypeTree TreeType = new TypeTree();
		public ObjectInfoTable ObjectInfoTable = new ObjectInfoTable();
		public ObjectIdentifierTable ObjectIDTable = new ObjectIdentifierTable();
		public FileIdentifierTable FileIdentifierTable = new FileIdentifierTable();
		public async Task ReadAsync(BinaryReader reader, UnityFileHeader header)
		{
			await TreeType.Read(reader);
			await ObjectInfoTable.Read(reader);
			await ObjectIDTable.Read(reader);
			await FileIdentifierTable.Read(reader);
		}
	}
}
