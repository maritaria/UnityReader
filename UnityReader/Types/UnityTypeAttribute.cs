using System;

namespace UnityReader.Types
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class UnityTypeAttribute : Attribute
	{
		public int ID { get; set; }

		public UnityTypeAttribute(int classID)
		{
			ID = classID;
		}

	}
}