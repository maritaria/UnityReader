using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader
{
	public sealed class LocalUnityContext : UnityContext
	{
		private static readonly string LibraryPrefix = @"library/";
		private static string LibraryReplacement = "Resources";
		private string _baseDir;
		private Dictionary<string, AssetsFile> _loadedFiles = new Dictionary<string, AssetsFile>();

		public LocalUnityContext(string baseDir)
		{
			_baseDir = baseDir;
		}

		public AssetsFile LoadFile(string name)
		{
			if (name.StartsWith(LibraryPrefix, StringComparison.OrdinalIgnoreCase))
			{
				name = Path.Combine(LibraryReplacement, name.Substring(LibraryPrefix.Length));
			}

			string path = Path.Combine(_baseDir, name);
			using (FileStream fs = File.OpenRead(path))
			{
				var reader = new UnityBinaryReader(fs);
				var file = new AssetsFile(this, reader);
				_loadedFiles.Add(name, file);
				return file;
			}
		}
	}
}
