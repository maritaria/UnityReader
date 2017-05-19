using System.Diagnostics;
using System.Runtime.Serialization;

namespace UnityParse.BakedFiles
{
	[DataContract]
	[DebuggerDisplay("File = {FileID}, PathID = {PathID}")]
	public sealed class AssetReference
	{
		[DataMember]
		public int FileID { get; set; }

		[DataMember]
		public long PathID { get; set; }
	}
}