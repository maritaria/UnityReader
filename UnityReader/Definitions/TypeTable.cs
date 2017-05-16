using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace UnityReader.Definitions
{
	public sealed class TypeTable
	{
		private HashSet<string> _supportedVersionPrefixes = new HashSet<string>();
		private Dictionary<string, UnityTypeNode> _typeByName = new Dictionary<string, UnityTypeNode>();
		private Dictionary<AssetCodes, UnityTypeNode> _typeByAssetCode = new Dictionary<AssetCodes, UnityTypeNode>();

		[XmlElement("UnityType")]
		public List<UnityTypeNode> Types { get; } = new List<UnityTypeNode>();

		public void AddTypeNode(UnityTypeNode node)
		{
			_typeByName[node.TypeName] = node;
			if (node.AssetCode >= 0)
			{
				_typeByAssetCode[(AssetCodes)node.AssetCode] = node;
			}
		}

		public UnityTypeNode this[string name]
		{
			get { return _typeByName[name]; }
		}

		public UnityTypeNode this[AssetCodes assetCode]
		{
			get { return _typeByAssetCode[assetCode]; }
		}

		public bool CanDeserialize(AssetCodes assetCode)
		{
			return _typeByAssetCode.ContainsKey(assetCode);
		}

		public bool CheckSupport(string query)
		{
			foreach (string prefix in _supportedVersionPrefixes)
			{
				if (query.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
	}
}