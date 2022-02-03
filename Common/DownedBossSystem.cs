using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Common
{
	public class DownedBossSystem : ModSystem
	{
		public static bool downedLepus = false;

		public override void OnWorldLoad() {
			downedLepus = false;
		}

		public override void OnWorldUnload() {
			downedLepus = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (downedLepus)
				tag["downedLepus"] = true;
		}

		public override void LoadWorldData(TagCompound tag) {
			var downed = tag.GetList<string>("downed");

			downedLepus = downed.Contains("downedLepus");
			//downedOtherBoss = downed.Contains("downedOtherBoss");
		}

		public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = downedLepus;
			//flags[1] = downedOtherBoss;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			//Order of operations is important and has to match that of NetSend
			BitsByte flags = reader.ReadByte();
			downedLepus = flags[0];
			//downedOtherBoss = flags[1];
		}
	}
}
