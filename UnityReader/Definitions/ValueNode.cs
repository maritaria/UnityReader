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


	[DebuggerDisplay("Value {TypeName,nq} {FieldName,nq}")]
	public class ValueNode : SerializationNode
	{
		[XmlAttribute("Type")]
		public string TypeName { get; set; }

		[XmlAttribute("Name")]
		public string FieldName { get; set; }

		public override void Read(UnityReader reader, UnityContext context, JObject currentObject)
		{
			object value = ReadValue(reader);
			if (value != null)
			{
				currentObject[FieldName] = new JValue(value);
			}
			else
			{
				JObject templated = new JObject();
				context.TypeTable[TypeName].Read(reader, context, templated);
				currentObject[FieldName] = templated;
			}
		}

		private object ReadValue(UnityReader reader)
		{
			switch (TypeName.ToLower())
			{
				case "int16": return reader.ReadInt16();
				case "uint16": return reader.ReadUInt16();
				case "int32": return reader.ReadInt32();
				case "uint32": return reader.ReadUInt32();
				case "int64": return reader.ReadInt64();
				case "uint64": return reader.ReadUInt64();
				case "float": return reader.ReadFloat();
				case "boolean": return reader.ReadBool();
				case "byte": return reader.ReadByte();
				case "string":
					int length = reader.ReadInt32();
					return reader.ReadStringFixed(length);
			}
			return null;
		}
	}
}
