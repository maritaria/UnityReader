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
		public const string IndexVariableName = "Array.Index";
		public const string SizeVariableName = "Array.Size";

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

		public override void Read(ReaderContext context)
		{
			int count = GetArraySize(context);
			JArray array = new JArray();
			using (context.Scope.CreateAnonymousScope())
			{
				context.Scope[SizeVariableName] = new JValue(count);
				for (int i = 0; i < count; i++)
				{
					context.Scope[IndexVariableName] = new JValue(i);
					array.Add(Type.Read(context));
				}
			}
			context.Scope.ActiveObject[FieldName] = array;
		}

		private int GetArraySize(ReaderContext context)
		{
			int count;
			if (IsDynamic)
			{
				count = context.Reader.ReadInt32();
			}
			else
			{
				count = Size.Evaluate(context.Scope);
			}
			return count;
		}

		public override void Write(WriterContext context)
		{
			JArray array = (JArray)context.Scope[FieldName];
			using (context.Scope.CreateAnonymousScope())
			{
				context.Scope[SizeVariableName] = array.Count;
				if (IsDynamic)
				{
					context.Writer.Write(array.Count);
				}
				for (int i = 0; i < array.Count; i++)
				{
					context.Scope[IndexVariableName] = i;
					var current = array[i];
					Type.Write(current, context);
				}
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
			public int Evaluate(ScopeStack scope)
			{
				throw new NotImplementedException();
			}
		}
	}
}