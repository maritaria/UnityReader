using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FlexParse;
using Newtonsoft.Json.Linq;
using UnityParse.BakedFiles;

namespace UnityParse
{
	internal static class Program
	{
		private static void Main()
		{
			Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));

			TypeSet types = ReadTypeSet();

			var headerType = types["AssetsFile.Header"];
			var metadataType = types["AssetsFile.Metadata"];

			using (FileStream fs = File.OpenRead("level0"))
			using (FileStream copy = File.OpenWrite("header"))
			{
				FlexReader reader = new FlexReader(fs) { IsLittleEndian = false };
				FlexWriter writer = new FlexWriter(copy) { IsLittleEndian = false };
				var readerContext = new ReaderContext(types, reader);
				var writerContext = new WriterContext(types, writer, readerContext.Scope);

				Header header = Header.FromStream(fs, types);
				var collection = AssetCollection.FromStream(fs, header, types);

				reader.IsLittleEndian = true;
				writer.IsLittleEndian = true;
				readerContext.Scope.GlobalFrame["Format"] = header.Format;

				var metadata = (JObject)metadataType.Read(readerContext);
				metadataType.Write(metadata, writerContext);
			}

			Console.WriteLine("Done");
			Console.ReadLine();
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