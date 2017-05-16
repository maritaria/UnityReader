using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FlexParse.Xml;

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
			ReadXmlCore(reader);
			foreach (UserType type in _types.Values.OfType<UserType>())
			{
				foreach (var instr in type.Instructions)
				{
					instr.PostDeserialization(this);
				}
			}
		}

		private void ReadXmlCore(XmlReader reader)
		{
			reader.MoveToContent();
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					switch (reader.Name)
					{
						case "Include":
							using (var fs = File.Open(reader.GetAttribute("Path"), FileMode.Open, FileAccess.Read, FileShare.None))
							using (var innerReader = XmlReader.Create(fs))
							{
								ReadXmlCore(innerReader);
							}
							if (reader.IsEmptyElement)
							{
								reader.ReadStartElement();
							}
							else
							{
								reader.Skip();
							}
							break;

						case "Type":
							var type = new UserType();
							type.ReadXml(reader);
							Add(type);
							break;

						default:
							throw new Exception($"Cannot process element '{reader.Name}'");
					}
				}
				reader.ReadEndElement();
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