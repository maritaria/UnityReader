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


	[DebuggerDisplay("Array {TemplateName != null ? TemplateName : \"+\"+Nodes.Count,nq}")]
	public class ArrayNode : ObjectNode
	{
		[XmlAttribute("Type")]
		public string TemplateTypeName { get; set; }

		public bool ShouldSerializeTemplateName()
		{
			return TemplateTypeName != null;
		}

		public override void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			JArray current = new JArray();
			currentObject[FieldName] = current;

			int count = reader.ReadInt32();

			if (TemplateTypeName != null)
			{
				var template = context.TypeTable[TemplateTypeName];
				for (int i = 0; i < count; i++)
				{
					JObject instance = new JObject();
					current.Add(instance);
					template.Read(reader, context, instance);
				}
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					JObject instance = new JObject();
					current.Add(instance);
					ReadChildren(reader, context, instance);
				}
			}
		}
	}

}
