using System;
using System.Collections.Generic;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader
{
	public interface UnityContext
	{
		TypeTable TypeTable { get; }

		AssetsFile LoadFile(string name);
	}
}