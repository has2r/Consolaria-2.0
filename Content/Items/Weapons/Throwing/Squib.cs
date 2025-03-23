using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Throwing {
    public class Squib : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults() {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 30;
            Item.knockBack = 2f;

            Item.useAnimation = Item.useTime = 20;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(copper: 15);
            Item.rare = ItemRarityID.White;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.Squib>();
            Item.shootSpeed = 6.5f;
        }
    }
}