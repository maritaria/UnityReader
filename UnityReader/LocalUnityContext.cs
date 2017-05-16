using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityReader.Definitions;

namespace UnityReader
{
	public sealed class LocalUnityContext : UnityContext
	{
		private static readonly string LibraryPrefix = @"library/";
		private static string LibraryReplacement = "Resources";

		private string _baseDir;
		private Dictionary<string, AssetsFile> _loadedFiles = new Dictionary<string, AssetsFile>();

		public TypeTable TypeTable { get; } = new TypeTable();

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
			AssetsFile result;
			if (_loadedFiles.TryGetValue(name, out result))
			{
				return result;
			}
			else
			{
				string path = Path.Combine(_baseDir, name);
				byte[] data = File.ReadAllBytes(path);
				var file = new AssetsFile(this, data);
				_loadedFiles.Add(name, file);
				return file;
			}
		}
	}
}