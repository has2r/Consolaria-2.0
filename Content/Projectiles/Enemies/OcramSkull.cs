using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class OcramSkull : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 5;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.Skull);
            AIType = ProjectileID.Skull;

            int width = 12; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.penetrate = 1;

            Projectile.light = 0.1f;
        }

        public override void AI () {
            if (++Projectile.frameCounter >= 8) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames [Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.netMode != NetmodeID.Server) {
                for (int dustCount = 0; dustCount < 5; dustCount++) {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1.75f);
                    Main.dust [dust].noGravity = true;
                    Main.dust [dust].velocity = Projectile.Center - Main.dust [dust].position;
                    Main.dust [dust].velocity.Normalize();
                    Main.dust [dust].velocity *= -5f;
                    Main.dust [dust].velocity += Projectile.velocity / 2f;
                }
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.4f, 0.9f);
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo info)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
            }
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
        }

        public override Color? GetAlpha (Color lightColor)
          => new Color(255, 255, 255, 200);
    }
}