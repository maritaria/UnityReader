using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public interface Instruction : IXmlSerializable
	{
		void Read(JObject localContext, ReaderContext context);

		void Write(JObject localContext, WriterContext context);

		void PostDeserialization(TypeSet set);
	}
}