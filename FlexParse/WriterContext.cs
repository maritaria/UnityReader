using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public sealed class WriterContext
	{
		public JObject Globals { get; }
		public TypeSet Types { get; }
		public FlexWriter Writer { get; }

		public WriterContext(TypeSet set, FlexWriter reader)
			: this(set, reader, new JObject())
		{
		}

		public WriterContext(TypeSet set, FlexWriter reader, JObject globals)
		{
			if (set == null) throw new ArgumentNullException(nameof(set));
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (globals == null) throw new ArgumentNullException(nameof(globals));
			Types = set;
			Writer = reader;
			Globals = globals;
		}
	}
}