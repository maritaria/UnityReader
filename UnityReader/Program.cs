using System;
using System.IO;
using System.Threading.Tasks;

namespace UnityReader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			MainAsync(args).Wait();
		}

		private static async Task MainAsync(string[] args)
		{
			using (FileStream fs = File.OpenRead("mod-nuterra"))
			{
				var reader = new BinaryReader(fs);
				var file = new UnityFile();
				await file.ReadAsync(reader);
			}
		}
	}
}