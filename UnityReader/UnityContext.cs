using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public interface UnityContext
	{
		AssetsFile LoadFile(string name);
	}
}
