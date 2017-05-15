using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityReader
{
	public class AssetsFileTable : IEnumerable<AssetFileInfo>
	{
		private AssetsFile _file;
		private Dictionary<long, AssetFileInfo> _items = new Dictionary<long, AssetFileInfo>();

		public AssetsFileTable(AssetsFile file)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));
			_file = file;
		}

		public void Read(UnityBinaryReader reader)
		{
			_items.Clear();
			int count = reader.ReadInt32();

			if (_file.Header.Version >= 14)
			{
				reader.Align(4);
			}

			for (int i = 0; i < count; i++)
			{
				var info = new AssetFileInfo(_file, reader);
				_items.Add(info.Index, info);
			}
		}

		public AssetFileInfo this[long pathID]
		{
			get { return _items[pathID]; }
			set
			{
				if (value == null)
				{
					_items.Remove(pathID);
				}
				_items[pathID] = value;
			}
		}

		#region IEnumerable<AssetFileInfo>

		public IEnumerator<AssetFileInfo> GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		#endregion IEnumerable<AssetFileInfo>
	}
}