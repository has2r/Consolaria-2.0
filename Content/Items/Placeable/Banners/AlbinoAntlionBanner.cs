using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable.Banners {
    public class AlbinoAntlionBanner : ModItem {

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 10; int height = 24;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;

            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners>());
            Item.placeStyle = 8;

            Item.consumable = true;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 10);
        }
    }
}
