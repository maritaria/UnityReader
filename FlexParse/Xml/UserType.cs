using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace FlexParse.Xml
{
	[DebuggerDisplay("{Name}")]
	public sealed class UserType : InstructionContainer, TypeDef
	{
		public string Name { get; set; }
		public string Proxy { get; set; }

		#region TypeDef

		public JToken Read(ReaderContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			JObject result = new JObject();
			using (context.Scope.CreateActiveObjectScope(result))
			{
				foreach (var instruction in Instructions)
				{
					instruction.Read(context);
				}
			}
			if (Proxy != null)
			{
				return result[Proxy];
			}
			else
			{
				return result;
			}
		}

		public void Write(JToken value, WriterContext context)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (context == null) throw new ArgumentNullException(nameof(context));

			JObject activeScope;
			if (Proxy == null)
			{
				activeScope = (JObject)value;
			}
			else
			{
				activeScope = new JObject();
				activeScope[Proxy] = value;
			}

			using (context.Scope.CreateActiveObjectScope(activeScope))
			{
				foreach (var instruction in Instructions)
				{
					instruction.Write(context);
				}
			}
		}

		#endregion TypeDef

		#region IXmlSerializable

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			Name = reader.GetAttribute("Name");
			Proxy = reader.GetAttribute("Proxy");
		}

		#endregion IXmlSerializable
	}
}