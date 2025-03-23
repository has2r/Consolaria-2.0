using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class SoulOfBlightInABottle : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.SoulBottleNight);
            Item.createTile = ModContent.TileType<Tiles.SoulOfBlightInABottleTile>();
        }
    }
}