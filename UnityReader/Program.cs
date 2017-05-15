using System;
using System.IO;
using UnityReader.Types;

namespace UnityReader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			DerPopoClassDatabase db = new DerPopoClassDatabase();
			using (FileStream fs = File.OpenRead("TestData/Types/unity-5.5.0f3.dat"))
			{
				db.Read(fs);
				db.Write();
			}

			UnityContext context = new LocalUnityContext("TestData");
			var file = context.LoadFile("level0");

			Console.WriteLine("Loading dependencies");
			foreach (var dependency in file.Dependencies)
			{
				Console.WriteLine($"Dependency: {dependency.AssetPath}");
				var loadedDependency = dependency.Load(context);
				FindScriptsInFile(loadedDependency);
			}
			Console.WriteLine();

			FindScriptsInFile(file);

			Console.ReadLine();
		}

		private static void FindScriptsInFile(AssetsFile file)
		{
			foreach (AssetFileInfo info in file.Assets)
			{
				var obj = info.GetAsset<AssetData>();
				if (obj is MonoScript)
				{
					var script = (MonoScript)obj;
					Console.WriteLine($"Script: {script.Name}");
					Console.WriteLine($"  Assembly:  {script.AssemblyName}");
					Console.WriteLine($"  Namespace: {script.Namespace}");
					Console.WriteLine($"  ClassName: {script.ClassName}");
					Console.WriteLine();
				}
			}
		}
	}
}