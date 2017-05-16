using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace UnityReader.Definitions
{
	[DebuggerDisplay("Align #{BlockSize}")]
	public sealed class AlignNode : SerializationNode
	{
		[XmlAttribute]
		public int BlockSize { get; set; } = 4;

		public override void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			reader.Align(4);
		}
	}
}