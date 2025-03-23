using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials {
    public class WhiteThread : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults() {
            int width = 28; int height = 20;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.White;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 4);
        }
    }
}
