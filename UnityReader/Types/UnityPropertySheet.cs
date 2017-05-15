using System;
using System.Collections.Generic;

namespace UnityReader.Types
{
	public sealed class UnityPropertySheet : AssetData
	{
		public IDictionary<string, UnityTexEnv> TexEnvs { get; } = new Dictionary<string, UnityTexEnv>();
		public IDictionary<string, float> Floats { get; } = new Dictionary<string, float>();
		public IDictionary<string, ColorFloatRgba> Colors { get; } = new Dictionary<string, ColorFloatRgba>();

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			TexEnvs.Clear();
			int envsCount = reader.ReadInt32();
			for (int i = 0; i < envsCount; i++)
			{
				string name = reader.ReadStringFixed(reader.ReadInt32());
				var env = reader.Read<UnityTexEnv>(owner);
				TexEnvs.Add(name, env);
			}
			Floats.Clear();
			int floatCount = reader.ReadInt32();
			for (int i = 0; i < floatCount; i++)
			{
				string name = reader.ReadStringFixed(reader.ReadInt32());
				var env = reader.ReadFloat();
				Floats.Add(name, env);
			}
			Colors.Clear();
			int colorCount = reader.ReadInt32();
			for (int i = 0; i < colorCount; i++)
			{
				string name = reader.ReadStringFixed(reader.ReadInt32());
				var env = reader.Read<ColorFloatRgba>(owner);
				Colors.Add(name, env);
			}
		}
	}
}