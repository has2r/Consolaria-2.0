using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using Terraria.Audio;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class HolyHandgrenade : ModProjectile
    {
        public override void SetDefaults() {
            int width = 24; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Generic;

            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 420;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (Main.expertMode) {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) damage /= 5;        
            }
        }

        public override void Kill(int timeLeft) {
            Vector2 position = Projectile.Center;

            SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Assets/Sounds/Items/Hallelujah"), Projectile.position);

            for (int i = 0; i < 50; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 80; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }

            for (int g = 0; g < 2; g++) {
                int goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

            int explosionRadius = 15;
            int minTileX = (int)(Projectile.position.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(Projectile.position.X / 16f + (float)explosionRadius);
            int minTileY = (int)(Projectile.position.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(Projectile.position.Y / 16f + (float)explosionRadius);
            if (minTileX < 0) minTileX = 0;      
            if (maxTileX > Main.maxTilesX) maxTileX = Main.maxTilesX;          
            if (minTileY < 0) minTileY = 0;         
            if (maxTileY > Main.maxTilesY) maxTileY = Main.maxTilesY;
            
            bool canKillWalls = false;
            for (int x = minTileX; x <= maxTileX; x++) {
                for (int y = minTileY; y <= maxTileY; y++) {
                    float diffX = Math.Abs((float)x - Projectile.position.X / 16f);
                    float diffY = Math.Abs((float)y - Projectile.position.Y / 16f);
                    double distance = Math.Sqrt((diffX * diffX + diffY * diffY));
                    if (distance < explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0) {
                        canKillWalls = true;
                        break;
                    }
                }
            }

            AchievementsHelper.CurrentlyMining = true;
            for (int i = minTileX; i <= maxTileX; i++) {
                for (int j = minTileY; j <= maxTileY; j++) {
                    float diffX = Math.Abs((float)i - Projectile.position.X / 16f);
                    float diffY = Math.Abs((float)j - Projectile.position.Y / 16f);
                    double distanceToTile = Math.Sqrt((diffX * diffX + diffY * diffY));
                    if (distanceToTile < explosionRadius) {
                        bool canKillTile = true;
                        if (Main.tile[i, j] != null && Main.tile[i, j].IsActive) {
                            canKillTile = true;
                            if (Main.tileDungeon[Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404) 
                                canKillTile = false;                       
                            if (!Main.hardMode && Main.tile[i, j].type == 58) canKillTile = false;                        
                            if (!TileLoader.CanExplode(i, j)) canKillTile = false;                           
                            if (canKillTile) {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile[i, j].IsActive && Main.netMode != NetmodeID.SinglePlayer)
                                    NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);                             
                            }
                        }
                        if (canKillTile) {
                            for (int x = i - 1; x <= i + 1; x++) {
                                for (int y = j - 1; y <= j + 1; y++) {
                                    if (Main.tile[x, y] != null && Main.tile[x, y].wall > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].wall)) {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile[x, y].wall == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                            NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);                                      
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;
        }
    }
}
