using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityReader.Types
{
	public sealed class TypeRegistry
	{
		private Dictionary<int, Type> _types = new Dictionary<int, Type>();

		public TypeRegistry()
		{
			Assembly asm = Assembly.GetCallingAssembly();
			var datatypes = asm.DefinedTypes.Where(t => typeof(AssetData).IsAssignableFrom(t));

			foreach (IGrouping<int?, TypeInfo> types in datatypes.ToLookup(t => t.GetCustomAttribute<UnityTypeAttribute>()?.ID).Where(a => a.Key.HasValue))
			{
				_types[types.Key.Value] = types.First();
			}
		}

		public void Register<T>(int classID) where T : AssetData, new()
		{
			_types[classID] = typeof(T);
		}

		public T CreateInstance<T>(int classID) where T : AssetData
		{
			Type type;
			if (_types.TryGetValue(classID, out type))
			{
				return (T)Activator.CreateInstance(type);
			}
			throw new ArgumentException($"class '{classID}' not in registry");
		}
	}
}