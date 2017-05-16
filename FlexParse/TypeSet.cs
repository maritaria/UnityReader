using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FlexParse
{
	public sealed class TypeSet : IXmlSerializable, IEnumerable<TypeDef>
	{
		private Dictionary<string, TypeDef> _types = new Dictionary<string, TypeDef>();

		public TypeSet()
		{
		}

		public TypeDef this[string typeName]
		{
			get { return _types[typeName]; }
			set { _types[typeName] = value; }
		}

		public void Add(TypeDef type)
		{
			_types.Add(type.Name, type);
		}

		#region IXmlSerializable

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			DefaultTypes.PopulateTypeSet(this);
			reader.MoveToContent();
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					switch (reader.Name)
					{
						case "Type":
							var type = new CustomType();
							type.ReadXml(reader);
							Add(type);
							break;

						default:
							reader.Skip();
							break;
					}
				}
				reader.ReadEndElement();
				foreach (CustomType type in _types.Values.OfType<CustomType>())
				{
					foreach (var instr in type.Instructions)
					{
						instr.PostDeserialization(this);
					}
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable

		#region IEnumerable<TypeDef>

		public IEnumerator<TypeDef> GetEnumerator()
		{
			return _types.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _types.Values.GetEnumerator();
		}

		#endregion IEnumerable<TypeDef>
	}
}