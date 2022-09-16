using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Common {
    public class SeasonalEvents : ModSystem {
        public static bool allEventsForToday;
        public static bool enabled = ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled || ConsolariaConfig.Instance.smallEventsEnabled || allEventsForToday,
            isEaster = (ConsolariaConfig.Instance.easterEnabled && SeasonalEventsHelper.CheckEaster()) || allEventsForToday,
            isThanksgiving = (ConsolariaConfig.Instance.thanksgivingEnabled && SeasonalEventsHelper.CheckThanksgiving()) || allEventsForToday,
            isChineseNewYear = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckChineseNewYear()) || allEventsForToday,
            isOktoberfest = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckOktoberfest()) || allEventsForToday,
            isSaintPatricksDay = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckSaintPatricksDay()) || allEventsForToday,
            isValentinesDay = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckValentinesDay()) || allEventsForToday;

		public override void PostUpdateTime () {
			string text = "The spirits of celebration dissipate...";
			if (Main.time == 24000.0) {
				if (allEventsForToday) {
					allEventsForToday = false;
					if (Main.netMode == NetmodeID.SinglePlayer)
						Main.NewText(text, Color.HotPink);
					else if (Main.netMode == NetmodeID.Server)
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), Color.HotPink);
					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.WorldData);
				}
			}
		}

		public override void OnWorldLoad ()
			=> allEventsForToday = false;
		
		public override void OnWorldUnload ()
			=> allEventsForToday = false;

		public override void SaveWorldData (TagCompound tag) {
			if (allEventsForToday) {
				tag ["allEventsForToday"] = true;
			}
		}

		public override void LoadWorldData (TagCompound tag) 
			=> allEventsForToday = tag.ContainsKey("allEventsForToday");
		

		public override void NetSend (BinaryWriter writer) {
			var flags = new BitsByte();
			flags [0] = allEventsForToday;
			writer.Write(flags);
		}

		public override void NetReceive (BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			allEventsForToday = flags [0];
		}
	}
}