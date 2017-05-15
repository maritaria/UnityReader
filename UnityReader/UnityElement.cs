namespace UnityReader
{

	public interface UnityElement
	{
		void Read(UnityBinaryReader reader, int version);
	}
}