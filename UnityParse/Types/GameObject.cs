using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UnityParse.Types
{
	[DataContract]
	public sealed class GameObject : Object
	{
		[DataMember]
		public ICollection<Component> Components { get; } = new List<Component>();

		[DataMember]
		public uint Layer { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public ushort Tag { get; set; }

		[DataMember]
		public bool IsActive { get; set; }
	}
}