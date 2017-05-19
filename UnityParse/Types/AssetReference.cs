using System.Runtime.Serialization;

namespace UnityParse.Types
{
	[DataContract]
	public class AssetReference<T>
	{
		[DataMember]
		public int FileID { get; set; }

		[DataMember]
		public long PathID { get; set; }
	}
}