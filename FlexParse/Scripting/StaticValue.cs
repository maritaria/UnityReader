using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public sealed class StaticValue<T> : Variable<T>
	{
		public T Value { get; set; }

		public StaticValue(T value)
		{
			Value = value;
		}

		public T Evaluate(ScopeStack scope)
		{
			return Value;
		}
	}
}
