using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FlexParse;
using Newtonsoft.Json.Linq;

namespace UnityParse.BakedFiles
{
	[CollectionDataContract]
	public sealed class AssetCollection : IEnumerable<Asset>
	{
		private Dictionary<long, Asset> _assets = new Dictionary<long, Asset>();

		public static AssetCollection FromStream(Stream stream, Header header, TypeSet types)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));

			Metadata meta = Metadata.FromStream(stream, types, header);

			AssetCollection result = new AssetCollection();
			var reader = new FlexReader(stream) { IsLittleEndian = !header.IsBigEndian };
			var readerContext = new ReaderContext(types, reader);
			foreach (AssetInfo info in meta.AssetIndex)
			{
				reader.BaseStream.Position = header.AssetsOffset + info.DataOffset;
				string typeName = info.ClassID.ToString();
				var typeDef = types[typeName];
				JObject value = (JObject)typeDef.Read(readerContext);
				result._assets.Add(info.Index, new Asset(result, info.ClassID, value));
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

		public void UpdateFile(Stream stream, TypeSet types, Header header)
		{
			stream.Position = header.AssetsOffset;

			MemoryStream assetsStream = new MemoryStream();
			throw new NotImplementedException();
		}

		public void UpdateMeta(int format)
		{
			throw new NotImplementedException();
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