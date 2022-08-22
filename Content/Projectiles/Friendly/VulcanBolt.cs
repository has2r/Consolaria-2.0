using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class VulcanBolt : ModProjectile {
        public override void SetDefaults () {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.OnFire3, 300);

        public override void OnHitPvp (Player target, int damage, bool crit)
            => target.AddBuff(BuffID.OnFire3, 300);

        public override void AI () {
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(2))
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 0, default, 1.2f);
            }
        }

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                int radius = 5;
                for (int x = -radius; x <= radius; x++) {
                    for (int y = -radius; y <= radius; y++) {
                        if (Math.Sqrt(x * x + y * y) <= radius + 0.5) {
                            int dust = Dust.NewDust(Projectile.Center, 20, 20, DustID.Smoke, 0.0f, 0.0f, 120, default, 1f);
                            int dust2 = Dust.NewDust(Projectile.Center, 20, 20, DustID.Torch, 0.0f, 0.0f, 100, default, 1.2f);
                            Main.dust [dust2].noGravity = true;
                            Main.dust [dust2].velocity = Main.dust [dust].velocity;
                        }
                    }
                }
            }
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.75f, MaxInstances = 3 }, Projectile.Center);
        }
    }
}