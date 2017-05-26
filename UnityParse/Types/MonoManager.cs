using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UnityParse.Types
{
	[DataContract]
	public class MonoManager
	{
		[DataMember]
		public ICollection<AssetReference<MonoScript>> Scripts { get; private set; }
		[DataMember]
		public ICollection<string> AssemblyNames { get; private set; }
	}
}