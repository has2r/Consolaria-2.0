using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpectralArrow : ModProjectile {

        private int hitCounter;
        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 2;
        }

        public override void SetDefaults () {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 5;

            Projectile.alpha = byte.MaxValue;
            Projectile.light = 0.2f;

            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.timeLeft = 180;
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => hitCounter++;

        public override void ModifyHitNPC (NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
          => damage -= (int) (damage * hitCounter * 0.1f);

        public override void Kill (int timeLeft) {
            if (Projectile.owner == Main.myPlayer) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f, 100, default, 0.9f);
                SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            }
        }

        public override Color? GetAlpha (Color lightColor)
            => new Color?(Color.White * 0.75f * (hitCounter * 0.25f));

        public override bool? CanCutTiles ()
            => false;
    }
}
