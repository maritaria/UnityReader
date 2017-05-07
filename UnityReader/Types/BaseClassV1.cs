using System;

namespace UnityReader.Types
{
	public class BaseClassV1 : BaseClass<ClassTableV2, TypeTreeV1>
	{
		protected override void ReadCore(UnityBinaryReader reader, SerializedFileHeader header, ClassTableV2 table)
		{
			throw new NotImplementedException();
		}
	}
}