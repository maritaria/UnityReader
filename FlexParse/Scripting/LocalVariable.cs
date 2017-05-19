using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public sealed class LocalVariable<T> : Variable<T>
	{
		public string Name { get; set; }

		public T Evaluate(ScopeStack scope)
		{
			if (Name == "Array.Index")
			{
			}
			return scope[Name].Value<T>();
		}
	}
}