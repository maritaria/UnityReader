using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public interface Variable<T>
	{
		T Evaluate(JObject localScope, JObject globalScope);
	}
}