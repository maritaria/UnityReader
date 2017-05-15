using System.Collections.Generic;

namespace UnityReader.Types
{
	[UnityType(1)]
	public class GameObject : AssetData
	{
		public ICollection<AssetReference<Component>> Components { get; } = new List<AssetReference<Component>>();
		public uint Layer { get; set; }
		public string Name { get; set; }
		public ushort Tag { get; set; }
		public bool IsActive { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var reference = new AssetReference<Component>();
				reference.Read(owner, reader);
				Components.Add(reference);
			}
			Layer = reader.ReadUInt32();
			int nameLenght = reader.ReadInt32();
			Name = reader.ReadStringFixed(nameLenght);
			Tag = reader.ReadUInt16();
			IsActive = reader.ReadBool();
		}
	}

}