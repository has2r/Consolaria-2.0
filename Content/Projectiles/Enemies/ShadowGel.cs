using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.Audio;

namespace Consolaria.Content.Projectiles.Enemies
{
    public class ShadowGel : ModProjectile
    {
        public override void SetDefaults() {
            int width = 6; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;

            Projectile.alpha = 255;
        }

        public override void AI() {
            if (Projectile.alpha == 0) {
                int dust = Dust.NewDust(Projectile.oldPosition - Projectile.velocity * 3f, Projectile.width, Projectile.height, 50, 0f, 0f, 100, default, 1f);
                Main.dust[dust].noGravity = false;
                Main.dust[dust].noLight = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            Projectile.alpha -= 50;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            
            if (Projectile.ai[1] == 0f) {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 17);
            }
        }

        public override void Kill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item112, Projectile.Center);
            Vector2 position = Projectile.Center;
            int radius = 2;
            for (int i = 0; i < 20; i++) {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 1f, Projectile.Center.Y - 1f), 2, 2, 50, 0f, 0f, 100, default, 1.6f);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].noLight = true;
                Main.dust[dust].noGravity = false;
            }

            for (int j = 0; j < 20; j++) {
                int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X - 1f, Projectile.Center.Y - 1f), 2, 2, 50, 0f, 0f, 100, default, 2f);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].noLight = true;
                Main.dust[dust2].noGravity = false;
            }

            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    int xPosition = (int)(x + position.X / 16f);
                    int yPosition = (int)(y + position.Y / 16f);
                    if (Math.Sqrt((x * x + y * y)) <= radius + 0.5)
                        WorldGen.Convert(xPosition, yPosition, 1, 1);           
                }
            }
        }
    }
}
