using Newtonsoft.Json.Linq;

namespace UnityReader.Definitions
{
	public abstract class SerializationNode
	{
		public abstract void Read(UnityReader reader, UnityContext context, JObject currentObject);
	}
}