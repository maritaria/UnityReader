using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public interface Instruction : IXmlSerializable
	{
		void Read(JToken target, ReaderContext context);

		void Write(JToken item, WriterContext context);

		void PostDeserialization(TypeSet set);
	}
}