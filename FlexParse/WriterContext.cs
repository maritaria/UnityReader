using System;
using System.Collections.Generic;
using System.Linq;
using FlexParse.Scripting;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public sealed class WriterContext
	{
		public ScopeStack Scope { get; }
		public TypeSet Types { get; }
		public FlexWriter Writer { get; }

		public WriterContext(TypeSet set, FlexWriter writer)
			: this(set, writer, new ScopeStack())
		{
		}

		public WriterContext(TypeSet set, FlexWriter writer, ScopeStack globals)
		{
			if (set == null) throw new ArgumentNullException(nameof(set));
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			if (globals == null) throw new ArgumentNullException(nameof(globals));
			Types = set;
			Writer = writer;
			Scope = globals;
		}
	}
}