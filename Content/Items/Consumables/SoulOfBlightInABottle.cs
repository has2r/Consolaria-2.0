using Consolaria.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Consumables {
    public class SoulOfBlightInABottle : ModItem {
        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults () {
            Item.CloneDefaults(ItemID.SoulBottleNight);
            Item.createTile = ModContent.TileType<Tiles.SoulOfBlightInABottle>();
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient<SoulofBlight>(1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}