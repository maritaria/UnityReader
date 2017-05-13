using System;

namespace UnityReader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			UnityContext context = new LocalUnityContext("data");
			var file = context.LoadFile("level0");

			Console.WriteLine("Loading dependencies");
			foreach (var dependency in file.Dependencies.Items)
			{
				Console.WriteLine($"Dependency: {dependency.AssetPath}");
				var loadedDependency = dependency.Load(context);
			}
			Console.WriteLine();


			Console.WriteLine("PreloadTable:");
			foreach (var obj in file.PreloadList.Items)
			{
				Console.WriteLine($"File:{obj.FileID} Path:{obj.PathID}");
				var fileDep = file.Dependencies.Items[obj.FileID];
				var loadedDep = fileDep.Load(context);
			}
			Console.WriteLine();


			Console.ReadLine();
		}
	}
}