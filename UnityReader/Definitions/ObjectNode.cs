using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace UnityReader.Definitions
{
	[DebuggerDisplay("Object {FieldName,nq} +{Nodes.Count}")]
	public class ObjectNode : SerializationNode
	{
		[XmlAttribute("Name")]
		public string FieldName { get; set; }

		[XmlElement("Value", Type = typeof(ValueNode))]
		[XmlElement("Object", Type = typeof(ObjectNode))]
		[XmlElement("Array", Type = typeof(ArrayNode))]
		[XmlElement("Align", Type = typeof(AlignNode))]
		[XmlElement("Skip", Type = typeof(SkipNode))]
		public List<SerializationNode> Nodes { get; } = new List<SerializationNode>();

		public override void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			JObject current = new JObject();
			//Add early to faults show progress
			currentObject[FieldName] = current;
			ReadChildren(reader, context, currentObject);
		}

		protected void ReadChildren(UnityReader reader, UnityContext context, JObject currentObject)
		{
			foreach (SerializationNode node in Nodes)
			{
				node.Read(reader, context, currentObject);
			}
		}
	}
}