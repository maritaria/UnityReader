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
			using (FileStream fs = File.OpenRead("Unity.xml"))
			{
				set = (TypeSet)ser.Deserialize(fs);
			}
			var header = set["Unity.Header"];
			using (FileStream fs = File.OpenRead("level0"))
			using (FlexReader reader = new FlexReader(fs) { IsLittleEndian = false })
			using (FileStream copy = File.Open("header", FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			using (FlexWriter writer = new FlexWriter(copy) { IsLittleEndian = false })
			{
				var context = new ReaderContext(set, reader);
				var writeContext = new WriterContext(set, writer);

				var obj = (JObject)header.Read(context);
				header.Write(obj, writeContext);
			}

			Console.WriteLine("Done");
			Console.ReadLine();
		}
	}
}