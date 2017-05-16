using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public sealed class ReaderContext
	{
		public JObject Globals { get; }
		public TypeSet Types { get; }
		public FlexReader Reader { get; }

		public ReaderContext(TypeSet set, FlexReader reader)
			: this(set, reader, new JObject())
		{
		}

		public ReaderContext(TypeSet set, FlexReader reader, JObject globals)
		{
			if (set == null) throw new ArgumentNullException(nameof(set));
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (globals == null) throw new ArgumentNullException(nameof(globals));
			Types = set;
			Reader = reader;
			Globals = globals;
		}
	}
}