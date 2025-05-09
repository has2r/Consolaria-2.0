using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class BrokenHeart : PetItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.Cupid>(), ModContent.BuffType<Buffs.Cupid>());

            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 1);
        }
    }
}