using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.Audio;
using Consolaria.Content.Projectiles.Enemies;
using Consolaria.Content.Items.BossDrops.Lepus;
using Consolaria.Common;
using Terraria.GameContent.ItemDropRules;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.NPCs.Lepus
{
    [AutoloadBossHead]
    public class Lepus : ModNPC
    {
       //private int _jumpHeight;
       //private int _animationTimer = 0;

        private int attacknum = 0;
        private int frame = 4;
        private int timer = 0;
        private int timer2 = 0;
        private int jumpnum = 0;

        private bool roar = false;

        private float posX = 0f;
        private float posY = 0f;
        private int velY = -1;

        Vector2 center;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lepus");
            Main.npcFrameCount[Type] = 7;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { 
                Velocity = 1f 
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() {
            int width = 90; int height = 76;
            NPC.Size = new Vector2(width, height);

            NPC.lifeMax = 3500;
            NPC.damage = 40;

            NPC.defense = 8;
            NPC.knockBackResist = 0f;

            NPC.value = Item.buyPrice(gold: 4);

            NPC.aiStyle = -1;
            AnimationType = -1;
            NPC.boss = true;
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.SpawnWithHigherTime(30);
            NPC.timeLeft = NPC.activeTime * 30;

            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Lepus");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale + numPlayers);
            NPC.damage = (int)(NPC.damage * 0.6f);
            NPC.defense = (int)(NPC.defense + numPlayers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("A hare of incredible size, capable of producing weak copies of itself as quickly as chewing the unfortunate Terrarian with its huge teeth.")
            });
        }

        private Rectangle GetFrame(int number) 
         => new Rectangle(0, NPC.frame.Height * (number - 1), NPC.frame.Width, NPC.frame.Height);
            
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
            => target.AddBuff(32, 120);

        // old AI
        /*  public override void AI() {
              Player player = Main.player[NPC.target];

              NPC.spriteDirection = NPC.direction;
              NPC.TargetClosest(true);
              NPC.netUpdate = true;
              _animationTimer++;
              _jumpHeight = Main.rand.Next(2, 8);

              if (!player.active || player.dead) {
                  NPC.noTileCollide = true;
                  NPC.TargetClosest(false);
                  NPC.velocity.Y = -20;
                  if (NPC.timeLeft > 10) NPC.timeLeft = 10;               
              }

              if (NPC.velocity.Y < 0) NPC.frame = GetFrame(5);         
              if (NPC.velocity.Y > 0) NPC.frame = GetFrame(6);
              if (NPC.velocity.Y == 0) {
                  if (_animationTimer % 10 == 0)
                      NPC.frame = GetFrame(Main.rand.Next(1, 3));
              } 

              NPC.localAI[0]++;
              if (NPC.localAI[0] % 300 == 0) {
                  AnimationType = -1;
                  NPC.frame = GetFrame(7);
                  int Type = ModContent.NPCType<SmallEgg>();
                  switch (Main.rand.Next(5)) {
                      case 0:
                          Type = ModContent.NPCType<SmallEgg>();
                          break;
                      case 1:
                          Type = ModContent.NPCType<SmallEgg>();
                          break;
                      case 2:
                          Type = ModContent.NPCType<SmallEgg>();
                          break;
                      case 3:
                          Type = ModContent.NPCType<SmallEgg>();
                          break;
                      case 4:
                          if (NPC.CountNPCS(ModContent.NPCType<Lepus>()) <= 4) Type = ModContent.NPCType<BigEgg>();                      
                          break;

                      default:
                          break;
                  }
                  NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)NPC.position.X, (int)NPC.position.Y + 35, Type);
              }

              if (NPC.localAI[0] >= 330) {
                  AnimationType = 50;
                  NPC.localAI[0] = 0;
              }
              if (NPC.Center.X < Main.player[NPC.target].Center.X - 2f) NPC.direction = 1;           
              if (NPC.Center.X > Main.player[NPC.target].Center.X + 2f) NPC.direction = -1;

              NPC.spriteDirection = NPC.direction;
              if (NPC.ai[0] == 0f) {
                  NPC.noTileCollide = false;               
                  if (NPC.velocity.Y == 0f) {
                      NPC.velocity.X = NPC.velocity.X * 0.5f;
                      NPC.ai[1] += 1f;
                      if (NPC.ai[1] > 0f) {
                          if (NPC.life < NPC.lifeMax) NPC.ai[1] += 0.25f;                        
                          if (NPC.life < NPC.lifeMax / 2) NPC.ai[1] += 0.5f;                       
                      }
                      if (NPC.ai[1] >= 50f) NPC.ai[1] = -20f;               
                      else if (NPC.ai[1] == -1f) {
                          NPC.TargetClosest(true);
                          NPC.velocity.X = NPC.velocity.X + 2f * NPC.direction;
                          NPC.velocity.Y = -18 + _jumpHeight;
                          NPC.ai[0] = 1f;
                          NPC.ai[1] = 0f;
                      }                                
                  }
              }

              int _extraDustCount;
              if (NPC.ai[0] == 1f && NPC.ai[0] <= 1f) { //Ground Pound
                  if (NPC.velocity.Y == 0f) {
                      if ((Main.expertMode || Main.masterMode) && Main.rand.Next(4) == 0) {
                          Vector2 velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
                          Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.position.X + 1, NPC.position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LepusStomp>(), 15, 0, 0);
                          Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.position.X - 1, NPC.position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LepusStomp>(), 15, 0, 0);
                          SoundEngine.PlaySound(SoundID.Item14, NPC.position);

                          NPC.ai[0] = 0f;
                          for (int _dustCount = (int)NPC.position.X - 20; _dustCount < (int)NPC.position.X + NPC.width + 40; _dustCount += 20) {
                              for (int i = 0; i < 4; i = _extraDustCount + 1) {
                                  int _dust = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, 31, 0f, 0f, 100, default(Color), 1.5f);
                                  Dust _dust2 = Main.dust[_dust];
                                  _dust2.velocity *= 0.2f;
                                  _extraDustCount = i;
                              }
                              int _cloudCount = Gore.NewGore(new Vector2((_dustCount - 20), NPC.position.Y + NPC.height - 8f), default(Vector2), Main.rand.Next(61, 64), 1f);
                              Gore gore = Main.gore[_cloudCount];
                              gore.velocity *= 0.4f;
                          }
                      }
                      if (!Main.expertMode && !Main.masterMode && Main.rand.Next(4) == 0) {
                          Vector2 velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
                          Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.position.X + 1, NPC.position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LilLepusStomp>(), 15, 0f, 0);
                          Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.position.X - 1, NPC.position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LilLepusStomp>(), 15, 0f, 0);
                          SoundEngine.PlaySound(SoundID.Item14, NPC.position);

                          NPC.ai[0] = 0f;
                          for (int _dustCount2 = (int)NPC.position.X - 20; _dustCount2 < (int)NPC.position.X + NPC.width + 40; _dustCount2 += 20) {
                              for (int d = 0; d < 4; d = _extraDustCount + 1) {
                                  int _dust3 = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
                                  Dust _dust4 = Main.dust[_dust3];
                                  _dust4.velocity *= 0.2f;
                                  _extraDustCount = d;
                              }
                              int num626 = Gore.NewGore(new Vector2((_dustCount2 - 20), NPC.position.Y + NPC.height - 8f), default(Vector2), Main.rand.Next(61, 63), 1f);
                              Gore gore = Main.gore[num626];
                              gore.velocity *= 0.3f;
                          }
                      }
                      else NPC.ai[0] = 0f;
                  }                  
                  else {
                      NPC.TargetClosest(true);
                      if (NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width) {
                          NPC.velocity.X = NPC.velocity.X * 0.5f;
                          NPC.velocity.Y = NPC.velocity.Y + 0.5f;
                      }
                      else {
                          if (NPC.direction < 0) NPC.velocity.X = NPC.velocity.X - 0.2f;                        
                          else if (NPC.direction > 0) NPC.velocity.X = NPC.velocity.X + 0.2f;

                          float _jumpBoost = 2f;
                          if (NPC.life < NPC.lifeMax) _jumpBoost += 1f;                  
                          if (NPC.velocity.X < -_jumpBoost) NPC.velocity.X = -_jumpBoost;                        
                          if (NPC.velocity.X > _jumpBoost) NPC.velocity.X = _jumpBoost;                      
                      }
                  }
              }

              int _targetDistance = 3000;
              if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > _targetDistance) {
                  NPC.TargetClosest(true);
                  if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) + Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > _targetDistance) {
                      NPC.active = false;
                      return;
                  }
              }
          }*/

        public override void AI() {
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = NPC.direction;

            for (int i = NPC.oldPos.Length - 1; i > 0; i--)
                NPC.oldPos[i] = NPC.oldPos[i - 1];
            NPC.oldPos[0] = NPC.position;

            if (player.dead) {
                NPC.TargetClosest(false);
                timer = 0;
                NPC.noTileCollide = true;
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }

            NPC.TargetClosest(true);
            NPC.direction = player.Center.X < NPC.Center.X ? -1 : 1;

            timer++;
            if ((timer <= 80 || timer >= 100) && timer < 120) {
                NPC.noTileCollide = false;
                if (NPC.velocity.Y == 0) {
                    NPC.frame = GetFrame(frame);
                    if (timer % 6 == 0) frame++;                 
                    if (frame >= 4) frame = 1;       
                }

                if (NPC.velocity.Y < 0) NPC.frame = GetFrame(5);              
                if (NPC.velocity.Y > 0) NPC.frame = GetFrame(6);   
            }

            if (!(timer >= 180 && timer <= 200))
                NPC.rotation = NPC.velocity.Y / 25f;
            
            if (timer >= 120 && timer <= 180) {
                if (NPC.velocity.Y <= 0) {
                    NPC.frame = GetFrame(5);
                    NPC.noTileCollide = true;
                }
                else  {
                    NPC.frame = GetFrame(6);
                    NPC.noTileCollide = false;
                }
            }

            if (timer >= 80 && !roar) {
                SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y, 10);
                NPC.frame = GetFrame(7);
                roar = true;
            }
            if (timer >= 120 && timer <= 140) {
                timer = 120;
                if (jumpnum < 4) {
                    if (NPC.velocity.Y == 0) {
                        if (player.position.Y > NPC.position.Y + NPC.height) {
                            NPC.velocity.Y *= 0.36f;
                            NPC.velocity.X *= 0.36f;
                        }
                        else {
                            NPC.velocity.X = 7 * NPC.direction;
                            SoundEngine.PlaySound(16, (int)NPC.position.X, (int)NPC.position.Y, 0, 1f, 0f);
                            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
                            float rotation = (float)Math.Atan2((vector8.Y) - (player.position.Y - 220 + (player.height * 0.5f)), (vector8.X) - (player.position.X + (player.width * 0.5f)));
                            NPC.velocity.Y = (float)(Math.Sin(rotation) * 14) * -1;
                            jumpnum++;
                        }
                    }
                }
                else  {
                    NPC.rotation = 0f;
                    if (attacknum >= 2) {
                        center = Main.player[NPC.target].Center;
                        center.Y -= 300f;
                        posX = Main.player[NPC.target].Center.X;
                        posY = Main.player[NPC.target].Center.Y;

                        NPC.velocity.X = 0;
                        timer = 180;
                    }
                    else {
                        if (NPC.velocity.Y == 0) {
                            timer = 0;
                            roar = false;
                            attacknum++;
                            jumpnum = 0;
                            NPC.velocity = Vector2.Zero;
                            NPC.noTileCollide = false;
                        }
                    }
                }
            }

            if (timer >= 180 && timer <= 200) {
                timer = 190;
                timer2++;
                if (timer2 <= 26) {
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    if (NPC.localAI[0] == 0) {
                        NPC.velocity = center - NPC.Center;
                        NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero);
                        NPC.velocity *= 16f;
                        NPC.frame = GetFrame(5);
                    }
                }

                Vector2 vector101 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num855 = posX - vector101.X;
                float num856 = posY - 300 - vector101.Y;
                float num857 = (float)Math.Sqrt((double)(num855 * num855 + num856 * num856));

                if (NPC.localAI[0] == 0 && num857 <= 120 || timer2 >= 26 && NPC.localAI[0] == 0) {
                    NPC.localAI[0] = 1;
                    NPC.velocity *= 0f;
                }
                if (NPC.localAI[0] == 1) NPC.localAI[1]++;               
                if (NPC.localAI[0] == 1 && NPC.localAI[1] >= 8) {
                    NPC.frame = GetFrame(6);
                    if (velY <= 14) velY++;                   
                    NPC.velocity.Y = velY;

                    if (Main.player[NPC.target].position.Y <= NPC.position.Y) {
                        if (Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].HasTile || Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
                            NPC.frame = GetFrame(3);
                            timer = 200;
                            NPC.velocity.Y = -8f;
                            NPC.velocity.X = NPC.velocity.X + 6.5f * -(float)NPC.direction;
                            NPC.noGravity = false;
                            for (int m = 0; m < 6; m++) {
                                Vector2 vector2 = new Vector2(13f, 0f);
                                vector2 = vector2.RotatedBy((double)((float)(-(float)m) * 6.28318548f / 10f), Vector2.Zero);
                                int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SmallEgg>());
                                Main.npc[npc].velocity.X = vector2.X;
                                Main.npc[npc].velocity.Y = vector2.Y;
                                SoundEngine.PlaySound(SoundID.Item7, NPC.position);
                            }
                        }
                    }
                }
            }

            if (timer >= 210 && timer <= 220) {
                if (!(Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].HasTile || Collision.SolidCollision(NPC.position, NPC.width, NPC.height))) timer = 210; 
                else {
                    NPC.velocity = Vector2.Zero;
                    NPC.noTileCollide = false;
                    SoundEngine.PlaySound(SoundID.Item56, NPC.position);
                    timer = 220;
                }
            }

            if (timer >= 240 && timer <= 240) {
                NPC.frame = GetFrame(7);
                SoundEngine.PlaySound(2, (int)NPC.position.X, (int)NPC.position.Y, 44, 1f, -0.6f);
                int bigegg = 0;
                bigegg = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 30, ModContent.NPCType<BigEgg>());
                Main.npc[bigegg].ai[0] = NPC.whoAmI;
            }

            if (timer >= 260) {
                velY = -1;
                attacknum = -1;
                roar = false;
                NPC.localAI[1] = 0;
                NPC.localAI[0] = 0;
                jumpnum = 0;
                NPC.noTileCollide = false;
                timer = 0;
                timer2 = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Color color = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
            Color color2 = color;
            color2 = Color.HotPink;

            float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + .8f;
            if (timer >= 180 && timer <= 200) {
                Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, scale, effects, 0f);
                for (int i = 1; i < NPC.oldPos.Length; i++) {
                    Color color3 = color2;
                    color3 = NPC.GetAlpha(color2);
                    color3 *= (float)(NPC.oldPos.Length - i) / 15f;
                    Main.spriteBatch.Draw(texture, new Vector2(NPC.oldPos[i].X - Main.screenPosition.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.oldPos[i].Y - Main.screenPosition.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color3, NPC.rotation, origin, scale, effects, 0f);
                }
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0) {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG2").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG3").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG4").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG5").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG6").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/LPG7").Type);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center, 0);
            }
        }

        public override void OnKill()
            => NPC.SetEventFlagCleared(ref DownedBossSystem.downedLepus, -1);

        public override void BossLoot(ref string name, ref int potionType) {
            if (NPC.CountNPCS(ModContent.NPCType<Lepus>()) == 1) 
                potionType = ItemID.LesserHealingPotion;  
            else potionType = 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            if (NPC.CountNPCS(ModContent.NPCType<Lepus>()) == 1) {         
                Conditions.NotExpert notExpert = new Conditions.NotExpert();
                npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LepusBag>()));
                //npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LepusRelic>()));
                //npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LepusPet>()));
                npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SuspiciousLookingEgg>()));
                //npcLoot.Add(ItemDropRule.ByCondition(_notExpert, ModContent.ItemType<OstaraHat>(), 3));
                //npcLoot.Add(ItemDropRule.ByCondition(_notExpert, ModContent.ItemType<OstaraChainmail>(), 3));
                //npcLoot.Add(ItemDropRule.ByCondition(_notExpert, ModContent.ItemType<OstaraBoots>(), 3));
                npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<EggCannon>(), 2));
                npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<LepusMask>(), 7));
                npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<LepusTrophy>(), 10));
                npcLoot.Add(ItemDropRule.ByCondition(notExpert, ItemID.BunnyHood, 10));
            }
        }
    }
}