using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using FlexParse;

namespace UnityParse.BakedFiles
{
	[DataContract]
	public sealed class Metadata
	{
		[DataMember]
		public string UnityVersion { get; private set; }

		[DataMember]
		public int TypeVersion { get; private set; }

		[DataMember]
		public bool HasTypeTree { get; private set; }

		[DataMember]
		public IList<AssetType> Types { get; private set; }

		[DataMember]
		public IList<AssetReference> PreloadTable { get; private set; }

		[DataMember]
		public IList<Dependency> Dependencies { get; private set; }

		[DataMember]
		public IList<AssetInfo> AssetIndex { get; private set; }

		public static Metadata FromStream(Stream stream, TypeSet types, Header header)
		{
			if (stream == null) throw new ArgumentNullException();
			stream.Position = header.MetadataOffset;
			TypeDef metaDefinition = types["AssetsFile.Metadata"];
			FlexReader reader = new FlexReader(stream) { IsLittleEndian = !header.IsBigEndian };
			var context = new ReaderContext(types, reader);
			context.Scope.GlobalFrame["Format"] = header.Format;
			var json = metaDefinition.Read(context);
			var meta = json.ToObject<Metadata>();
			if (header.Format > 15)
			{
				meta.LookupClassIDs();
			}
			return meta;
		}

		private void LookupClassIDs()
		{
			foreach (var info in AssetIndex)
			{
				if (info.TreeIndex < Types.Count)
				{
					var type = Types[info.TreeIndex];
					if (type.ScriptIndex == -1)
					{
						info.ClassID = type.ClassID;
						info.InheritedUnityClass = type.ClassID;
						info.ScriptIndex = -1;
					}
					else
					{
						info.ClassID = type.ClassID;
						info.InheritedUnityClass = type.ClassID;
						info.ScriptIndex = type.ScriptIndex;
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}
	}
}