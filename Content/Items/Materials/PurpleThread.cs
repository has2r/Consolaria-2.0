using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials
{
    public class PurpleThread : ModItem
    {
        public override void SetStaticDefaults() 
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults() {
            int width = 28; int height = 20;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.White;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 4);
        }

        public override void AddRecipes() 
        {
            CreateRecipe()
                .AddIngredient(ItemID.DeathweedSeeds, 3)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
