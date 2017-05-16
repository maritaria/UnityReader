using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using FlexParse.Scripting;
using Newtonsoft.Json.Linq;

namespace FlexParse.Xml
{
	public sealed class Switch : Instruction
	{
		public Variable<long> Source { get; set; }
		public ICollection<Case> Cases { get; } = new List<Case>();
		public Case Default { get; set; }
		public SwitchMode Mode { get; set; } = SwitchMode.One;

		public void Read(JObject localContext, ReaderContext context)
		{
			long value = Source.Evaluate(localContext, context.Globals);
			var accepting = GetAcceptingCases(value);
			foreach (Case c in accepting)
			{
				c.Read(localContext, context);
				if (Mode == SwitchMode.One)
				{
					break;
				}
			}
		}

		public void Write(JObject localContext, WriterContext context)
		{
			long value = Source.Evaluate(localContext, context.Globals);
			var accepting = GetAcceptingCases(value);
			foreach (Case c in accepting)
			{
				c.Write(localContext, context);
				if (Mode == SwitchMode.One)
				{
					break;
				}
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
			if (!passed && Default != null)
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
			Source = VariableFactory.CreateScriptedVariable<long>(reader.GetAttribute("Source"));

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