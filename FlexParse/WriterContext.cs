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

		public WriterContext(TypeSet set, FlexWriter reader)
			: this(set, reader, new ScopeStack())
		{
		}

		public WriterContext(TypeSet set, FlexWriter reader, ScopeStack globals)
		{
			if (set == null) throw new ArgumentNullException(nameof(set));
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (globals == null) throw new ArgumentNullException(nameof(globals));
			Types = set;
			Writer = reader;
			Scope = globals;
		}
	}
}