using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class UnityFile
	{
		public UnityFileHeader Header { get; set; } = new UnityFileHeader();
		public UnityFileMeta Meta { get; set; } = new UnityFileMeta();

		public async Task ReadAsync(BinaryReader reader)
		{
			reader.IsLittleEndian = false;
			await Header.ReadAsync(reader);
			if (Header.Version < 9)
			{
				reader.Position = Header.FileSize - Header.MetaSize + 1;
			}
			if (Header.Version > 5)
			{
				reader.IsLittleEndian = true;
			}
			await Meta.ReadAsync(reader, Header);
		}
	}
}
