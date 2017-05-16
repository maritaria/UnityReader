namespace UnityReader
{

	public interface UnityElement
	{
		void Read(UnityReader reader, int version);
	}
}