using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	[DebuggerDisplay("Array ")]
	public sealed class Array : Value
	{
		public int Size { get; set; } = -1;

		public bool IsFixed
		{
			get { return Size != -1; }
			set { Size = -1; }
		}

		public override void Read(JToken target, ReaderContext context)
		{
			int count = IsFixed ? Size : context.Reader.ReadInt32();
			JArray array = new JArray();
			for (int i = 0; i < count; i++)
			{
				array.Add(Type.Read(context));
			}
			target[FieldName] = array;
		}

		public override void Write(JToken item, WriterContext context)
		{
			JArray array = (JArray)item[FieldName];
			if (!IsFixed)
			{
				context.Writer.Write(array.Count);
			}
			foreach (JToken element in array)
			{
				Type.Write(element, context);
			}
		}

		#region IXmlSerializable

		public override void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			string sizeString = reader.GetAttribute("Size");
			if (sizeString != null)
			{
				Size = int.Parse(sizeString);
			}
			base.ReadXml(reader);
		}

		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable
	}
}