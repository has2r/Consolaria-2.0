using Consolaria.Content.Projectiles.Friendly;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Magic {
    public class OcramsEye : ModItem {
        public override void SetDefaults () {
            int width = 36; int height = width;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Magic;
            Item.damage = 62;
            Item.knockBack = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.reuseDelay = 30;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.mana = 16;
            Item.UseSound = SoundID.Item33;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<OcramEyeGlow>();
            Item.shootSpeed = 10f;
        }

        public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
            position += new Vector2(-_velocity.Y, _velocity.X) * (3f * player.direction);
        }

        public override bool Shoot (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            float projectilesCount = Main.rand.Next(3, 5);
            float rotation = MathHelper.ToRadians(7);
            for (int i = 0; i < projectilesCount; i++) {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectilesCount - 1))) * 1.25f;
                Projectile.NewProjectile(source, position + player.velocity, perturbedSpeed, ModContent.ProjectileType<OcramEyeLaser>(), damage, knockback, player.whoAmI);
            }
            return true;
        }
    }
}