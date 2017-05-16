using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FlexParse.Xml
{
	public abstract class InstructionContainer : IXmlSerializable
	{
		public ICollection<Instruction> Instructions { get; } = new List<Instruction>();

		#region IXmlSerializable

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			ReadXmlAttributes(reader);
			bool hasElements = !reader.IsEmptyElement;
			reader.ReadStartElement();
			if (hasElements)
			{
				while (reader.IsStartElement())
				{
					reader.MoveToContent();
					ReadXmlChild(reader);
				}
				reader.ReadEndElement();
			}
		}

		protected virtual void ReadXmlAttributes(XmlReader reader)
		{
		}

		protected virtual void ReadXmlChild(XmlReader reader)
		{
			var item = CreateInstructionInstance(reader);
			if (item != null)
			{
				item.ReadXml(reader);
				Instructions.Add(item);
			}
			else
			{
				reader.Skip();
			}
		}

		private Instruction CreateInstructionInstance(XmlReader reader)
		{
			switch (reader.Name)
			{
				case "Value": return new Value();

				case "Align": return new Align();

				case "Array": return new Array();

				case "Switch": return new Switch();

				default: return null;
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable
	}
}