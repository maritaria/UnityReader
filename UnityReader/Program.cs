using System;
using System.IO;
using System.Threading.Tasks;

namespace UnityReader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (FileStream fs = File.OpenRead("mod-nuterra")) // level0 globalgamemanagers
			{
				var reader = new UnityBinaryReader(fs);
				var file = new SerializedFile();
				file.Read(reader);
			}
		}
	}
}