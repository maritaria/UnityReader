using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public sealed class Align : Instruction
	{
		public int BlockSize { get; set; }

		public void Read(JToken target, ReaderContext context)
		{
			context.Reader.Align(BlockSize);
		}

		public void Write(JToken input, WriterContext context)
		{
			context.Writer.Align(BlockSize);
		}

		public void PostDeserialization(TypeSet set)
		{
			if (BlockSize < 1)
			{
				throw new Exception("Blocksize must be larger than 0");
			}
		}

		#region IXmlSerializable

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			BlockSize = int.Parse(reader.GetAttribute("To"));
			reader.Skip();
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable
	}
}