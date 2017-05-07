using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	public abstract class ClassTable
	{
		private BinarySection _raw;
		public Dictionary<int, BaseClass> Classes { get; } = new Dictionary<int, BaseClass>();

		public void Read(UnityBinaryReader reader, SerializedFileHeader header)
		{
			using (_raw = reader.StartSection())
			{
				ReadCore(reader, header);
			}
		}
		protected abstract void ReadCore(UnityBinaryReader reader, SerializedFileHeader header);


	}
}