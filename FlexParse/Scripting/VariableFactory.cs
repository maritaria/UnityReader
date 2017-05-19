using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlexParse.Scripting
{
	public static class VariableFactory
	{
		public static readonly char ScriptPrefix = '$';

		public static Regex VariableParser = new Regex(
			@"^\$\{(?<IsGlobal>\!)?(?<Name>[\w._]*)\}$",
			RegexOptions.Compiled);

		public static bool IsScriptedVariable(string script)
		{
			return VariableParser.IsMatch(script);
		}

		public static Variable<T> CreateScriptedVariable<T>(string script)
		{
			var matches = VariableParser.Match(script);
			string variableName = matches.Groups["Name"].Value;
			if (matches.Groups["IsGlobal"].Success)
			{
				return new GlobalVariable<T> { Name = variableName };
			}
			else
			{
				return new LocalVariable<T> { Name = variableName };
			}
		}
	}
}