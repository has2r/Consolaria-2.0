﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Consolaria.Content.Projectiles.Friendly {
    public class OcramEyeLaser : ModProjectile {

        public override void SetStaticDefaults () {

            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.EyeLaser;

            Projectile.width = Projectile.height = 6;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;

            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.timeLeft = 900;
            Projectile.penetrate = -1;

            Projectile.light = 0.1f;
            Projectile.extraUpdates = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI () {
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(10) && Projectile.timeLeft <= 895) {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, 0, 0, 100, default, 1.1f);
                    Main.dust [dust].velocity = -Projectile.velocity * 0.3f;
                    Main.dust[dust].fadeIn = Main.rand.NextFloat(0.2f, 0.6f);
                }
            }
            if (Projectile.timeLeft <= 890) Projectile.alpha = 50;
            Lighting.AddLight(Projectile.Center, 0.4f, 0.1f, 0.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(100 - k * 5, 30, 80 + k * 4, 40 + k * 4);
                float rotation = (float)Math.Atan2(Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y, Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X);
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, (Projectile.scale - k / (float)Projectile.oldPos.Length) * 0.75f, effects, 0f);
                spriteBatch.Draw(texture, drawPos - Projectile.oldPos[k] * 0.5f + Projectile.oldPos[k + 1] * 0.5f, null, color, rotation, drawOrigin, (Projectile.scale - k / (float)Projectile.oldPos.Length) * 0.75f, effects, 0f);
            }
            return true;
        }

        public override void OnKill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 4; i++) {
                    int num506 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
                    Main.dust [num506].noGravity = true;
                    Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
                }
            }
        }

        public override Color? GetAlpha (Color lightColor)
            => new Color(255, 255, 255, 200);
    }
}