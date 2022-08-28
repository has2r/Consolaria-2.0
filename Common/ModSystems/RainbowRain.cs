using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class RainbowRain : ModSystem {
        private int dropChance;
        public override void PostUpdateWorld () {
            if (SeasonalEvents.enabled && Main.raining) dropChance = 5;
            if (SeasonalEvents.isSaintPatricksDay) dropChance = 3;

            Vector2 randomPosition = new Vector2(Main.rand.Next(Main.maxTilesX * 16), 200);
            if (SeasonalEvents.isSaintPatricksDay || (!SeasonalEvents.enabled && Main.raining)) {
                if (Main.dayTime) {
                    if (Main.time % 450 == 0 && Main.rand.NextBool(dropChance)) {
                        int item = Item.NewItem(Entity.GetSource_NaturalSpawn(), randomPosition, 20, 20, ModContent.ItemType<Content.Items.Materials.RainbowPiece>(), 1, false, 0, false, false);
                        if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
        }
    }
}