using Consolaria.Content.Projectiles.Friendly;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Magic {
    public class RomanCandle : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 40; int height = 16;
            Item.Size = new Vector2(width, height);

            Item.damage = 10;
            Item.knockBack = 4.5f;
            Item.DamageType = DamageClass.Magic;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 16;

            Item.mana = 5;
            Item.shoot = ModContent.ProjectileType<RomanFlame>();
            Item.shootSpeed = 2.5f;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;

            Item.UseSound = SoundID.Item11;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
            position += _velocity * 35;
            position += new Vector2(-_velocity.Y, _velocity.X) * (-5f * player.direction);
        }

        public override Vector2? HoldoutOffset()
            => new Vector2(0, 0);
    }
}