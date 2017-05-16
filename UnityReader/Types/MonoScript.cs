using UnityReader.Definitions;

namespace UnityReader.Types
{
	[UnityType(AssetCodes.MonoScript)]
	public class MonoScript : AssetObject
	{
		public string Name { get; set; }
		public int ExecutionOrder { get; set; }
		public Hash128 PropertiesHash { get; set; }
		public string ClassName { get; set; }
		public string Namespace { get; set; }
		public string AssemblyName { get; set; }
		public bool IsEditorScript { get; set; }
	}
}