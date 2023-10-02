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

        private float rot;
        private float distance;
        private int cycle;
        private bool cycleSwitch;

        private bool useCycleColor = true;

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            int width = 8; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.scale = 0.5f;
            Projectile.timeLeft = 1800;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];

            rot += 0.025f;
            if (rot >= Math.PI * 2f) rot -= (float)Math.PI * 2f;

            if (Projectile.timeLeft < 20 && distance > 0) distance -= 10;
            else if (distance < player.GetModPlayer<SpectralPlayer>().absorptionRadius) distance += 5;

            Projectile.position = player.MountedCenter + new Vector2(0, distance).RotatedBy(rot + Math.PI * 2 / 3 * Projectile.ai[0]);

            if (!cycleSwitch) cycle++;
            else cycle--;
            if (cycle >= 80) cycleSwitch = true;
            if (cycle <= 0) cycleSwitch = false;

            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(25)) {
                    int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.RainbowMk2, 0, 0, 120, new Color(240 - cycle * 2, 225 - cycle * 2, cycle * 3, 50), 0.8f);
                    Main.dust [dust].position = Projectile.Center;
                    Main.dust [dust].velocity *= 0.25f;
                    Main.dust [dust].noLightEmittence = true;
                    Main.dust [dust].noGravity = true;
                }
            }

            if (!player.active || player.dead) {
                Projectile.Kill();
            }
        }

        public override void OnKill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int dustCount = 16; dustCount > 0; --dustCount) {
                    Vector2 velocity = Projectile.velocity;
                    if (Main.rand.NextBool(2)) {
                        int dust = Dust.NewDust(Projectile.position, 2, 2, DustID.GoldFlame, 0.0f, 0.0f, 75, default, 2.5f);
                        Main.dust [dust].velocity = velocity.RotatedBy(15 * (dustCount + 5), new Vector2());
                        Main.dust [dust].noGravity = true;
                        Main.dust [dust].noLightEmittence = true;
                    }
                }
            }
        }

        public override void PostDraw (Color lightColor) {
            Player player = Main.player[Projectile.owner];
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            int length = 40;
            for (int k = 0; k < length; k++) {
                Vector2 drawPos = player.MountedCenter + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Vector2 drawPosNew = drawPos + new Vector2(0, distance).RotatedBy(rot + Math.PI * 2 / 3 * Projectile.ai[0] - k * 0.025f);
                Vector2 drawPosOld = drawPos + new Vector2(0, distance).RotatedBy(rot + Math.PI * 2 / 3 * Projectile.ai[0] - (k - 1) * 0.025f);
                Color color;
                if (useCycleColor) color = new Color(240 - cycle * 2, 225 - cycle * 2, cycle * 3, 50);
                else color = new Color(240 - k * 4, 225 - k * 4, k * 6, 50);
                float rotation = (float) Math.Atan2(drawPosNew.Y - drawPosOld.Y, drawPosNew.X - drawPosOld.X);
                spriteBatch.Draw(texture, drawPosNew, null, color * 0.3f, rotation, drawOrigin, Projectile.scale * (length - k) / length, effects, 0f);
            }
        }

        public override bool? CanCutTiles ()
           => false;

        public override bool? CanDamage ()
            => false;
    }
}