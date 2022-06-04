using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Common
{
    public class RainbowRain : ModSystem 
    {
        private int rainbowDropCount;

        public override void PostUpdateWorld() {
            Vector2 randomPosition = new(Main.rand.Next(Main.maxTilesX * 16), Main.maxTilesY);

            if (SeasonalEvents.isSaintPatricksDay || !SeasonalEvents.enabled && Main.raining) {
                if (Main.dayTime) {
                    rainbowDropCount++;
                    if (rainbowDropCount % 450 == 0 && Main.rand.Next(4) == 0)
                        Item.NewItem(Item.GetSource_NaturalSpawn(), randomPosition, ModContent.ItemType<Content.Items.Materials.RainbowPiece>(), 1);                
                }
                else rainbowDropCount = 0;
            }
        }
    }
}