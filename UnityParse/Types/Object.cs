using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UnityParse.Types
{
	[DataContract]
	public class Object
	{
		[DataMember]
		public int FileID { get; private set; }

		[DataMember]
		public int PathID { get; private set; }
	}
}