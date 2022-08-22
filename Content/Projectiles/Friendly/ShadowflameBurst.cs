using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class ShadowflameBurst : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private readonly int lifeLimit = 40;

        public override void SetDefaults () {
            int width = 8; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = lifeLimit;

            Projectile.extraUpdates = 1;
        }

        public override void AI () {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.15f) / 255f, ((255 - Projectile.alpha) * 0.45f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);
            if (Projectile.timeLeft > lifeLimit) 
                Projectile.timeLeft = lifeLimit;

            if (Projectile.ai [0] > 6f) {
                if (Main.netMode != NetmodeID.Server) {
                    if (Main.rand.NextBool(2)) {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f);
                        Main.dust [dust].noGravity = true;
                        Main.dust [dust].velocity *= 2.5f;
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                        Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 1.5f);
                    }
                }
            }
            else Projectile.ai [0] += 1f;
            return;
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPvp (Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override bool OnTileCollide (Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
    }
}