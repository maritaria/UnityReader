using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityReader.Definitions;
using UnityReader.Types;

namespace UnityReader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			DerPopoClassDatabase db = new DerPopoClassDatabase();
			using (FileStream fs = File.OpenRead("TestData\\DerPopo\\unity-5.5.0f3.dat"))
			{
				db.Read(fs);
				db.Write();
			}

			UnityContext context = new LocalUnityContext("TestData\\Assets");

			using (FileStream fs = File.OpenRead("TestData\\typedefs.xml"))
			{
				XmlSerializer ser = new XmlSerializer(typeof(TypeDatabase));
				var myDatabase = (TypeDatabase)ser.Deserialize(fs);
				foreach (var type in myDatabase.Tables[0].Types)
				{
					context.TypeTable.AddTypeNode(type);
				}
			}
			var file = context.LoadFile("level0");
			FindScriptsInFile(file);
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
			foreach (AssetFileInfo info in file.Assets.Where(info => info.ClassID == AssetCodes.MonoScript))
			{
				var script = info.ParseAssetData<MonoScript>();
				if (script.AssemblyName.Contains("Assembly"))
				{
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