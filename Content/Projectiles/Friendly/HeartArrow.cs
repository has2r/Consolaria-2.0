using Consolaria.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {

    public class HeartArrow : ModProjectile {

        public override void SetDefaults () {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
        }

        public override void AI () {
            if (Main.rand.NextBool(3)) {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X + 2f, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.HeartCrystal, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.25f);
                Main.dust [dust].noGravity = true;
            }
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
            if (!target.buffImmune [BuffID.Confused] && Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<Stunned>(), 90);
        }

        public override void Kill (int timeLeft) {
            if (Projectile.owner == Main.myPlayer) {
                if (Main.rand.NextBool(4) && !Projectile.noDropItem) {
                    Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, Projectile.width, Projectile.height, ModContent.ItemType<Items.Weapons.Ammo.HeartArrow>());
                }
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.HeartCrystal, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f, 100, default, 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            }
        }
    }
}
