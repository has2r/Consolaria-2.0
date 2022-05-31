using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Common
{
	public class DownedBossSystem : ModSystem
	{
		public static bool downedLepus = false;
		public static bool downedTurkor = false;
		public static bool downedOcram = false;

		public override void OnWorldLoad() {
			downedLepus = false;
			downedTurkor = false;
			downedOcram = false;
		}

		public override void OnWorldUnload() {
			downedLepus = false;
			downedTurkor = false;
			downedOcram = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (downedLepus) tag["downedLepus"] = true;
			if (downedTurkor) tag["downedTurkor"] = true;
			if (downedOcram) tag["downedOcram"] = true;
		}

		public override void LoadWorldData(TagCompound tag) {
			var downed = tag.GetList<string>("downed");
			downedLepus = downed.Contains("downedLepus");
			downedTurkor = downed.Contains("downedTurkor");
			downedOcram = downed.Contains("downedOcram");
		}

		public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = downedLepus;
			flags[1] = downedTurkor;
			flags[2] = downedOcram;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedLepus = flags[0];
			downedTurkor = flags[1];
			downedOcram = flags[2];
		}
	}
}
