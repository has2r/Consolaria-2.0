using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class SpectralBomb : ModProjectile {
        private float runeRotation;

        public override void SetDefaults () {
            int width = 90; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
            Projectile.timeLeft = 16;

            Projectile.light = 0.1f;
            Projectile.alpha = 255;
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 origin = new(texture.Width * 0.5f, texture.Height * 0.5f);
            runeRotation++;
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), Color.White * 0.8f, runeRotation / 2, origin, 1f, SpriteEffects.None, 0f);
            return true;
        }
        public override Color? GetAlpha (Color lightColor)
            => Color.White * 0.8f;

        public override void OnKill (int timeLeft) {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);
            if (Main.netMode != NetmodeID.Server) {
                int radius = 10;
                for (int x = -radius; x <= radius; x++) {
                    for (int y = -radius; y <= radius; y++) {
                        int xPosition = (int) (x + position.X / 16.0f);
                        int yPosition = (int) (y + position.Y / 16.0f);

                        if (Math.Sqrt(x * x + y * y) <= radius + 0.5) {
                            int dust = Dust.NewDust(new Vector2(xPosition, yPosition), 20, 20, DustID.BlueFairy, 0f, 0f, 0, default, 1.5f);
                            Main.dust [dust].noGravity = true;
                        }
                    }
                }
            }
        }
    }
}