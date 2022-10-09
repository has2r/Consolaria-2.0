using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.Projectiles.Friendly {
    public class HolyHandgrenade2 : ModProjectile {
        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            int width = 12; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Generic;

            Projectile.aiStyle = 16;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 420;

            DrawOriginOffsetY = -6;
        }

        public override void ModifyHitNPC (NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (Main.expertMode) {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) damage /= 5;
            }
        }

        public override void PostAI () {
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
                Projectile.tileCollide = false;
                Projectile.alpha = 255;
                Projectile.position = Projectile.Center;
                Projectile.width = 600;
                Projectile.height = 600;
                Projectile.Center = Projectile.position;
                Projectile.damage = 600;
                Projectile.knockBack = 16f;
            }
        }

        public override void Kill (int timeLeft) {
            Vector2 position = Projectile.Center;

            SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/Hallelujah") { Volume = 0.5f, Pitch = -0.5f }, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 1.75f, Pitch = -2.5f }, Projectile.position);
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 50; i++) {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                    Main.dust [dustIndex].velocity *= 1.4f;
                }
                for (int i = 0; i < 80; i++) {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 3f);
                    Main.dust [dustIndex].noGravity = true;
                    Main.dust [dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 2f);
                    Main.dust [dustIndex].velocity *= 3f;
                }
                for (int g = 0; g < 2; g++) {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore [goreIndex].scale = 1.5f;
                    Main.gore [goreIndex].velocity.X = Main.gore [goreIndex].velocity.X + 1.5f;
                    Main.gore [goreIndex].velocity.Y = Main.gore [goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore [goreIndex].scale = 1.5f;
                    Main.gore [goreIndex].velocity.X = Main.gore [goreIndex].velocity.X - 1.5f;
                    Main.gore [goreIndex].velocity.Y = Main.gore [goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore [goreIndex].scale = 1.5f;
                    Main.gore [goreIndex].velocity.X = Main.gore [goreIndex].velocity.X + 1.5f;
                    Main.gore [goreIndex].velocity.Y = Main.gore [goreIndex].velocity.Y - 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default, Main.rand.Next(61, 64), 1f);
                    Main.gore [goreIndex].scale = 1.5f;
                    Main.gore [goreIndex].velocity.X = Main.gore [goreIndex].velocity.X - 1.5f;
                    Main.gore [goreIndex].velocity.Y = Main.gore [goreIndex].velocity.Y - 1.5f;
                }
            }

            Projectile.position.X = Projectile.position.X + Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;

            int explosionRadius = 15;
            int minTileX = (int) (Projectile.position.X / 16f - explosionRadius);
            int maxTileX = (int) (Projectile.position.X / 16f + explosionRadius);
            int minTileY = (int) (Projectile.position.Y / 16f - explosionRadius);
            int maxTileY = (int) (Projectile.position.Y / 16f + explosionRadius);
            if (minTileX < 0) minTileX = 0;
            if (maxTileX > Main.maxTilesX) maxTileX = Main.maxTilesX;
            if (minTileY < 0) minTileY = 0;
            if (maxTileY > Main.maxTilesY) maxTileY = Main.maxTilesY;

            bool canKillWalls = false;
            for (int x = minTileX; x <= maxTileX; x++) {
                for (int y = minTileY; y <= maxTileY; y++) {
                    float diffX = Math.Abs(x - Projectile.position.X / 16f);
                    float diffY = Math.Abs(y - Projectile.position.Y / 16f);
                    double distance = Math.Sqrt((diffX * diffX + diffY * diffY));
                    if (distance < explosionRadius && Main.tile [x, y] != null && Main.tile [x, y].WallType == 0) {
                        canKillWalls = true;
                        break;
                    }
                }
            }

            AchievementsHelper.CurrentlyMining = true;
            for (int i = minTileX; i <= maxTileX; i++) {
                for (int j = minTileY; j <= maxTileY; j++) {
                    float diffX = Math.Abs(i - Projectile.position.X / 16f);
                    float diffY = Math.Abs(j - Projectile.position.Y / 16f);
                    double distanceToTile = Math.Sqrt((diffX * diffX + diffY * diffY));
                    if (distanceToTile < explosionRadius) {
                        bool canKillTile = true;
                        if (Main.tile [i, j] != null && Main.tile [i, j].HasTile) {
                            canKillTile = true;
                            if (Main.tileDungeon [Main.tile [i, j].TileType] || Main.tile [i, j].TileType == 88 || Main.tile [i, j].TileType == 21 || Main.tile [i, j].TileType == 26 || Main.tile [i, j].TileType == 107 || Main.tile [i, j].TileType == 108 || Main.tile [i, j].TileType == 111 || Main.tile [i, j].TileType == 226 || Main.tile [i, j].TileType == 237 || Main.tile [i, j].TileType == 221 || Main.tile [i, j].TileType == 222 || Main.tile [i, j].TileType == 223 || Main.tile [i, j].TileType == 211 || Main.tile [i, j].TileType == 404)
                                canKillTile = false;
                            if (!Main.hardMode && Main.tile [i, j].TileType == 58) canKillTile = false;
                            if (!TileLoader.CanExplode(i, j)) canKillTile = false;
                            if (canKillTile) {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile [i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                            }
                        }
                        if (canKillTile) {
                            for (int x = i - 1; x <= i + 1; x++) {
                                for (int y = j - 1; y <= j + 1; y++) {
                                    if (Main.tile [x, y] != null && Main.tile [x, y].WallType > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile [x, y].WallType)) {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile [x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, x, y, 0f, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;

            Player player = Main.player [Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.oldPosition, Projectile.velocity + new Vector2(Main.rand.NextFloat(-20f, 20f), -20f), ModContent.ProjectileType<HolyHandgrenade2>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 drawPos = Projectile.oldPos [k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(200 - k * 10, 200 - k * 10, 100 - k * 4, 50);
                float rotation = (float) Math.Atan2(Projectile.oldPos [k].Y - Projectile.oldPos [k + 1].Y, Projectile.oldPos [k].X - Projectile.oldPos [k + 1].X);
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
                if (k == 1) spriteBatch.Draw(texture, drawPos, null, color, rotation + (float) Math.PI / 2, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
            }
            return true;
        }
    }
}