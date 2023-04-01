using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Consolaria.Content.Projectiles.Enemies {
    public class OcramLaser1 : ModProjectile {

        public override string Name => "Ocram Laser";

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.EyeLaser;

            Projectile.hostile = true;
            Projectile.tileCollide = false;

            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.width = 6;

            Projectile.timeLeft = 900;
            Projectile.penetrate = -1;

            Projectile.light = 0.1f;
        }

        public override void AI () {
            if (Projectile.timeLeft <= 895) Projectile.alpha = 50;
            Lighting.AddLight(Projectile.Center, 0.6f, 0.1f, 0.1f);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (Projectile.timeLeft < 890) {
                for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                    Vector2 drawPos = Projectile.oldPos [k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                    Color color = new Color(60 + k * 4, 20 - k, 10 + k, 60 + k * 4);
                    float rotation = (float) Math.Atan2(Projectile.oldPos [k].Y - Projectile.oldPos [k + 1].Y, Projectile.oldPos [k].X - Projectile.oldPos [k + 1].X);
                    spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, (Projectile.scale - k / (float) Projectile.oldPos.Length) * 0.75f, effects, 0f);
                    spriteBatch.Draw(texture, drawPos - Projectile.oldPos [k] * 0.5f + Projectile.oldPos [k + 1] * 0.5f, null, color, rotation, drawOrigin, (Projectile.scale - k / (float) Projectile.oldPos.Length) * 0.75f, effects, 0f);
                }
            }
            return true;
        }

        public override Color? GetAlpha (Color lightColor)
            => new Color(255, 255, 255, 200);
    }
}