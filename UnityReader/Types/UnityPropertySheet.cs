using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public sealed class UnityPropertySheet
	{
		public IDictionary<string, UnityTexEnv> TexEnvs { get; } = new Dictionary<string, UnityTexEnv>();
		public IDictionary<string, float> Floats { get; } = new Dictionary<string, float>();
		public IDictionary<string, ColorFloatRgba> Colors { get; } = new Dictionary<string, ColorFloatRgba>();
	}
}