namespace UnityReader
{

	public sealed class Guid128
	{
		public long Lower { get; set; }
		public long Upper { get; set; }

		public void Read(UnityReader reader, int version)
		{
			bool littleEndian = reader.IsLittleEndian;
			reader.IsLittleEndian = false;
			try
			{
				Upper = reader.ReadInt64();
				Lower = reader.ReadInt64();
			}
			finally
			{
				reader.IsLittleEndian = littleEndian;
			}
		}
	}
}