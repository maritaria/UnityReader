using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityReader.Types
{
	[UnityType(11)]
	public sealed class AudioManager : AssetData
	{
		public float Volume { get; set; }
		public float RolloffScale { get; set; }
		public float DopplerFactor { get; set; }
		public SpeakerMode DefaultSpeakerMode { get; set; }

		public int SampleRate { get; set; }
		public int DspBufferSize { get; set; }
		public int VirtualVoiceCount { get; set; }
		public int RealVoiceCount { get; set; }
		public string SpatializerPlugin { get; set; }
		public bool DisableAudio { get; set; }
		public bool VirtualizeEffects { get; set; }

		public void Read(AssetsFile owner, UnityBinaryReader reader)
		{
			Volume = reader.ReadFloat();
			RolloffScale = reader.ReadFloat();
			DopplerFactor = reader.ReadFloat();
			DefaultSpeakerMode = (SpeakerMode)reader.ReadInt32();
			SampleRate = reader.ReadInt32();
			DspBufferSize = reader.ReadInt32();
			VirtualVoiceCount = reader.ReadInt32();
			RealVoiceCount = reader.ReadInt32();
			SpatializerPlugin = reader.ReadStringFixed(reader.ReadInt32());
			DisableAudio = reader.ReadBool();
			VirtualizeEffects = reader.ReadBool();
		}

		public enum SpeakerMode
		{

		}
	}
}
