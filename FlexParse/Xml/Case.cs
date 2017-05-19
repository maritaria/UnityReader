using System;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace FlexParse.Xml
{
	public class Case : InstructionContainer
	{
		public long Value { get; set; } = long.MinValue;
		public ComparisonMode Comparison { get; set; } = ComparisonMode.Equal;

		public void Read(ReaderContext context)
		{
			foreach (var instr in Instructions)
			{
				instr.Read(context);
			}
		}

		public void Write(WriterContext context)
		{
			foreach (var instr in Instructions)
			{
				instr.Write(context);
			}
		}

		public void PostDeserialization(TypeSet set)
		{
			foreach (Instruction instr in Instructions)
			{
				instr.PostDeserialization(set);
			}
		}

		#region IXmlSerializable

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			string valueString = reader.GetAttribute("Value");
			if (valueString != null)
			{
				Value = long.Parse(valueString);
			}
			string comparison = reader.GetAttribute("Comparison");
			if (comparison != null)
			{
				Comparison = (ComparisonMode)Enum.Parse(typeof(ComparisonMode), comparison);
			}
		}

		#endregion IXmlSerializable

		public enum ComparisonMode
		{
			Equal = 0,

			Below = 1,
			Lower = 1,
			Smaller = 1,

			Above = 2,
			Higher = 2,
			Larger = 2,
		}
	}
}