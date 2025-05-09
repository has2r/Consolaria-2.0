using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class RainbowRain : ModSystem {
        public override void PostUpdateWorld() {
            Vector2 randomPosition = new Vector2(Main.rand.Next(Main.maxTilesX * 16), 200);
            if (Main.dayTime && (SeasonalEvents.configEnabled && SeasonalEvents.IsPatrickDay()) || (!SeasonalEvents.configEnabled && Main.raining)) {
                if (Main.time % 450 == 0 && Main.rand.NextBool(1 + RainbowDropDenominator()) && Main.rand.Next(10) < 7) {
                    int item = Item.NewItem(Entity.GetSource_NaturalSpawn(), randomPosition, 20, 20, ModContent.ItemType<Content.Items.Materials.RainbowPiece>(), 1, false, 0, false, false);
                    if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        private static int RainbowDropDenominator() {
            if (SeasonalEvents.configEnabled)
                return SeasonalEvents.IsPatrickDay() ? 6 : 0;
            else return Main.raining ? 10 : 0;
        }
    }
}