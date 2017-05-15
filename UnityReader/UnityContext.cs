using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityReader.Types;

namespace UnityReader
{
	public interface UnityContext
	{
		TypeRegistry SerializationTypes { get; }

		AssetsFile LoadFile(string name);
	}
}
