using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UnityParse.Types
{
	[DataContract]
	public class MonoScript
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ExecutionOrder { get; set; }

		[DataMember]
		public Guid PropertiesHash { get; set; }

		[DataMember]
		public string ClassName { get; set; }

		[DataMember]
		public string Namespace { get; set; }

		[DataMember]
		public string AssemblyName { get; set; }

		[DataMember]
		public bool IsEditorScript { get; set; }
	}
}