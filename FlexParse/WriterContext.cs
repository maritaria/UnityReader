using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexParse
{
	public sealed class WriterContext
	{
		public IDictionary<string, long> Globals { get; }
		public TypeSet Types { get; }
		public FlexWriter Writer { get; }

		public WriterContext(TypeSet set, FlexWriter reader)
			: this(set, reader, new Dictionary<string, long>())
		{
		}

		public WriterContext(TypeSet set, FlexWriter reader, IDictionary<string, long> globals)
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