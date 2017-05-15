using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	[UnityType(13)]
	public sealed class InputManager : GlobalGameManager
	{
		public ICollection<InputAxis> Axes { get; } = new List<InputAxis>();

		public override void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			reader.ReadArray(owner, Axes);
		}

		public sealed class InputAxis : NamedAssetData
		{
			public string Name { get; set; }
			public string DescriptiveName { get; set; }
			public string DescriptiveNegativeName { get; set; }
			public string NegativeButton { get; set; }
			public string PositiveButton { get; set; }
			public string NegativeButtonAlt { get; set; }
			public string PositiveButtonAlt { get; set; }
			public float Gravity { get; set; }
			public float Dead { get; set; }
			public float Sensitivity { get; set; }
			public bool Snap { get; set; }
			public bool Invert { get; set; }
			public int Type { get; set; }
			public int Axis { get; set; }
			public int JoyNum { get; set; }

			public void Read(AssetsFile owner, UnityBinaryReader reader)
			{
				Name = reader.ReadStringFixed(reader.ReadInt32());
				DescriptiveName = reader.ReadStringFixed(reader.ReadInt32());
				DescriptiveNegativeName = reader.ReadStringFixed(reader.ReadInt32());
				NegativeButton = reader.ReadStringFixed(reader.ReadInt32());
				PositiveButton = reader.ReadStringFixed(reader.ReadInt32());
				NegativeButtonAlt = reader.ReadStringFixed(reader.ReadInt32());
				PositiveButtonAlt = reader.ReadStringFixed(reader.ReadInt32());
				Gravity = reader.ReadFloat();
				Dead = reader.ReadFloat();
				Sensitivity = reader.ReadFloat();
				Snap = reader.ReadBool();
				Invert = reader.ReadBool();
				Type = reader.ReadInt32();
				Axis = reader.ReadInt32();
				JoyNum = reader.ReadInt32();
			}
		}
	}
}
