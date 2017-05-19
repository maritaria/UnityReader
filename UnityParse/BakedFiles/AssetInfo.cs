using System.Runtime.Serialization;

namespace UnityParse.BakedFiles
{
	[DataContract]
	public sealed class AssetInfo
	{
		[DataMember]
		public long Index { get; set; }

		[DataMember]
		public int DataOffset { get; set; }

		[DataMember]
		public int DataLength { get; set; }

		[DataMember]
		public AssetCode ClassID { get; set; }

		[DataMember]
		public AssetCode InheritedUnityClass { get; set; }

		[DataMember]
		public int TreeIndex { get; set; }

		[DataMember]
		public short ScriptIndex { get; set; }

		[DataMember]
		public byte Unknown { get; set; }
	}
}