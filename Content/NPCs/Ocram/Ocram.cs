using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.Audio;
using Consolaria.Content.Projectiles.Enemies;
using Consolaria.Common;
using Terraria.GameContent.ItemDropRules;
using Consolaria.Content.Items.BossDrops.Ocram;
using Consolaria.Content.Items.Weapons.Summon;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Magic;
using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Weapons.Melee;

namespace Consolaria.Content.NPCs.Ocram {
    [AutoloadBossHead]
    public class Ocram : ModNPC {
        private int t = 0;
        private float h = 0.2f;
        private bool Phase2 = false;
        private const int MissileProjectiles = 5;
        private const float MissileAngleSpread = 150;
        private bool effect = false;
        private float rad = (float)Math.PI * 2f;
        private bool showeye = false;
        private Vector2 oldocrampos;
        private float glowopacity;

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Ocram");
            Main.npcFrameCount [NPC.type] = 6;
            NPCID.Sets.MPAllowedEnemies [Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f,
                Scale = 0.85f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 190; int height = 180;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;
            AnimationType = 126;

            NPC.lifeMax = 35000;
            NPC.damage = 50;

            NPC.defense = 20;
            NPC.knockBackResist = 0f;

            NPC.value = Item.buyPrice(gold: 10);
            NPC.npcSlots = 1f;

            NPC.boss = true;
            NPC.lavaImmune = true;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.timeLeft = NPC.activeTime * 30;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath18;

            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ocram");
        }

        public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
            NPC.lifeMax = 45000 + (int) (numPlayers > 1 ? NPC.lifeMax * 0.2 * numPlayers : 0);
            NPC.damage = (int) (NPC.damage * 0.65f);
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("This forgotten blighted demon, once the powerful Emperor of Darkness, now seeks revenge against all surface dwellers.")
            });
        }

        public override void AI () {
            Lighting.AddLight(NPC.Center, 0.6f, 0.4f, 0.5f);
            if (NPC.target < 0 || NPC.target == 255 || Main.player [NPC.target].dead || !Main.player [NPC.target].active) {
                NPC.TargetClosest(true);
            }
            if (Main.expertMode) {
                if (NPC.life < (int) (NPC.lifeMax * 0.65f)) {
                    Phase2 = true;
                }
            }

            if (!Main.expertMode) {
                if (NPC.life < (int) (NPC.lifeMax / 2)) {
                    Phase2 = true;
                }
            }

            bool dead2 = Main.player [NPC.target].dead;
            float num317 = NPC.position.X + (float) (NPC.width / 2) - Main.player [NPC.target].position.X - (float) (Main.player [NPC.target].width / 2);
            float num318 = NPC.position.Y + (float) NPC.height - 59f - Main.player [NPC.target].position.Y - (float) (Main.player [NPC.target].height / 2);
            float num319 = (float) Math.Atan2((double) num318, (double) num317) + 1.57f;

            if (num319 < 0f) {
                num319 += rad;
            }
            else {
                if (num319 > rad) {
                    num319 -= rad;
                }
            }
            float num320 = 0.1f;

            if (NPC.rotation < num319) {
                if ((double) (num319 - NPC.rotation) > 3.1) {
                    NPC.rotation -= num320;
                }
                else {
                    NPC.rotation += num320;
                }
            }
            else {
                if (NPC.rotation > num319) {
                    if ((double) (NPC.rotation - num319) > 3.1) {
                        NPC.rotation += num320;
                    }
                    else {
                        NPC.rotation -= num320;
                    }
                }
            }
            if (NPC.rotation > num319 - num320 && NPC.rotation < num319 + num320) {
                NPC.rotation = num319;
            }
            if (NPC.rotation < 0f) {
                NPC.rotation += rad;
            }
            else
            {
                if (NPC.rotation > rad) {
                    NPC.rotation -= rad;
                }
            }
            if (NPC.rotation > num319 - num320 && NPC.rotation < num319 + num320) {
                NPC.rotation = num319;
            }
            if (Main.rand.NextBool(5)) {
                int num321 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float) NPC.height * 0.25f), NPC.width, (int) ((float) NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f, 0, default(Color), 1f);
                Dust expr_146B6_cp_0 = Main.dust [num321];
                expr_146B6_cp_0.velocity.X = expr_146B6_cp_0.velocity.X * 0.5f;
                Dust expr_146D6_cp_0 = Main.dust [num321];
                expr_146D6_cp_0.velocity.Y = expr_146D6_cp_0.velocity.Y * 0.1f;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && !dead2 && NPC.timeLeft < 10) {
                for (int num845 = 0; num845 < 200; num845++) {
                    if (num845 != NPC.whoAmI && Main.npc [num845].active && Main.npc [num845].timeLeft - 1 > NPC.timeLeft) {
                        NPC.timeLeft = Main.npc [num845].timeLeft - 1;
                    }
                }
            }
            if (dead2 || Main.dayTime) {
                NPC.velocity.Y = NPC.velocity.Y - 0.04f;
                if (NPC.timeLeft > 10) {
                    NPC.timeLeft = 10;
                    return;
                }
            }
            else {
                if (NPC.ai [0] == 0f) {
                    if (NPC.ai [1] == 0f) {
                        float num322 = 12f;
                        float num323 = 0.2f;
                        int num324 = 1;
                        if (NPC.position.X + (float) (NPC.width / 2) < Main.player [NPC.target].position.X + (float) Main.player [NPC.target].width) {
                            num324 = -1;
                        }
                        Vector2 vector32 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num325 = Main.player [NPC.target].position.X + (Main.player [NPC.target].width / 2) - 300f - (num324 * 300) - vector32.X; //about 12 if boss is to the left and -12 if it is to the right
                        float num326 = Main.player [NPC.target].position.Y + (Main.player [NPC.target].height / 2) - 300f - vector32.Y;
                        float num327 = (float) Math.Sqrt((num325 * num325 + num326 * num326));
                        num327 = num322 / num327;
                        num325 *= num327;
                        num326 *= num327;

                        //adjust velocity.x and y between 12 and -12 depending on player position

                        if (NPC.velocity.X < num325) {
                            NPC.velocity.X = NPC.velocity.X + num323;
                            if (NPC.velocity.X < 0f && num325 > 0f) {
                                NPC.velocity.X = NPC.velocity.X + num323;
                            }
                        }
                        else {
                            if (NPC.velocity.X > num325) {
                                NPC.velocity.X = NPC.velocity.X - num323;
                                if (NPC.velocity.X > 0f && num325 < 0f) {
                                    NPC.velocity.X = NPC.velocity.X - num323;
                                }
                            }
                        }
                        if (NPC.velocity.Y < num326) {
                            NPC.velocity.Y = NPC.velocity.Y + num323;
                            if (NPC.velocity.Y < 0f && num326 > 0f) {
                                NPC.velocity.Y = NPC.velocity.Y + num323;
                            }
                        }
                        else {
                            if (NPC.velocity.Y > num326) {
                                NPC.velocity.Y = NPC.velocity.Y - num323;
                                if (NPC.velocity.Y > 0f && num326 < 0f) {
                                    NPC.velocity.Y = NPC.velocity.Y - num323;
                                }
                            }
                        }
                        NPC.ai [2] += 1f;
                        if (NPC.ai [2] >= 920f) { //reset the cycle if it's over
                            t = 0;
                            NPC.ai [1] = 1f;
                            NPC.ai [2] = 0f;
                            NPC.ai [3] = 0f;
                            NPC.target = 255;
                            NPC.netUpdate = true;
                        }
                        else {
                            if (NPC.ai [2] < 360) { // laser spam
                                if (!Main.player [NPC.target].dead) {
                                    NPC.ai [3] += 1f;
                                    glowopacity += 0.015f;
                                }
                                if (NPC.ai [3] >= 60 && NPC.ai [3] <= 70) {
                                    float Speed = 8f;
                                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
                                    SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                                    float rotation = (float) Math.Atan2(vector8.Y - (Main.player [NPC.target].position.Y + (Main.player [NPC.target].height * 0.5f)), vector8.X - (Main.player [NPC.target].position.X + (Main.player [NPC.target].width * 0.5f)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float) ((Math.Cos(rotation) * Speed) * -1), (float) ((Math.Sin(rotation) * Speed) * -1), ModContent.ProjectileType<OcramLaser1>(), (int) (NPC.damage * 1.5f), 0f, 0);
                                    if (NPC.ai [3] >= 70) {
                                        NPC.ai [3] = 0;
                                    }

                                    if (glowopacity > 0) glowopacity -= 0.15f;
                                    if (glowopacity < 0) glowopacity = 0;

                                    if (NPC.ai [3] % 2 == 0) {
                                        int index3 = Dust.NewDust(NPC.Center, 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.8f, 2f));
                                        Main.dust [index3].noGravity = true;
                                        Main.dust [index3].velocity *= 0.8f;
                                        Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                    }
                                }
                            }
                            if (NPC.ai [2] > 360 && NPC.ai [2] <= 520) { //stationary laser barrage preparation
                                if (!Main.player [NPC.target].dead) {
                                    NPC.ai [3] += 1f;
                                }
                                if (NPC.ai [2] == 380) {
                                    SoundEngine.PlaySound(SoundID.Item15, NPC.position);
                                    int num23 = 36;
                                    for (int index1 = 0; index1 < num23; ++index1) {
                                        Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2((float) NPC.width / 2f, (float) NPC.height) * 0.75f * 0.5f).RotatedBy((double) (index1 - (num23 / 2 - 1)) * 6.28318548202515 / (double) num23, new Vector2()) + NPC.Center;
                                        Vector2 vector2_4 = vector2_3 - NPC.Center;
                                        int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.LifeDrain, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                        Main.dust [index2].noGravity = true;
                                        Main.dust [index2].noLight = true;
                                        Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * -3f;
                                    }
                                }

                                if (NPC.ai[2] >= 380 && NPC.ai[2] <= 420) glowopacity += 0.15f;
                                if (glowopacity > 1) glowopacity = 1;

                                if (NPC.ai [2] > 420 && NPC.ai [2] <= 480) { //stationary laser barrage
                                     NPC.velocity *= 0.9f;
                                    if ((double) NPC.velocity.X > -0.1 && (double) NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if ((double) NPC.velocity.Y > -0.1 && (double) NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                    if (NPC.ai [3] > 2) {
                                        SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                                        NPC.ai [3] = 0;
                                        Vector2 velocity = Vector2.Normalize(Main.player [NPC.target].Center - NPC.Center) * 10;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X - Main.rand.Next(-2, 2), velocity.Y - Main.rand.Next(-2, 2), ModContent.ProjectileType<OcramLaser1>(), (int) (NPC.damage / 2), 0f, Main.myPlayer, 0f, NPC.whoAmI);

                                        int index3 = Dust.NewDust(NPC.Center, 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.8f, 2f));
                                        Main.dust [index3].noGravity = true;
                                        Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                    }

                                    if (glowopacity > 0) glowopacity -= 0.02f;
                                    if (glowopacity < 0) glowopacity = 0;
                                }

                                if (NPC.ai [2] > 480 && NPC.ai [2] <= 500) {
                                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                    if ((double) NPC.velocity.X > -0.1 && (double) NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if ((double) NPC.velocity.Y > -0.1 && (double) NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                            }

                            if (NPC.ai [2] == 500f) { //summon begins
                                effect = true; //trail activates
                                if (NPC.Center.X < Main.player [NPC.target].Center.X) {
                                    NPC.velocity.X = -12;
                                }
                                else {
                                    NPC.velocity.X = 12;
                                }
                                NPC.velocity.Y = -10;
                                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                                int num23 = 36;
                                for (int index1 = 0; index1 < num23; ++index1) {
                                    Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2((float) NPC.width / 2f, (float) NPC.height) * 0.75f * 0.5f).RotatedBy((double) (index1 - (num23 / 2 - 1)) * 6.28318548202515 / (double) num23, new Vector2()) + NPC.Center;
                                    Vector2 vector2_4 = vector2_3 - NPC.Center;
                                    int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.LifeDrain, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                    Main.dust [index2].noGravity = true;
                                    Main.dust [index2].noLight = true;
                                    Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * 3f;
                                }
                            }

                            if (NPC.ai [2] >= 540f && NPC.ai [2] <= 740f) { //servant summon
                                if (t <= 30) {
                                    t++;
                                }
                                h += 0.1f;
                                NPC.velocity.X = (float) Math.Cos(h) * t;
                                NPC.velocity.Y = (float) Math.Sin(h) * t;
                                NPC.rotation = (float) Math.Atan2((double) NPC.velocity.Y, (double) NPC.velocity.X) - 1.57f;
                                if (NPC.ai [2] % 35 == 0) {
                                    SoundEngine.PlaySound(SoundID.NPCDeath45, NPC.position);
                                    NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X - 40, (int) NPC.Center.Y, ModContent.NPCType<ServantofOcram>());
                                }
                            }

                            if (NPC.ai [2] >= 740f) { //summon finishes
                                if (NPC.ai [2] >= 740f) {
                                    NPC.ai [2] = 920;
                                    SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                                    int num23 = 36;
                                    for (int index1 = 0; index1 < num23; ++index1) {
                                        Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2((float) NPC.width / 2f, (float) NPC.height) * 0.75f * 0.5f).RotatedBy((double) (index1 - (num23 / 2 - 1)) * 6.28318548202515 / (double) num23, new Vector2()) + NPC.Center;
                                        Vector2 vector2_4 = vector2_3 - NPC.Center;
                                        int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.Shadowflame, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                        Main.dust [index2].noGravity = true;
                                        Main.dust [index2].noLight = true;
                                        Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * 3f;
                                    }
                                }
                                NPC.velocity.X = NPC.velocity.X * 0.93f;
                                NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                if ((double) NPC.velocity.X > -0.1 && (double) NPC.velocity.X < 0.1) {
                                    NPC.velocity.X = 0f;
                                }
                                if ((double) NPC.velocity.Y > -0.1 && (double) NPC.velocity.Y < 0.1) {
                                    NPC.velocity.Y = 0f;
                                }
                            }
                        }
                    }
                    else {
                        if (NPC.ai [1] == 1f) { //dash attack
                            NPC.rotation = num319;
                            float num332 = 14f;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                            Vector2 vector33 = new Vector2(NPC.position.X + (float) NPC.width * 0.5f, NPC.position.Y + (float) NPC.height * 0.5f);
                            float num333 = Main.player [NPC.target].position.X + (float) (Main.player [NPC.target].width / 2) - vector33.X;
                            float num334 = Main.player [NPC.target].position.Y + (float) (Main.player [NPC.target].height / 2) - vector33.Y;
                            float num335 = (float) Math.Sqrt((double) (num333 * num333 + num334 * num334));
                            num335 = num332 / num335;
                            NPC.velocity.X = num333 * num335;
                            NPC.velocity.Y = num334 * num335;
                            NPC.ai [1] = 2f; //switch to pause between dashes
                        }
                        else {
                            if (NPC.ai [1] == 2f) { //dash attack pause
                                NPC.ai [2] += 1f; // timer
                                if (NPC.ai [2] >= 25f) {
                                    NPC.velocity.X = NPC.velocity.X * 0.96f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.96f;
                                    if ((double) NPC.velocity.X > -0.01 && (double) NPC.velocity.X < 0.001) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if ((double) NPC.velocity.Y > -0.01 && (double) NPC.velocity.Y < 0.001) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                                else {
                                    NPC.rotation = (float) Math.Atan2((double) NPC.velocity.Y, (double) NPC.velocity.X) - 1.57f;
                                    if (Main.rand.NextBool(4)) {
                                        int index4 = Dust.NewDust(NPC.Center + new Vector2(120, 0).RotatedBy(NPC.rotation), 20, 20, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.6f, 1.2f)); //235, 258, 296, 183
                                        Main.dust [index4].velocity = NPC.velocity;
                                        Main.dust [index4].noGravity = true;
                                        Main.dust [index4].fadeIn = Main.rand.NextFloat(0, 1.4f);
                                        int index5 = Dust.NewDust(NPC.Center + new Vector2(-120, 0).RotatedBy(NPC.rotation), 20, 20, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.6f, 1.2f)); //235, 258, 296, 183
                                        Main.dust [index5].velocity = NPC.velocity;
                                        Main.dust [index5].noGravity = true;
                                        Main.dust [index5].fadeIn = Main.rand.NextFloat(0, 1.4f);
                                    }
                                }
                                if (NPC.ai [2] >= 70f) { //timer runs out
                                    NPC.ai [3] += 1f; //count the dash with ai[3]
                                    NPC.ai [2] = 0f;
                                    NPC.target = 255;
                                    NPC.rotation = num319;
                                    if (NPC.ai [3] >= 4f) { //dash attack ends after ai[3] reaches 4
                                        effect = false; //trail deactivates
                                        NPC.ai [1] = 0f;
                                        NPC.ai [3] = 0f;
                                    }
                                    else {
                                        NPC.ai [1] = 1f; //if the attack didn't end, make another dash
                                    }
                                }
                            }
                        }
                    }
                    if (Phase2) {
                        NPC.ai [0] = 1f;
                        NPC.ai [1] = 0f;
                        NPC.ai [2] = 0f;
                        NPC.ai [3] = 0f;
                        effect = false;
                        NPC.netUpdate = true;
                        return;
                    }
                }
                else {
                    if (NPC.ai [0] == 1f || NPC.ai [0] == 2f) { //transformation spin begins
                        if (NPC.ai [0] == 1f) {
                            NPC.ai [2] += 0.005f;
                            if ((double) NPC.ai [2] > 0.5) {
                                NPC.ai [2] = 0.5f;
                            }
                        }
                        else {
                            NPC.ai [2] -= 0.005f;
                            if (NPC.ai [2] < 0f) {
                                NPC.ai [2] = 0f;
                            }
                        }
                        NPC.rotation += NPC.ai [2];
                        NPC.ai [1] += 1f;
                        if (NPC.ai [1] == 100f) {
                            NPC.ai [0] += 1f;
                            NPC.ai [1] = 0f;
                            if (NPC.ai [0] == 3f) {
                                NPC.ai [2] = 0f;
                            }
                            else {
                                showeye = true;
                                SoundEngine.PlaySound(SoundID.NPCHit1, NPC.position);
                                if (Main.netMode != NetmodeID.Server) {

                                    for (int num373 = 0; num373 < 2; num373++) {
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2((float) Main.rand.Next(-30, 31) * 0.2f, (float) Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, 1f);
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2((float) Main.rand.Next(-30, 31) * 0.2f, (float) Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, 1f);
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2((float) Main.rand.Next(-30, 31) * 0.2f, (float) Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, 1f);
                                    }
                                }
                                for (int num374 = 0; num374 < 20; num374++) {
                                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float) Main.rand.Next(-30, 31) * 0.2f, (float) Main.rand.Next(-30, 31) * 0.2f, 0, default(Color), 1f);
                                }
                                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                            }
                        }
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float) Main.rand.Next(-30, 31) * 0.2f, (float) Main.rand.Next(-30, 31) * 0.2f, 0, default(Color), 1f);
                        NPC.velocity.X = NPC.velocity.X * 0.98f;
                        NPC.velocity.Y = NPC.velocity.Y * 0.98f;
                        if ((double) NPC.velocity.X > -0.1 && (double) NPC.velocity.X < 0.1) {
                            NPC.velocity.X = 0f;
                        }
                        if ((double) NPC.velocity.Y > -0.1 && (double) NPC.velocity.Y < 0.1) {
                            NPC.velocity.Y = 0f;
                            return;
                        }
                    } //transformation spin ends, phase 2 ai begins
                    else {
                        NPC.damage = (int) (NPC.defDamage * 1.01);
                        if (NPC.ai [1] == 0f) {
                            float num375 = 14f;
                            float num376 = 0.1f;
                            int num377 = 1;
                            if (NPC.position.X + (float) (NPC.width / 2) < Main.player [NPC.target].position.X + (float) Main.player [NPC.target].width) {
                                num377 = -1;
                            }
                            Vector2 vector38 = new Vector2(NPC.position.X + (float) NPC.width * 0.5f, NPC.position.Y + (float) NPC.height * 0.5f);
                            float num378 = Main.player [NPC.target].position.X + (float) (Main.player [NPC.target].width / 2) - (float) (num377 * 180) - vector38.X;
                            float num379 = Main.player [NPC.target].position.Y + (float) (Main.player [NPC.target].height / 2) - 300f - vector38.Y;
                            float num380 = (float) Math.Sqrt((double) (num378 * num378 + num379 * num379));
                            num380 = num375 / num380;
                            num378 *= num380;
                            num379 *= num380;
                            if (NPC.velocity.X < num378) {
                                NPC.velocity.X = NPC.velocity.X + num376;
                                if (NPC.velocity.X < 0f && num378 > 0f) {
                                    NPC.velocity.X = NPC.velocity.X + num376;
                                }
                            }
                            else {
                                if (NPC.velocity.X > num378) {
                                    NPC.velocity.X = NPC.velocity.X - num376;
                                    if (NPC.velocity.X > 0f && num378 < 0f) {
                                        NPC.velocity.X = NPC.velocity.X - num376;
                                    }
                                }
                            }
                            if (NPC.velocity.Y < num379) {
                                NPC.velocity.Y = NPC.velocity.Y + num376;
                                if (NPC.velocity.Y < 0f && num379 > 0f) {
                                    NPC.velocity.Y = NPC.velocity.Y + num376;
                                }
                            }
                            else {
                                if (NPC.velocity.Y > num379) {
                                    NPC.velocity.Y = NPC.velocity.Y - num376;
                                    if (NPC.velocity.Y > 0f && num379 < 0f) {
                                        NPC.velocity.Y = NPC.velocity.Y - num376;
                                    }
                                }
                            }
                            NPC.ai [2] += 1f; //ai[2] is a simple timer (reset by some attacks), ai[1] cycles between attacks depending on it

                            if (NPC.ai [2] <= 200f) { //laser bullet hell
                                if (Main.netMode != NetmodeID.MultiplayerClient) {
                                    NPC.localAI [2] += 1f;
                                    if (NPC.localAI [2] > 8f) {
                                        NPC.localAI [2] = 0f;
                                    }
                                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                                        NPC.velocity *= 0.95f;
                                        NPC.localAI [1] += 2f;
                                        if ((double) NPC.life < (double) NPC.lifeMax * 0.5) {
                                            NPC.localAI [1] += 1f;
                                        }
                                        if ((double) NPC.life < (double) NPC.lifeMax * 0.25) {
                                            NPC.localAI [1] += 2f;
                                        }
                                        if (NPC.localAI [1] > 8f) {
                                            NPC.localAI [1] = 0f;
                                            float num363 = Main.player [NPC.target].position.X + (float) (Main.player [NPC.target].width / 2) - vector38.X - 70;
                                            float num364 = Main.player [NPC.target].position.Y + (float) (Main.player [NPC.target].height / 2) - vector38.Y;
                                            vector38 = new Vector2(NPC.position.X + (float) NPC.width * 0.5f, NPC.position.Y + (float) NPC.height * 0.5f);
                                            num363 -= Main.rand.Next(-80, 80) - 70;
                                            num364 -= Main.rand.Next(-80, 80);
                                            float num365 = 10 / (float) Math.Sqrt((double) (num363 * num363 + num364 * num364));
                                            num363 *= num365;
                                            num364 *= num365;
                                            num363 += (float) Main.rand.Next(-30, 31) * 0.05f;
                                            num364 += (float) Main.rand.Next(-30, 31) * 0.05f;
                                            vector38.X += num363 * 3f;
                                            vector38.Y += num364 * 3f;
                                            Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
                                            vector8 += new Vector2(0, -30).RotatedBy(NPC.rotation);
                                            SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                                            float rotation = (float) Math.Atan2(vector8.Y - (Main.player [NPC.target].position.Y + (Main.player [NPC.target].height * 0.5f)), vector8.X - (Main.player [NPC.target].position.X + (Main.player [NPC.target].width * 0.5f)));
                                            Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, num363, num364, ModContent.ProjectileType<OcramLaser2>(), NPC.damage * 2, 0f, 0);

                                            int index3 = Dust.NewDust(vector8, 0, 0, DustID.Shadowflame, 0f, 0f, 100, default(Color), 1f + Main.rand.NextFloat(0, 1.5f));
                                            Main.dust [index3].noGravity = true;
                                            Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                        }
                                    }
                                }
                            }

                            if (NPC.ai [2] >= 540f) //reset and switch to next attack
                            {
                                effect = true;
                                NPC.ai [1] = 1f;
                                NPC.ai [2] = 0f;
                                NPC.ai [3] = 0f;
                                NPC.target = 255;
                                NPC.netUpdate = true;
                            }
                        }
                        else {
                            if (NPC.ai [1] == 1f) { //scythe attack dash
                                int damage = NPC.damage / 3;
                                float Speed = 9f;
                                Vector2 velocity = Vector2.Normalize(Main.player [NPC.target].Center - NPC.Center) * Speed;

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X + 120, NPC.position.Y + 50, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X + 160, NPC.position.Y + 50, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X - 120, NPC.position.Y - 50, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X - 160, NPC.position.Y - 50, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X + 50, NPC.position.Y + 120, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X + 50, NPC.position.Y + 160, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X - 50, NPC.position.Y - 120, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X - 50, NPC.position.Y - 160, velocity.X, velocity.Y, ModContent.ProjectileType<OcramScythe>(), damage, 0f, 0);

                                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                                NPC.rotation = num319;
                                float num384 = 18f;
                                Vector2 vector39 = new Vector2(NPC.position.X + (float) NPC.width * 0.5f, NPC.position.Y + (float) NPC.height * 0.5f);
                                float num385 = Main.player [NPC.target].position.X + (float) (Main.player [NPC.target].width / 2) - vector39.X;
                                float num386 = Main.player [NPC.target].position.Y + (float) (Main.player [NPC.target].height / 2) - vector39.Y;
                                float num387 = (float) Math.Sqrt((double) (num385 * num385 + num386 * num386));
                                num387 = num384 / num387;
                                NPC.velocity.X = num385 * num387;
                                NPC.velocity.Y = num386 * num387;
                                NPC.ai [1] = 2f; //switch to pause between dashes
                                return;
                            }
                            if (NPC.ai [1] == 2f) { //scythe attack pause
                                NPC.ai [2] += 1f; //timer between dashes
                                if (NPC.ai [2] >= 50f) {
                                    //int num23 = 36;
                                    int num23 = 2;
                                    for (int index1 = 0; index1 < num23; ++index1) {
                                        //Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2((float) NPC.width / 2f, (float) NPC.height) * 0.75f * 0.5f).RotatedBy((double) (index1 - (num23 / 2 - 1)) * Math.PI * 2 / (double) num23, new Vector2()) + NPC.Center;
                                        //Vector2 vector2_4 = vector2_3 - NPC.Center;
                                        //int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, 27, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                        int index2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, DustID.Shadowflame, 0f, 0f, 100, default(Color), 1f + Main.rand.NextFloat(0, 1.5f));
                                        Main.dust [index2].noGravity = true;
                                        Main.dust [index2].noLight = false;
                                        Main.dust [index2].fadeIn = Main.rand.NextFloat(0, 1f);
                                        Main.dust [index2].position += new Vector2(Main.rand.Next(80, 120), 0).RotatedByRandom(Math.PI * 2);
                                        Main.dust [index2].velocity = Vector2.Normalize(NPC.Center - Main.dust [index2].position) * Main.rand.NextFloat(0, 1.5f);
                                    }
                                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                    if ((double) NPC.velocity.X > -0.1 && (double) NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if ((double) NPC.velocity.Y > -0.1 && (double) NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                                else {
                                    NPC.rotation = (float) Math.Atan2((double) NPC.velocity.Y, (double) NPC.velocity.X) - 1.57f;
                                }
                                if (NPC.ai [2] >= 100f) { //timer runs out
                                    NPC.ai [3] += 1f; //count the dash with ai[3]
                                    NPC.ai [2] = 0f;
                                    NPC.target = 255;
                                    NPC.rotation = num319;
                                    if (NPC.ai [3] >= 6f) { //scythe attack ends after ai[3] reaches 7
                                        effect = false; //trail deactivates
                                        NPC.ai [1] = 0f;
                                        NPC.ai [3] = 0f;
                                        return;
                                    }
                                    NPC.ai [1] = 1f; //if the attack didn't end, make another dash
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            if (Main.bloodMoon && Phase2 && Main.rand.NextBool(600)) { //dumb skull attack
                Vector2 Vector3 = new Vector2(NPC.position.X + NPC.width * 0.1f, NPC.position.Y + NPC.height * 0.1f);
                if (Collision.CanHit(Vector3, 1, 1, Main.player [NPC.target].position, Main.player [NPC.target].width, Main.player [NPC.target].height)) {
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        for (int playerIndex = 0; playerIndex < 255; playerIndex++) {
                            if (Main.player [playerIndex].active) {
                                for (int i = 0; i < MissileProjectiles; i++) {
                                    SoundEngine.PlaySound(SoundID.NPCDeath45, NPC.position);
                                    Player player = Main.player [playerIndex];
                                    int Speed = 10;
                                    float SpawnX = Main.rand.Next(1000) - 500 + player.Center.X;
                                    float SpawnY = -1400 + player.Center.Y;
                                    Vector2 BaseSpawn = new Vector2(SpawnX, SpawnY);
                                    Vector2 BaseVelocity = player.Center - BaseSpawn;
                                    BaseVelocity.Normalize();
                                    BaseVelocity = BaseVelocity * Speed / 2;
                                    Vector2 Spawn = BaseSpawn;
                                    Spawn.X = Spawn.X + i * 30 - (MissileProjectiles * 20);
                                    Vector2 Velocity = BaseVelocity;
                                    Velocity = BaseVelocity.RotatedBy(MathHelper.ToRadians(-MissileAngleSpread / 4 + (MissileAngleSpread * i / MissileProjectiles)));
                                    Velocity.X = Velocity.X + 2 * Main.rand.NextFloat() - 1.3f;
                                    int Num33 = Projectile.NewProjectile(NPC.GetSource_FromAI(), Spawn.X, Spawn.Y, Velocity.X, Velocity.Y, ModContent.ProjectileType<OcramSkull>(), NPC.damage / 3, 1f, Main.myPlayer, 0f, 0f);
                                    Main.projectile [Num33].velocity.X = Main.rand.Next(-200, 201) * 0.1f;
                                    Main.npc [Num33].velocity.Y = Main.rand.Next(-200, 201) * 0.02f;
                                    Main.npc [Num33].netUpdate = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/Ocram");
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new((float) (texture.Width / 2), (float) (texture.Height / Main.npcFrameCount [NPC.type] / 2));
            Vector2 ocrampos = new Vector2(NPC.position.X - Main.screenPosition.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale);
            if (effect) {
                Main.spriteBatch.Draw(texture, ocrampos, new Rectangle?(NPC.frame), Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);
                for (int i = 1; i < NPC.oldPos.Length; i++) {
                    Color color = Lighting.GetColor((int) ((double) NPC.position.X + (double) NPC.width * 0.5) / 16, (int) (((double) NPC.position.Y + (double) NPC.height * 0.5) / 16.0));
                    Color color2 = color;
                    if (NPC.ai [0] == 0f) color2 = Color.Red;
                    else color2 = Color.BlueViolet;
                    color2 = NPC.GetAlpha(color2);
                    color2 *= (NPC.oldPos.Length - i) / 15f;
                    Main.spriteBatch.Draw(texture, ocrampos - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D eye = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramEye");
            Texture2D glow = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramGlow");
            Texture2D eyeglow = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramEyeGlow");

            Vector2 origin = new((float)(eye.Width / 2), (float)(eye.Height / 2));
            Vector2 ocrampos = NPC.Center - Main.screenPosition;

            if (showeye) {
                float eyerotation = (float)Math.Atan2(NPC.Center.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), NPC.Center.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                Vector2 toplayer = NPC.Center - Main.player[NPC.target].Center;
                float playerdistance = (float)Math.Sqrt(toplayer.X * toplayer.X + toplayer.Y * toplayer.Y);
                ocrampos -= new Vector2(Math.Min(500, playerdistance), 0).RotatedBy(eyerotation) / 120;
                Main.spriteBatch.Draw(eye, (ocrampos + oldocrampos) / 2, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);

                if (NPC.ai[2] <= 200f && NPC.ai[1] == 0f)
                {
                    float blink = (float)Math.Sin(NPC.ai[2] / 4) / 2 + 0.5f;
                    drawColor = new Color(1f - blink, 0.5f * blink, 2.5f * blink, 0);
                    Main.spriteBatch.Draw(eyeglow, (ocrampos + oldocrampos) / 2, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
                }

                oldocrampos = ocrampos;
            }
            else {
                drawColor = new Color(1 * glowopacity, 0, 0, 0);
                Main.spriteBatch.Draw(glow, ocrampos, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
        }

        public override void HitEffect (int hitDirection, double damage) {
            if (Main.netMode == NetmodeID.Server)
                return;

            for (int k = 0; k < damage / NPC.lifeMax * 100; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default(Color), 1f);

            if (NPC.life <= 0) {
                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SeaSnail, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, 1.2f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, 1.2f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, 1f);
            }
        }

        public override void OnKill ()
            => NPC.SetEventFlagCleared(ref DownedBossSystem.downedOcram, -1);

        public override void BossLoot (ref string name, ref int potionType)
            => potionType = ItemID.GreaterHealingPotion;

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            Conditions.NotExpert notExpert = new Conditions.NotExpert();
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<OcramBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<OcramRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<CursedFang>(), 4));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(new OneFromOptionsDropRule(1, 1, ModContent.ItemType<EternityStaff>(), ModContent.ItemType<DragonBreath>(), ModContent.ItemType<OcramsEye>(), ModContent.ItemType<Tizona>()));
            npcLoot.Add(notExpertRule);
            npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SoulofBlight>(), 1, 25, 40));
            npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OcramMask>(), 7));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OcramTrophy>(), 10));
        }
    }
}