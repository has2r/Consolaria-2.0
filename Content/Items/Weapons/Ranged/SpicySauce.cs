using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged {
    public class SpicySauce : ModItem {
        public override void SetStaticDefaults () {
            // Tooltip.SetDefault("Covers enemies in oil" + "\nSet oiled enemies on fire for extra damage");
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults () {
            int width = 32; int height = width;
            Item.Size = new Vector2(width, height);

            Item.damage = 38;
            Item.knockBack = 2f;

            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;

            Item.maxStack = 9999;
            Item.noUseGraphic = true;

            Item.consumable = true;
            Item.autoReuse = false;
            Item.useTime = Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.SpicySauce>();
            Item.shootSpeed = 10f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item87;

            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.Orange;
        }
    }
}