using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json.Linq;

namespace FlexParse
{
	public sealed class Switch : Instruction
	{
		public static readonly char GlobalVariablePrefix = '$';

		public string Source { get; set; }
		public ICollection<Case> Cases { get; } = new List<Case>();
		public Case Default { get; set; }
		public SwitchMode Mode { get; set; } = SwitchMode.One;

		public void Read(JToken target, ReaderContext context)
		{
			long value = ResolveValue(target, context.Globals);
			var accepting = GetAcceptingCases(value);
			foreach (Case c in accepting)
			{
				c.Read(target, context);
				if (Mode == SwitchMode.One)
				{
					break;
				}
			}
		}

		public void Write(JToken item, WriterContext context)
		{
			long value = ResolveValue(item, context.Globals);
			var accepting = GetAcceptingCases(value);
			foreach (Case c in accepting)
			{
				c.Write(item, context);
				if (Mode == SwitchMode.One)
				{
					break;
				}
			}
		}

		private long ResolveValue(JToken target, IDictionary<string, long> globals)
		{
			if (Source[0] == GlobalVariablePrefix)
			{
				return globals[Source.Substring(1)];
			}
			else
			{
				return target[Source].Value<long>();
			}
		}

		private IEnumerable<Case> GetAcceptingCases(long value)
		{
			bool passed = false;
			foreach (Case c in Cases)
			{
				switch (c.Comparison)
				{
					case Case.ComparisonMode.Lower:
						if (value < c.Value)
						{
							passed = true;
							yield return c;
						}
						break;

					case Case.ComparisonMode.Equal:
						if (value == c.Value)
						{
							passed = true;
							yield return c;
						}
						break;

					case Case.ComparisonMode.Above:
						if (value > c.Value)
						{
							passed = true;
							yield return c;
						}
						break;
				}
			}
			if (!passed)
			{
				yield return Default;
			}
		}

		public void PostDeserialization(TypeSet set)
		{
			Default?.PostDeserialization(set);
			foreach (Case c in Cases)
			{
				c.PostDeserialization(set);
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
			Source = reader.GetAttribute("Source");

			string modeString = reader.GetAttribute("Mode");
			if (modeString != null)
			{
				Mode = (SwitchMode)Enum.Parse(typeof(SwitchMode), modeString);
			}

			bool hasElements = !reader.IsEmptyElement;
			reader.ReadStartElement();
			if (hasElements)
			{
				while (reader.IsStartElement())
				{
					reader.MoveToContent();
					if (reader.Name == "Default" && Default == null)
					{
						Default = ReadCase(reader);
					}
					else
					{
						var c = ReadCase(reader);
						Cases.Add(c);
					}
				}
				reader.ReadEndElement();
			}
		}

		private Case ReadCase(XmlReader reader)
		{
			Case result = new Case();
			result.ReadXml(reader);
			return result;
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable

		public enum SwitchMode
		{
			One,
			Many,
		}
	}
}