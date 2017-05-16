using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace UnityReader.Definitions
{


	[DebuggerDisplay("Skip #{NumBytes}")]
	public sealed class SkipNode : SerializationNode
	{
		[XmlAttribute]
		public int NumBytes { get; set; } = 1;

		public override void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			reader.Position += NumBytes;
		}
	}
}
