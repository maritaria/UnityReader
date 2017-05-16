using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public interface TypeDef
	{
		string Name { get; }

		JToken Read(ReaderContext context);

		void Write(JToken value, WriterContext context);
	}
}