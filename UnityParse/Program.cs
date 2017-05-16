using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FlexParse;
using Newtonsoft.Json.Linq;

namespace UnityParse
{
	internal static class Program
	{
		private static void Main()
		{
			XmlSerializer ser = new XmlSerializer(typeof(TypeSet));
			TypeSet set;
			using (FileStream fs = File.OpenRead("AssetsFile.xml"))
			{
				set = (TypeSet)ser.Deserialize(fs);
			}
			var headerType = set["AssetsFile.Header"];
			var metadataType = set["AssetsFile.Metadata"];

			using (FileStream fs = File.OpenRead("level0"))
			using (FlexReader reader = new FlexReader(fs) { IsLittleEndian = false })
			using (FileStream copy = File.OpenWrite("header"))
			using (FlexWriter writer = new FlexWriter(copy) { IsLittleEndian = false })
			{
				var readerContext = new ReaderContext(set, reader);
				var writerContext = new WriterContext(set, writer, readerContext.Globals);

				var header = (JObject)headerType.Read(readerContext);
				headerType.Write(header, writerContext);

				reader.IsLittleEndian = true;
				writer.IsLittleEndian = true;
				readerContext.Globals["Format"] = header["Format"].Value<long>();

				var metadata = (JObject)metadataType.Read(readerContext);
				metadataType.Write(metadata, writerContext);
			}

			Console.WriteLine("Done");
			Console.ReadLine();
		}
	}
}