using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class SpectralBomb : ModProjectile {
        private float runeRotation;
        private float fadeIn;

        public override void SetDefaults () {
            int width = 90; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 16;

            Projectile.light = 0.1f;
            Projectile.alpha = 255;
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 origin = new(texture.Width * 0.5f, texture.Height * 0.5f);
            runeRotation++;
            if (fadeIn < 0.8f)
                fadeIn += 0.1f;
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), Color.White * fadeIn, runeRotation / 4, origin, 1.8f - fadeIn, SpriteEffects.None, 0f);
            return false;
        }
        public override Color? GetAlpha (Color lightColor)
            => Color.White * 0.8f;

        public override void OnKill (int timeLeft) {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);
            if (Main.netMode != NetmodeID.Server) {
                int radius = 10;
                for (int g = 0; g < 2; g++)
                {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                }
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