using Microsoft.Xna.Framework;

using System;
using System.IO;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Common {
    public class SeasonalEvents : ModSystem {
        static DateTime currentDate = DateTime.Now;

        public static bool allEventsForToday;
        public static bool configEnabled = ModContent.GetInstance<ConsolariaConfig>().easterEnabled || ModContent.GetInstance<ConsolariaConfig>().thanksgivingEnabled || ModContent.GetInstance<ConsolariaConfig>().smallEventsEnabled;

        public override void PostUpdateTime() {
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
                if (WishbonePlayer.purchasedWishbone)
                    WishbonePlayer.purchasedWishbone = false;
            }
        }

        public static bool IsEaster()
            => currentDate.Month == 4 || allEventsForToday;

        public static bool IsThanksgiving()
            => currentDate.Month == 11 || allEventsForToday;

        public static bool IsChineseNewYear()
            => ((currentDate.Day >= 20 && currentDate.Month == 1) || (currentDate.Day <= 15 && currentDate.Month == 2)) || allEventsForToday;

        public static bool IsOktoberfest()
            => ((currentDate.Day >= 27 && currentDate.Month == 9) || (currentDate.Month == 10)) || allEventsForToday;

        public static bool IsPatrickDay()
            => (currentDate.Day >= 5 && currentDate.Month == 3) || allEventsForToday;

        public static bool IsValentineDay()
            => currentDate.Month == 2 || allEventsForToday;

        public override void OnWorldLoad()
            => allEventsForToday = false;

        public override void OnWorldUnload()
            => allEventsForToday = false;

        public override void SaveWorldData(TagCompound tag) {
            if (allEventsForToday) {
                tag["allEventsForToday"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
            => allEventsForToday = tag.ContainsKey("allEventsForToday");

        public override void NetSend(BinaryWriter writer) {
            var flags = new BitsByte();
            flags[0] = allEventsForToday;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader) {
            BitsByte flags = reader.ReadByte();
            allEventsForToday = flags[0];
        }
    }

    public class WishbonePlayer : ModPlayer {
        public static bool purchasedWishbone;

        public override void Initialize()
            => purchasedWishbone = false;

        public override void SaveData(TagCompound tag)
            => tag.Add("purchasedWishbone", purchasedWishbone);

        public override void LoadData(TagCompound tag)
            => purchasedWishbone = tag.GetBool("purchasedWishbone");

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item) {
            if (vendor.type == NPCID.SkeletonMerchant && item.type == ModContent.ItemType<Content.Items.Consumables.Wishbone>())
                purchasedWishbone = true;
        }
    }
}
