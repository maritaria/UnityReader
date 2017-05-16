using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnityReader.Definitions
{
	public sealed class TypeDatabase
	{
		[XmlElement("TypeTable")]
		public List<TypeTable> Tables { get; } = new List<TypeTable>();

		public TypeTable GetSupportedUnityTypes(string unityVersion)
		{
			return Tables.FirstOrDefault(t => t.CheckSupport(unityVersion));
		}
	}
}
