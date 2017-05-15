using System;
using System.Collections.Generic;

namespace UnityReader
{
	public sealed class TypeTree
	{
		public string UnityVersion { get; set; }//max 25 bytes
		public int TypeVersion { get; set; }
		public bool HasTypeTree { get; set; }
		public List<Unity5Type> Types { get; } = new List<Unity5Type>();

		private int _unknown;
		private int _format;

		public void Read(UnityBinaryReader reader, int version)
		{
			_format = version;
			HasTypeTree = true;

			if (version <= 6)
			{
				UnityVersion = $"Unsupported Format";
				TypeVersion = 0;
				return;
			}

			UnityVersion = reader.ReadString();
			TypeVersion = reader.ReadInt32();
			if (version >= 13)
			{
				HasTypeTree = reader.ReadBool();
			}
			var fieldCount = reader.ReadInt32();
			if (version > 13)
			{
				for (int i = 0; i < fieldCount; i++)
				{
					Types.Add(new Unity5Type(reader, version, HasTypeTree));
				}
			}
			else
			{
				throw new NotImplementedException();
			}
			if (version < 14)
			{
				_unknown = reader.ReadInt32();
			}
		}
	}
}