using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FlexParse;

namespace UnityParse.BakedFiles
{
	[CollectionDataContract]
	public sealed class AssetCollection : IEnumerable<Asset>
	{
		private Dictionary<long, Asset> _assets = new Dictionary<long, Asset>();

		public static AssetCollection FromStream(Stream stream, TypeSet types, Header header, Metadata meta)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));

			AssetCollection result = new AssetCollection();
			var reader = new FlexReader(stream) { IsLittleEndian = !header.IsBigEndian };
			var readerContext = new ReaderContext(types, reader);
			foreach (AssetInfo info in meta.AssetIndex)
			{
				reader.BaseStream.Position = header.AssetsOffset + info.DataOffset;
				byte[] data = reader.ReadBytes(info.DataLength);
				result._assets.Add(info.Index, new Asset(result, info.ClassID, data)
				{
					IsPreloaded = meta.PreloadTable.Any(p => p.FileID == 0 && p.PathID == info.Index)
				});
			}
			return result;
		}

		public long GetPathID(Asset value)
		{
			return _assets.First(kv => kv.Value == value).Key;
		}

		public Asset this[long pathID]
		{
			get { return _assets[pathID]; }
		}

		public void WriteTo(Stream stream, TypeSet types, Header header, Metadata meta)
		{
			stream.Position = header.AssetsOffset;

			using (MemoryStream assetBuffer = new MemoryStream())
			{
				var writer = new FlexWriter(assetBuffer);
				foreach (KeyValuePair<long, Asset> kv in _assets)
				{
					var index = kv.Key;
					var asset = kv.Value;
					var info = meta.AssetIndex.Single(a => a.Index == kv.Key);

					writer.Align(8);
					info.DataOffset = (int)assetBuffer.Position;
					info.DataLength = asset.Data.Length;
					writer.WriteBytes(asset.Data);
				}

				header.AssetsSize = (int)(header.AssetsOffset + assetBuffer.Length);
				header.WriteTo(stream, types);
				meta.WriteTo(stream, types, header);

				assetBuffer.Position = 0;
				stream.Position = header.AssetsOffset;
				assetBuffer.CopyTo(stream);
			}
		}

		#region IEnumerable<Asset>

		public IEnumerator<Asset> GetEnumerator()
		{
			return _assets.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _assets.Values.GetEnumerator();
		}

		#endregion IEnumerable<Asset>
	}
}