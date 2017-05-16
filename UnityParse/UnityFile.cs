using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlexParse
{
	public sealed class UnityFile
	{
		public AssetCollection Assets { get; }
	}



	public sealed class AssetCollection : IEnumerable<Asset>
	{
		public Asset this[int pathID]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int Add(Asset item)
		{
			throw new NotImplementedException();
		}

		public void Remove(Asset item)
		{
			throw new NotImplementedException();
		}

		#region IEnumerable<Asset>

		public IEnumerator<Asset> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion IEnumerable<Asset>
	}

	public interface Asset
	{
		AssetCode ClassID { get; }
	}

	public enum AssetCode
	{
	}
}