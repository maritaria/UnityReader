using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UnityParse.BakedFiles
{
	[DataContract]
	[DebuggerDisplay("ClassID = {ClassID}")]
	public class AssetType
	{
		[DataMember]
		public AssetCode ClassID { get; private set; }

		[DataMember]
		public short ScriptIndex { get; private set; }

		[DataMember]
		public Guid ScriptHash { get; private set; }

		[DataMember]
		public Guid PropertiesHash { get; private set; }
	}
}