using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FlexParse;
using Newtonsoft.Json.Linq;

namespace UnityParse.BakedFiles
{
	[DataContract]
	public sealed class Header
	{
		[DataMember]
		public long AssetsOffset { get; private set; }

		[DataMember]
		public int AssetsSize { get; internal set; }

		public long MetadataOffset { get; private set; }

		[DataMember]
		public int MetadataSize { get; private set; }

		[DataMember]
		public int Format { get; private set; }

		[DataMember]
		public bool IsBigEndian { get; private set; }

		[DataMember]
		public ICollection<byte> Unknown { get; private set; }

		public static Header FromStream(Stream stream, TypeSet types)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (types == null) throw new ArgumentNullException(nameof(types));
			stream.Position = 0;
			TypeDef headerDefinition = types["AssetsFile.Header"];
			FlexReader reader = new FlexReader(stream) { IsLittleEndian = false };
			var json = headerDefinition.Read(new ReaderContext(types, reader));
			var header = json.ToObject<Header>();
			if (header.Format > 8)
			{
				header.MetadataOffset = reader.BaseStream.Position;
			}
			else
			{
				header.MetadataOffset = header.AssetsOffset + header.AssetsSize;
			}
			return header;
		}

		public void WriteTo(Stream stream, TypeSet types)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (types == null) throw new ArgumentNullException(nameof(types));
			stream.Position = 0;
			TypeDef headerDefinition = types["AssetsFile.Header"];
			FlexWriter writer = new FlexWriter(stream) { IsLittleEndian = false };
			var json = JObject.FromObject(this);
			headerDefinition.Write(json, new WriterContext(types, writer));
		}
	}
}