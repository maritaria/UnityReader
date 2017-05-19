using System;
using System.Runtime.Serialization;

namespace UnityParse.BakedFiles
{
	[DataContract]
	public sealed class Dependency
	{
		[DataMember]
		public string BufferedPath { get; set; }

		[DataMember]
		public Guid Guid { get; set; }

		[DataMember]
		public int Type { get; set; }

		[DataMember]
		public string AssetPath { get; set; }
	}
}