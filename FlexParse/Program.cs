using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FlexParse;

namespace FlexParse
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			XmlSerializer ser = new XmlSerializer(typeof(TypeSet));
			using (FileStream fs = File.OpenRead("ExampleTypeSet.xml"))
			{
				TypeSet set = (TypeSet)ser.Deserialize(fs);
			}
		}
	}
}