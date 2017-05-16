using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public sealed class GlobalVariable<T> : Variable<T>
	{
		public string Name { get; set; }

		public T Evaluate(JObject localScope, JObject globalScope)
		{
			return globalScope[Name].Value<T>();
		}
	}
}