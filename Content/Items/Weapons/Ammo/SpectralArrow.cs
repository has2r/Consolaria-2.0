using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ammo {
    public class SpectralArrow : ModItem {
        public override void SetStaticDefaults ()
            => Item.ResearchUnlockCount = 99;

        public override void SetDefaults () {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.damage = 12;
            Item.knockBack = 0f;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.SpectralArrow>();
            Item.shootSpeed = 3.5f;

            Item.value = Item.sellPrice(silver: 1, copper: 10);
            Item.rare = ItemRarityID.Orange;

            Item.ammo = AmmoID.Arrow;
        }
    }
}