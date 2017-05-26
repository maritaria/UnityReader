using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FlexParse;
using Newtonsoft.Json.Linq;
using UnityParse.BakedFiles;
using UnityParse.Types;

namespace UnityParse
{
	internal static class Program
	{
		private static void Main()
		{
			Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
			TypeSet types = ReadTypeSet();

			List<string> files = new List<string>
			{
				"level0",
				"globalgamemanagers",
				"globalgamemanagers.assets",
				//"resources.assets",
				//"sharedassets0.assets",
			};
			foreach (var filename in files)
			{
				Console.Write($"Modifying {filename} ");
				int count = RenameNamelessNamespace(types, filename);
				Console.WriteLine($" ~{count}");
			}

			Console.WriteLine("Done");
			//Console.ReadLine();
		}

		private static int RenameNamelessNamespace(TypeSet types, string filename)
		{
			int count = 0;
			using (FileStream fs = File.OpenRead(filename))
			using (FileStream copy = File.Open(@"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\" + filename, FileMode.Create))
			{
				FlexReader reader = new FlexReader(fs) { IsLittleEndian = false };
				FlexWriter writer = new FlexWriter(copy) { IsLittleEndian = false };
				var readerContext = new ReaderContext(types, reader);
				var writerContext = new WriterContext(types, writer, readerContext.Scope);

				var header = Header.FromStream(fs, types);
				var meta = Metadata.FromStream(fs, types, header);
				var collection = AssetCollection.FromStream(fs, types, header, meta);

				var monoScriptDef = types["MonoScript"];
				var monoManagerDef = types["MonoManager"];
				var assemblies = new HashSet<string>();
				foreach (Asset asset in collection)
				{
					if (asset.ClassID == AssetCode.MonoScript)
					{
						using (var ms = new MemoryStream(asset.Data))
						{
							var scriptReader = new FlexReader(ms) { IsLittleEndian = !header.IsBigEndian };
							var scriptReaderContext = new ReaderContext(types, scriptReader);
							scriptReaderContext.Scope.GlobalFrame["Format"] = header.Format;
							var scriptJson = monoScriptDef.Read(scriptReaderContext);
							var script = scriptJson.ToObject<MonoScript>();
							if (script.AssemblyName == "Assembly-CSharp.dll" && script.Namespace.Length == 0)
							{
								if (script.ClassName.StartsWith("uScript"))
								{
									script.Namespace = "TerraTech.uScript";
								}
								else if (script.ClassName.StartsWith("Man"))
								{
									script.Namespace = "TerraTech.Managers";
								}
								else if (script.ClassName.StartsWith("Mission"))
								{
									script.Namespace = "TerraTech.uScript.Missions";
								}

								using (var newMS = new MemoryStream())
								{
									var scriptWriter = new FlexWriter(newMS) { IsLittleEndian = !header.IsBigEndian };
									var scriptWriterContext = new WriterContext(types, scriptWriter);
									scriptWriterContext.Scope.GlobalFrame["Format"] = header.Format;
									var newJson = JObject.FromObject(script);
									monoScriptDef.Write(newJson, scriptWriterContext);
									asset.Data = newMS.ToArray();
								}
								count++;
							}
						}
					}
					else if (asset.ClassID == AssetCode.MonoManager)
					{
						using (var ms = new MemoryStream(asset.Data))
						{
							var scriptReader = new FlexReader(ms) { IsLittleEndian = !header.IsBigEndian };
							var scriptReaderContext = new ReaderContext(types, scriptReader);
							scriptReaderContext.Scope.GlobalFrame["Format"] = header.Format;
							var scriptJson = monoManagerDef.Read(scriptReaderContext);
							var manager = scriptJson.ToObject<MonoManager>();

						}
					}
				}

				collection.WriteTo(copy, types, header, meta);
			}
			return count;
		}

		private static TypeSet ReadTypeSet()
		{
			XmlSerializer ser = new XmlSerializer(typeof(TypeSet));
			TypeSet set;
			using (FileStream fs = File.OpenRead("Types.xml"))
			{
				set = (TypeSet)ser.Deserialize(fs);
			}

			return set;
		}
	}
}