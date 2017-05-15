namespace UnityReader.Types
{
	[UnityType(115)]
	public class MonoScript : AssetData
	{
		public string Name { get; set; }
		public int ExecutionOrder { get; set; }
		public Hash128 PropertiesHash { get; set; }
		public string ClassName { get; set; }
		public string Namespace { get; set; }
		public string AssemblyName { get; set; }
		public bool IsEditorScript { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Name = reader.ReadStringFixed(reader.ReadInt32());
			ExecutionOrder = reader.ReadInt32();
			PropertiesHash = reader.Read<Hash128>(owner);
			ClassName = reader.ReadStringFixed(reader.ReadInt32());
			Namespace = reader.ReadStringFixed(reader.ReadInt32());
			AssemblyName = reader.ReadStringFixed(reader.ReadInt32());
			IsEditorScript = reader.ReadBool();
		}
	}
}