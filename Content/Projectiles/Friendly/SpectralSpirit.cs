using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpectralSpirit : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 1;
        }

        public override void SetDefaults () {
            int width = 8; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.scale = 0.8f;
        }

        public override void AI () {
            float posX = Projectile.Center.X;
            float posY = Projectile.Center.Y;
            float maxDetectRadius = 1000f;
            bool flag = false;
            for (int _npc = 0; _npc < Main.maxNPCs; ++_npc) {
                if (Main.npc [_npc].CanBeChasedBy(Projectile, false) && Projectile.Distance(Main.npc [_npc].Center) < maxDetectRadius && Collision.CanHit(Projectile.Center, 1, 1, Main.npc [_npc].Center, 1, 1)) {
                    float npsPosX = Main.npc [_npc].position.X + (Main.npc [_npc].width / 2);
                    float npsPosY = Main.npc [_npc].position.Y + (Main.npc [_npc].height / 2);
                    float dist = Math.Abs(Projectile.position.X + (Projectile.width / 2) - npsPosX) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - npsPosY);
                    if (dist < maxDetectRadius) {
                        maxDetectRadius = dist;
                        posX = npsPosX;
                        posY = npsPosY;
                        flag = true;
                    }
                }
            }
            if (!flag) return;
            float homingSpeed = 12f;
            Vector2 vector2 = new(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
            float posX2 = posX - vector2.X;
            float posY2 = posY - vector2.Y;
            float vel = (float) Math.Sqrt(posX2 * (double) posX2 + posY2 * posY2);
            float vel2 = homingSpeed / vel;
            float velX = posX2 * vel2;
            float velY = posY2 * vel2;
            Projectile.velocity.X = (float) ((Projectile.velocity.X * 20.0 + velX) / 21.0);
            Projectile.velocity.Y = (float) ((Projectile.velocity.Y * 20.0 + velY) / 21.0);

            Projectile.rotation = (float) Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1f;
        }

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int dustCount = 16; dustCount > 0; --dustCount) {
                    Vector2 velocity = Projectile.velocity;
                    if (Main.rand.NextBool(2)) {
                        int dust2 = Dust.NewDust(Projectile.position, 2, 2, DustID.GoldFlame, 0.0f, 0.0f, 75, default, 2.5f);
                        Main.dust [dust2].velocity = velocity.RotatedBy(15 * (dustCount + 5), new Vector2());
                        Main.dust [dust2].noGravity = true;
                    }
                }
            }
        }

        public override void PostDraw (Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/Tonbogiri_Glow");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 drawPos = Projectile.oldPos [k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(240 - k * 5, 225 - k * 5, 60 + k * 10, 100);
                float rotation = (float) Math.Atan2(Projectile.oldPos [k].Y - Projectile.oldPos [k + 1].Y, Projectile.oldPos [k].X - Projectile.oldPos [k + 1].X);
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
            }
        }
    }
}