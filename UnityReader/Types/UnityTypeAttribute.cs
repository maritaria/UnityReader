using System;

namespace UnityReader.Types
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class UnityTypeAttribute : Attribute
	{
		public AssetCodes AssetType { get; set; }

		public UnityTypeAttribute(AssetCodes classID)
		{
			AssetType = classID;
		}

	}
}