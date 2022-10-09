using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpicyExplosion : ModProjectile {
        public override void SetStaticDefaults () => Main.projFrames [Projectile.type] = 7;

        public override void SetDefaults () {
            int width = 90; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.tileCollide = false;

            Projectile.timeLeft = 420;
            Projectile.penetrate = -1;

            Projectile.rotation = Main.rand.NextFloat(0, (float) Math.PI * 2f);
            Projectile.alpha = 50;
        }

        public override void AI () {
            Projectile.alpha += 10;
            if (Main.netMode != NetmodeID.Server) {

                Vector2 position = new Vector2(24f, 24f);
                int _dust = Dust.NewDust(Projectile.Center - position / 2f, (int) position.X, (int) position.Y, DustID.Water_BloodMoon, 0f, -2f, 100, Color.Brown, 1f);
                Main.dust [_dust].velocity *= 2f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames [Projectile.type])
                Projectile.Kill();
        }
    }
}