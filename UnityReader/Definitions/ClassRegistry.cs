using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityReader.Types;

namespace UnityReader.Definitions
{
	public sealed class ClassRegistry
	{
		private Dictionary<int, Type> _types = new Dictionary<int, Type>();

		public static ClassRegistry FromReflection()
		{
			var reg = new ClassRegistry();
			Assembly asm = Assembly.GetCallingAssembly();
			var datatypes = asm.DefinedTypes.Where(t => typeof(AssetObject).IsAssignableFrom(t));
			foreach (IGrouping<int?, TypeInfo> types in datatypes.ToLookup(t => t.GetCustomAttribute<UnityTypeAttribute>()?.AssetType).Where(a => a.Key.HasValue))
			{
				reg._types[types.Key.Value] = types.First();
			}
			return reg;
		}

		public void Register<T>(int classID) where T : new()
		{
			_types[classID] = typeof(T);
		}

		public T CreateInstance<T>(int classID)
		{
			Type type;
			if (_types.TryGetValue(classID, out type))
			{
				return (T)Activator.CreateInstance(type);
			}
			throw new ArgumentException($"Class '{classID}' not in registry");
		}
	}
}