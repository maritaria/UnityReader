using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using FlexParse.Scripting;
using Newtonsoft.Json.Linq;

namespace FlexParse.Xml
{
	[DebuggerDisplay("Array ")]
	public sealed class Array : Value
	{
		public Variable<int> Size { get; private set; } = new StaticValue<int>(-1);

		public bool IsDynamic => Size is DynamicArrayMarker;

		public void MakeVariableSize(Variable<int> size)
		{
			Size = size;
		}

		public void MakeDynamicSize()
		{
			Size = new DynamicArrayMarker();
		}

		public override void Read(JObject target, ReaderContext context)
		{
			int count;
			count = GetArraySize(target, context);
			JArray array = new JArray();
			for (int i = 0; i < count; i++)
			{
				array.Add(Type.Read(context));
			}
			target[FieldName] = array;
		}

		private int GetArraySize(JObject target, ReaderContext context)
		{
			int count;
			if (IsDynamic)
			{
				count = context.Reader.ReadInt32();
			}
			else
			{
				count = Size.Evaluate(target, context.Globals);
			}

			return count;
		}

		public override void Write(JObject item, WriterContext context)
		{
			JArray array = (JArray)item[FieldName];
			if (IsDynamic)
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
				if (VariableFactory.IsScriptedVariable(sizeString))
				{
					Size = VariableFactory.CreateScriptedVariable<int>(sizeString);
				}
				else
				{
					Size = new StaticValue<int>(int.Parse(sizeString));
				}
			}
			else
			{
				Size = new DynamicArrayMarker();
			}
			base.ReadXml(reader);
		}

		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			throw new NotImplementedException();
		}

		#endregion IXmlSerializable

		private class DynamicArrayMarker : Variable<int>
		{
			public int Evaluate(JObject localScope, JObject globalScope)
			{
				throw new NotImplementedException();
			}
		}
	}
}