using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public sealed class LocalVariable<T> : Variable<T>
	{
		public string Name { get; set; }

		public T Evaluate(JObject localScope, JObject globalScope)
		{
			if (Name == "AssetsInfo_Count")
			{

			}
			return localScope[Name].Value<T>();
		}
	}
}