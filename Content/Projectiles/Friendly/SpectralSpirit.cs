using Consolaria.Content.Items.Armor.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpectralSpirit : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private bool changeAlpha;

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            int width = 8; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.scale = 0.5f;

            Projectile.timeLeft = 1800;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            double deg = (double) (Projectile.ai [1] + Projectile.ai [0] * 180) / 3;
            double rad = deg * (Math.PI / 180);
            double dist = 100;
            Projectile.position.X = player.MountedCenter.X - (int) (Math.Cos(rad) * dist) - player.width / 2;
            Projectile.position.Y = player.MountedCenter.Y - (int) (Math.Sin(rad) * dist) - player.height / 2 + 4 + player.gfxOffY;
            Projectile.ai [1] += 5f * player.direction;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (!player.active || player.dead) {
                Projectile.Kill();
            }
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

        public override bool? CanCutTiles ()
           => false;

        public override bool? CanDamage ()
            => false;
    }
}