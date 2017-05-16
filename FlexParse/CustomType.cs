using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	[DebuggerDisplay("{Name}")]
	public sealed class CustomType : InstructionContainer, TypeDef
	{
		public string Name { get; set; }

		public JObject Read(ReaderContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			JObject obj = new JObject();
			foreach (var instruction in Instructions)
			{
				instruction.Read(obj, context);
			}
			return obj;
		}

		public void Write(JObject value, WriterContext context)
		{
			foreach (var instruction in Instructions)
			{
				instruction.Write(value, context);
			}
		}


		#region TypeDef

		JToken TypeDef.Read(ReaderContext context)
		{
			return Read(context);
		}

		void TypeDef.Write(JToken value, WriterContext context)
		{
			Write((JObject)value, context);
		}

		#endregion TypeDef

		#region IXmlSerializable

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			Name = reader.GetAttribute("Name");
		}

		#endregion IXmlSerializable
	}
}