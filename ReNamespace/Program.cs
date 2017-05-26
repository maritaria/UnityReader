using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace ReNamespace
{
	internal static class Program
	{
		private static void Main()
		{
			string file = @"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\Managed\Assembly-CSharp.dll";
			using (var module = ModuleDefMD.Load(file + ".original"))
			{
				var unnamedNamespace = module.Types.Where(t => t.Namespace == "");

				foreach (var type in unnamedNamespace.Where(t => t.Name.StartsWith("uScript")))
				{
					type.Namespace = "TerraTech.uScript";
				}
				foreach (var type in unnamedNamespace.Where(t => t.Name.StartsWith("Man")))
				{
					type.Namespace = "TerraTech.Managers";
				}
				foreach (var type in unnamedNamespace.Where(t => t.Name.StartsWith("Mission")))
				{
					type.Namespace = "TerraTech.uScript.Missions";
				}

				module.Write(@"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\Managed\Assembly-CSharp.dll");
			}
		}
	}
}