using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityReader
{
	public class DependencyList : UnityList<DependencyList.Dependency>, IEnumerable<DependencyList.Dependency>
	{
		public AssetsFile MainFile { get; }
		public DependencyList(AssetsFile scope)
		{
			MainFile = scope;
		}
		public AssetsFile GetFile(int fileID)
		{
			if (fileID == 0)
			{
				return MainFile;
			}
			else
			{
				return Items[fileID - 1].Load(MainFile.Context);
			}
		}

		#region IEnumerable<AssetsFileDependency>

		public IEnumerator<Dependency> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		#endregion IEnumerable<AssetsFileDependency>

		public class Dependency : UnityElement
		{
			public string BufferedPath { get; set; }
			public Guid128 Guid { get; set; }
			public int Type { get; set; }
			public string AssetPath { get; set; }

			void UnityElement.Read(UnityReader reader, int version)
			{
				BufferedPath = reader.ReadString();
				Guid = new Guid128();
				Guid.Read(reader, version);
				Type = reader.ReadInt32();
				AssetPath = reader.ReadString();
			}

			public AssetsFile Load(UnityContext context)
			{
				return context.LoadFile(AssetPath);
			}
		}
	}
}