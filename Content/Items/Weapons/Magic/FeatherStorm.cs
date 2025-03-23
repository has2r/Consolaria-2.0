using Consolaria.Content.Projectiles.Friendly;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Magic {
    public class FeatherStorm : ModItem {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 28; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Magic;
            Item.damage = 20;
            Item.knockBack = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 24;

            Item.value = Item.buyPrice(gold: 1, silver: 15);
            Item.rare = ItemRarityID.Orange;

            Item.mana = 8;
            Item.UseSound = SoundID.Item42;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<SharpFeather>();
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int numberProjectiles = 3 + Main.rand.Next(2);
            for (int index = 0; index < numberProjectiles; ++index) {
                Vector2 vector2_1 = new Vector2((float)(player.position.X + player.width * 0.5 + Main.rand.Next(201) * -player.direction + (Main.mouseX + (double)Main.screenPosition.X - player.position.X)), (float)(player.position.Y + player.height * 0.5 - 600));
                vector2_1.X = (float)((vector2_1.X + player.Center.X) / 2.0) + Main.rand.Next(-100, 101);
                vector2_1.Y -= 100 * index;
                float num12 = Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if (num13 < 0.0) num13 *= -1f;
                if (num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt(num12 * (double)num12 + num13 * (double)num13);
                float num15 = Item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = num17 + Main.rand.Next(-10, 21) * 0.05f;
                Projectile.NewProjectile(source, vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}