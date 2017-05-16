using System;
using System.Diagnostics;

namespace UnityReader
{

	public sealed class AssetsFileHeader
	{
		private int _metadataSize;
		private int _assetsSize;
		public int Version { get; private set; }
		public bool IsLittleEndian { get; private set; }
		public byte[] Unknown { get; private set; }
		public long AssetsOffset { get; private set; }
		public long MetadataOffset { get; private set; }

		public void Read(UnityReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			reader.IsLittleEndian = false;
			_metadataSize = reader.ReadInt32();
			_assetsSize = reader.ReadInt32();
			Debug.Assert(_assetsSize >= 0);
			Version = reader.ReadInt32();
			AssetsOffset = reader.ReadInt32();
			if (Version >= 9)
			{
				IsLittleEndian = !reader.ReadBool();
				Unknown = reader.ReadBytes(3);
				reader.IsLittleEndian = IsLittleEndian;
				MetadataOffset = reader.Position;
			}
			else
			{
				MetadataOffset = AssetsOffset + _assetsSize;
				//Meta comes after object data
				throw new NotImplementedException();
			}
		}
	}

}