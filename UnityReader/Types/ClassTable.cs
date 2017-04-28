using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityReader.Types
{
	public abstract class ClassTable
	{
		public Dictionary<int, BaseClass> Classes { get; } = new Dictionary<int, BaseClass>();

		public abstract void Read(UnityBinaryReader reader, SerializedFileHeader header);

	}
}