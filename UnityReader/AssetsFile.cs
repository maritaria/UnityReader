using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityReader
{
	public sealed class AssetsFile
	{
		private byte[] _buffer;
		public UnityContext Context { get; }
		public AssetsFileHeader Header { get; } = new AssetsFileHeader();
		public TypeTree TypeTree { get; } = new TypeTree();
		public AssetsFileTable Assets { get; }
		public PreloadList PreloadList { get; } = new PreloadList();
		public DependencyList Dependencies { get; }

		public AssetsFile(UnityContext context, byte[] data)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (data == null) throw new ArgumentNullException(nameof(data));
			_buffer = data;
			Context = context;
			Assets = new AssetsFileTable(this);
			Dependencies = new DependencyList(this);

			var reader = CreateReader(0);
			Header.Read(reader);
			reader.Position = Header.MetadataOffset;
			TypeTree.Read(reader, Header.Version);
			Assets.Read(reader);
			PreloadList.Read(reader, Header.Version);
			Dependencies.Read(reader, Header.Version);
		}

		public UnityBinaryReader CreateReader(long startingPos)
		{
			MemoryStream ms = new MemoryStream(_buffer);
			var reader = new UnityBinaryReader(ms);
			reader.Position = startingPos;
			reader.IsLittleEndian = Header.IsLittleEndian;
			return reader;
		}
	}
}