using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace UnityReader.Definitions
{
	[DebuggerDisplay("{TypeName,nq} (#{AssetCode})")]
	public class UnityTypeNode
	{
		[XmlAttribute]
		public int AssetCode { get; set; } = -1;

		[XmlAttribute("Type")]
		public string TypeName { get; set; }

		[XmlElement("Value", Type = typeof(ValueNode))]
		[XmlElement("Object", Type = typeof(ObjectNode))]
		[XmlElement("Array", Type = typeof(ArrayNode))]
		[XmlElement("Align", Type = typeof(AlignNode))]
		[XmlElement("Skip", Type = typeof(SkipNode))]
		public List<SerializationNode> Nodes { get; } = new List<SerializationNode>();

		public void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			foreach (SerializationNode node in Nodes)
			{
				node.Read(reader, context, currentObject);
			}
		}

		public bool ShouldSerializeAssetCode()
		{
			return (AssetCode >= 0);
		}
	}

}