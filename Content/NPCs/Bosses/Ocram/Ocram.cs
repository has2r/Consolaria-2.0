using Consolaria.Common;
using Consolaria.Content.Items.BossDrops.Ocram;
using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Weapons.Magic;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Summon;
using Consolaria.Content.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Ocram {
    [AutoloadBossHead]
    public class Ocram : ModNPC {
        private int t = 0;
        private int rage;
        private float h = 0.2f;
        private float accelerationV;
        private float accelerationR;
        private float transformR;
        private bool secondPhaseActive = false;

        private const int MissileProjectiles = 5;
        private const float MissileAngleSpread = 150;

        private bool drawTrail = false;
        private bool showEye = false;
        private float glowOpacity;
        private float pulseOpacity;
        private float boomOpacity;
        private float boomOpacityLoss;
        private Color boomColor;
        private float predictScale;
        private float predictRotation;
        private Vector2 ocramOldPos;
        private int spawnCheck;

        private readonly bool bloodMoonMode = Main.bloodMoon;
        private float [] boomRot = new float [9];
        private float [] boomScale = new float [9];
        private float [] boomSpin = new float [9];

        private Vector2 [] scytheSpawnOffset = new Vector2 [8] { new Vector2(120, 50), new Vector2(160, 50), new Vector2(-120, -50), new Vector2(-160, -50), new Vector2(50, 120), new Vector2(50, 160), new Vector2(-50, -120), new Vector2(-50, -160) };
        private float scytheSpawnRotation = Main.rand.NextFloat(0, (float) Math.PI * 2f);

        private readonly bool isExpert = Main.expertMode || Main.masterMode;
        private readonly float rad = (float) Math.PI * 2f;

        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 6;
            NPCID.Sets.MPAllowedEnemies [Type] = true;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int [] {
                    BuffID.Poisoned,
                    BuffID.Confused,
                    BuffID.ShadowFlame
                }
            };

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            float scale = 1f;
            float xOffset = 20f;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/Ocram_Bestiary",
                Position = new Vector2(0, -xOffset),
                Scale = scale,
                PortraitPositionYOverride = -xOffset,
                PortraitScale = scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 190; int height = 180;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;
            AnimationType = 126;

            NPC.lifeMax = 54000;
            NPC.damage = 108;

            NPC.defense = 36;
            NPC.knockBackResist = 0f;

            NPC.value = Item.buyPrice(gold: 15);
            NPC.npcSlots = 10f;

            NPC.boss = true;
            NPC.lavaImmune = true;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.timeLeft = NPC.activeTime * 30;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath18;

            NPC.alpha = 255;

            if (!Main.dedServ) Music = ModContent.GetInstance<ConsolariaConfig>().vanillaBossMusic ? MusicID.Boss5 : (bloodMoonMode ? MusicLoader.GetMusicSlot(Mod, "Assets/Music/EerieOcram") : MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ocram"));
            
        }

        public override void ApplyDifficultyAndPlayerScaling (int numPlayers, float balance, float bossAdjustment) {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * 1.3f);
            NPC.damage = (int) (NPC.damage * 0.7f);
            if (numPlayers <= 1) return;
            float healthBoost = 0.35f;
            for (int k = 1; k < numPlayers; k++) {
                NPC.lifeMax += (int)(NPC.lifeMax * healthBoost);
                healthBoost += (1 - healthBoost) / 3;
            }
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("This forgotten blighted demon, once the powerful Emperor of Darkness, now seeks revenge against all surface dwellers.")
            });
        }

        //abandon hope all ye who enter here
        public override void AI () {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) {
                NPC.TargetClosest(true);
            }

            if (spawnCheck > 0) Lighting.AddLight(NPC.Center, 0.6f + 0.05f * (100 - spawnCheck), 0.4f, 0.5f);

            if (spawnCheck < 100) {
                if (spawnCheck == 0) {
                    string typeName = NPC.TypeName;
                    if (Main.netMode == 0)
                        Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
                    else if (Main.netMode == 2)
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", NPC.GetTypeNetName()), new Color(175, 75, 255));

                    SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/OcramRoar"), NPC.position);
                    NPC.position -= new Vector2(0f, 1150f);
                    NPC.velocity = new Vector2(0, 50f);
                }
                if (spawnCheck == 1) AddGlow(20f, 0.95f, Color.Red);
                if (NPC.alpha > 0) NPC.alpha -= 10;
                NPC.velocity *= 0.95f;
                spawnCheck++;

                if (Main.rand.NextBool(2)) {
                    int index2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(1f, 1.5f));
                    Main.dust [index2].noGravity = true;
                    Main.dust [index2].noLight = false;
                    Main.dust [index2].fadeIn = Main.rand.NextFloat(0, 1.5f);
                    Main.dust [index2].position += new Vector2(Main.rand.Next(80, 120), 0).RotatedByRandom(rad);
                    Main.dust [index2].velocity = Vector2.Normalize(NPC.Center - Main.dust [index2].position) * Main.rand.NextFloat(1f, 2f);
                }
            }

            if (Main.expertMode) {
                if (NPC.life < (int) (NPC.lifeMax * 0.65f)) {
                    secondPhaseActive = true;
                }
            }
            if (!Main.expertMode) {
                if (NPC.life < NPC.lifeMax / 2) {
                    secondPhaseActive = true;
                }
            }

            bool dead2 = Main.player [NPC.target].dead;
            float num317 = NPC.position.X + NPC.width / 2 - Main.player [NPC.target].position.X - Main.player [NPC.target].width / 2;
            float num318 = NPC.position.Y + NPC.height - 59f - Main.player [NPC.target].position.Y - Main.player [NPC.target].height / 2;
            float num319 = (float) Math.Atan2((double) num318, (double) num317) + 1.57f;

            if (num319 < 0f) {
                num319 += rad;
            }
            else {
                if (num319 > rad) {
                    num319 -= rad;
                }
            }

            if (NPC.rotation < num319) {
                if ((double) (num319 - NPC.rotation) > 3.1) {
                    NPC.rotation -= accelerationR;
                }
                else {
                    NPC.rotation += accelerationR;
                }
            }
            else {
                if (NPC.rotation > num319) {
                    if ((double) (NPC.rotation - num319) > 3.1) {
                        NPC.rotation += accelerationR;
                    }
                    else {
                        NPC.rotation -= accelerationR;
                    }
                }
            }
            if (accelerationR < 0.1f) accelerationR += 0.005f;
            if (NPC.rotation > num319 - accelerationR && NPC.rotation < num319 + accelerationR) {
                NPC.rotation = num319;
            }
            if (NPC.rotation < 0f) {
                NPC.rotation += rad;
            }
            else {
                if (NPC.rotation > rad) {
                    NPC.rotation -= rad;
                }
            }
            if (NPC.rotation > num319 - accelerationR && NPC.rotation < num319 + accelerationR) {
                NPC.rotation = num319;
            }
            if (Main.rand.NextBool(5)) {
                if (Main.netMode != NetmodeID.Server) {
                    int num321 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + NPC.height * 0.25f), NPC.width, (int) (NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f, 0, default, 1f);
                    Dust expr_146B6_cp_0 = Main.dust [num321];
                    expr_146B6_cp_0.velocity.X = expr_146B6_cp_0.velocity.X * 0.5f;
                    Dust expr_146D6_cp_0 = Main.dust [num321];
                    expr_146D6_cp_0.velocity.Y = expr_146D6_cp_0.velocity.Y * 0.1f;
                }
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
                        if (accelerationV < 0.2f) accelerationV += 0.01f;
                        float num322 = 12f;
                        int num324 = 1;
                        if (NPC.position.X + NPC.width / 2 < Main.player [NPC.target].position.X + Main.player [NPC.target].width) {
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
                            NPC.velocity.X = NPC.velocity.X + accelerationV;
                            if (NPC.velocity.X < 0f && num325 > 0f) {
                                NPC.velocity.X = NPC.velocity.X + accelerationV;
                            }
                        }
                        else {
                            if (NPC.velocity.X > num325) {
                                NPC.velocity.X = NPC.velocity.X - accelerationV;
                                if (NPC.velocity.X > 0f && num325 < 0f) {
                                    NPC.velocity.X = NPC.velocity.X - accelerationV;
                                }
                            }
                        }
                        if (NPC.velocity.Y < num326) {
                            NPC.velocity.Y = NPC.velocity.Y + accelerationV;
                            if (NPC.velocity.Y < 0f && num326 > 0f) {
                                NPC.velocity.Y = NPC.velocity.Y + accelerationV;
                            }
                        }
                        else {
                            if (NPC.velocity.Y > num326) {
                                NPC.velocity.Y = NPC.velocity.Y - accelerationV;
                                if (NPC.velocity.Y > 0f && num326 < 0f) {
                                    NPC.velocity.Y = NPC.velocity.Y - accelerationV;
                                }
                            }
                        }

                        if (spawnCheck < 100) return;

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
                                    glowOpacity += 0.015f;
                                }
                                if (NPC.ai [3] >= 60 || NPC.ai [3] >= 40 && bloodMoonMode) {
                                    float Speed = 8f;
                                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
                                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                                    float rotation = (float) Math.Atan2(vector8.Y - (Main.player [NPC.target].position.Y + (Main.player [NPC.target].height * 0.5f)), vector8.X - (Main.player [NPC.target].position.X + (Main.player [NPC.target].width * 0.5f)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float) ((Math.Cos(rotation) * Speed) * -1), (float) ((Math.Sin(rotation) * Speed) * -1), ModContent.ProjectileType<OcramLaser1>(), (int) (NPC.damage * 0.2f), 1.5f);
                                    if (NPC.ai [3] >= 70) {
                                        NPC.ai [3] = 0;
                                    }

                                    if (glowOpacity > 0) glowOpacity -= 0.15f;
                                    if (glowOpacity < 0) glowOpacity = 0;

                                    if (NPC.ai [3] % 2 == 0) {
                                        if (Main.netMode != NetmodeID.Server) {
                                            int index3 = Dust.NewDust(NPC.Center, 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.8f, 2f));
                                            Main.dust [index3].noGravity = true;
                                            Main.dust [index3].velocity *= 0.8f;
                                            Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);

                                        }
                                    }
                                }
                            }
                            if (NPC.ai [2] > 360 && NPC.ai [2] <= 520) { //stationary laser barrage preparation
                                if (!Main.player [NPC.target].dead) {
                                    NPC.ai [3] += 1f;
                                }

                                if (NPC.ai [2] == 379 && !isExpert || NPC.ai [2] == 379 && bloodMoonMode && Main.rand.NextBool(2)) NPC.ai [2] = 481;

                                if (NPC.ai [2] == 380) {
                                    SoundEngine.PlaySound(SoundID.Item15, NPC.Center);
                                    if (Main.netMode != NetmodeID.Server) {
                                        int num23 = 36;
                                        for (int index1 = 0; index1 < num23; ++index1) {
                                            Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2(NPC.width / 2f, NPC.height) * 0.75f * 0.5f).RotatedBy((index1 - (num23 / 2 - 1)) * 6.25 / num23, new Vector2()) + NPC.Center;
                                            Vector2 vector2_4 = vector2_3 - NPC.Center;
                                            int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.LifeDrain, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                            Main.dust [index2].noGravity = true;
                                            Main.dust [index2].noLight = true;
                                            Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * -3f;
                                        }
                                    }
                                }

                                if (NPC.ai [2] >= 380 && NPC.ai [2] <= 420) glowOpacity += 0.15f;
                                if (glowOpacity > 1) glowOpacity = 1;

                                if (NPC.ai [2] > 420 && NPC.ai [2] <= 480) { //stationary laser barrage
                                    NPC.velocity *= 0.9f;
                                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                    if (NPC.ai [3] > 2) {
                                        SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                                        NPC.ai [3] = 0;
                                        Vector2 velocity = Vector2.Normalize(Main.player [NPC.target].Center - NPC.Center) * 10;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X - Main.rand.Next(-2, 2), velocity.Y - Main.rand.Next(-2, 2), ModContent.ProjectileType<OcramLaser1>(), (int) (NPC.damage * 0.2f), 1.5f);
                                        if (Main.netMode != NetmodeID.Server) {
                                            int index3 = Dust.NewDust(NPC.Center, 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.8f, 2f));
                                            Main.dust [index3].noGravity = true;
                                            Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                        }
                                    }

                                    if (glowOpacity > 0) glowOpacity -= 0.02f;
                                    if (glowOpacity < 0) glowOpacity = 0;
                                }

                                if (NPC.ai [2] > 480 && NPC.ai [2] <= 500) {
                                    if (glowOpacity > 0) glowOpacity -= 0.02f;
                                    if (glowOpacity < 0) glowOpacity = 0;
                                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                            }

                            if (NPC.ai [2] == 500f) { //summon begins
                                drawTrail = true; //trail activates
                                if (NPC.Center.X < Main.player [NPC.target].Center.X) {
                                    NPC.velocity.X = -12;
                                }
                                else {
                                    NPC.velocity.X = 12;
                                }
                                NPC.velocity.Y = -10;
                                SoundEngine.PlaySound(SoundID.Roar with { PitchVariance = 0.15f, MaxInstances = 0 }, NPC.Center);
                                if (Main.netMode != NetmodeID.Server) {
                                    int num23 = 36;
                                    for (int index1 = 0; index1 < num23; ++index1) {
                                        Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2(NPC.width / 2f, NPC.height) * 0.75f * 0.5f).RotatedBy((index1 - (num23 / 2 - 1)) * 6.25 / num23, new Vector2()) + NPC.Center;
                                        Vector2 vector2_4 = vector2_3 - NPC.Center;
                                        int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.LifeDrain, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                        Main.dust [index2].noGravity = true;
                                        Main.dust [index2].noLight = true;
                                        Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * 3f;
                                    }
                                }
                            }

                            if (NPC.ai [2] >= 540f && NPC.ai [2] <= 740f) { //servant summon
                                if (t <= 30) {
                                    t++;
                                }
                                h += 0.1f;
                                NPC.velocity.X = (float) Math.Cos(h) * t;
                                NPC.velocity.Y = (float) Math.Sin(h) * t;
                                NPC.rotation = (float) Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                                if (bloodMoonMode && NPC.ai [2] % 5 == 0) {
                                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 10f).RotatedBy(NPC.rotation), ModContent.ProjectileType<OcramLaser1>(), (int) (NPC.damage * 0.2f), 1.5f);
                                }
                                if (NPC.ai [2] % 35 == 0) {
                                    SoundEngine.PlaySound(SoundID.NPCDeath45, NPC.Center);
                                    NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X - 40, (int) NPC.Center.Y, ModContent.NPCType<ServantofOcram>());
                                }
                            }

                            if (NPC.ai [2] >= 740f) { //summon finishes
                                if (NPC.ai [2] >= 740f) {
                                    NPC.ai [2] = 920;
                                    if (Main.netMode != NetmodeID.Server) {
                                        int num23 = 36;
                                        for (int index1 = 0; index1 < num23; ++index1) {
                                            Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2(NPC.width / 2f, NPC.height) * 0.75f * 0.5f).RotatedBy((index1 - (num23 / 2 - 1)) * 6.25 / num23, new Vector2()) + NPC.Center;
                                            Vector2 vector2_4 = vector2_3 - NPC.Center;
                                            int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.Shadowflame, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                            Main.dust [index2].noGravity = true;
                                            Main.dust [index2].noLight = true;
                                            Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * 3f;
                                        }
                                    }
                                }
                                NPC.velocity.X = NPC.velocity.X * 0.93f;
                                NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1) {
                                    NPC.velocity.X = 0f;
                                }
                                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1) {
                                    NPC.velocity.Y = 0f;
                                }
                            }
                        }
                    }
                    else {
                        if (NPC.ai [1] == 1f) { //dash attack
                            NPC.rotation = num319;
                            float num332 = 14f;
                            SoundEngine.PlaySound(SoundID.Roar with { PitchVariance = 0.15f, MaxInstances = 0 }, NPC.Center);
                            Vector2 vector33 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                            float num333 = Main.player [NPC.target].position.X + Main.player [NPC.target].width / 2 - vector33.X;
                            float num334 = Main.player [NPC.target].position.Y + Main.player [NPC.target].height / 2 - vector33.Y;
                            float num335 = (float) Math.Sqrt((double) (num333 * num333 + num334 * num334));
                            num335 = num332 / num335;
                            NPC.velocity.X = num333 * num335;
                            NPC.velocity.Y = num334 * num335;
                            if (bloodMoonMode) NPC.velocity *= 1.3f;
                            NPC.ai [1] = 2f; //switch to pause between dashes
                        }
                        else {
                            if (NPC.ai [1] == 2f) { //dash attack pause
                                NPC.ai [2] += 1f; // timer
                                if (NPC.ai [2] >= 25f) {
                                    NPC.velocity.X = NPC.velocity.X * 0.96f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.96f;
                                    if (NPC.velocity.X > -0.01 && NPC.velocity.X < 0.001) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if (NPC.velocity.Y > -0.01 && NPC.velocity.Y < 0.001) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                                else {
                                    NPC.rotation = (float) Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                                    if (Main.netMode != NetmodeID.Server) {
                                        if (Main.rand.NextBool(4)) {
                                            int index4 = Dust.NewDust(NPC.Center + new Vector2(120, 0).RotatedBy(NPC.rotation), 20, 20, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.6f, 1.2f));
                                            Main.dust [index4].velocity = NPC.velocity;
                                            Main.dust [index4].noGravity = true;
                                            Main.dust [index4].fadeIn = Main.rand.NextFloat(0, 1.4f);
                                            int index5 = Dust.NewDust(NPC.Center + new Vector2(-120, 0).RotatedBy(NPC.rotation), 20, 20, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.6f, 1.2f));
                                            Main.dust [index5].velocity = NPC.velocity;
                                            Main.dust [index5].noGravity = true;
                                            Main.dust [index5].fadeIn = Main.rand.NextFloat(0, 1.4f);
                                        }
                                    }
                                }
                                if (NPC.ai [2] >= 70f) { //timer runs out
                                    NPC.ai [3] += 1f; //count the dash with ai[3]
                                    NPC.ai [2] = 0f;
                                    NPC.target = 255;
                                    NPC.rotation = num319;
                                    if (NPC.ai [3] >= 4f) { //dash attack ends after ai[3] reaches 4
                                        if (Main.netMode != NetmodeID.Server) {
                                            int num23 = 40;
                                            for (int index1 = 0; index1 < num23; ++index1) {
                                                int index2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, DustID.LavaMoss, 0f, 0f, 100, new Color(255, 0, 244), Main.rand.NextFloat(0.8f, 1.4f));
                                                Main.dust [index2].noGravity = true;
                                                Main.dust [index2].noLight = false;
                                                Main.dust [index2].fadeIn = Main.rand.NextFloat(0, 1f);
                                                Main.dust [index2].position += new Vector2(Main.rand.Next(60, 100), 0).RotatedByRandom(rad);
                                                Main.dust [index2].velocity = Vector2.Normalize(NPC.Center - Main.dust [index2].position) * Main.rand.NextFloat(0, 1.5f);
                                            }
                                        }
                                        drawTrail = false; //trail deactivates
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
                    if (secondPhaseActive) {
                        transformR = NPC.rotation;
                        NPC.ai [0] = 1f;
                        NPC.ai [1] = 0f;
                        NPC.ai [2] = 0f;
                        NPC.ai [3] = 0f;
                        drawTrail = false;
                        NPC.netUpdate = true;
                        return;
                    }
                }
                else {
                    if (NPC.ai [0] == 1f || NPC.ai [0] == 2f) { //transformation spin begins
                        if (NPC.ai [0] == 1f) {
                            NPC.ai [2] += 0.005f;
                            if (NPC.ai [2] > 0.5f) {
                                NPC.ai [2] = 0.5f;
                            }
                        }
                        else {
                            NPC.ai [2] -= 0.005f;
                            if (NPC.ai [2] < 0f) {
                                NPC.ai [2] = 0f;
                            }
                        }
                        NPC.rotation = transformR + NPC.ai [2]; //overrides rotation checks by remembering previous values
                        transformR = NPC.rotation;
                        NPC.ai [1] += 1f;
                        if (NPC.ai [1] % 20 == 0 && NPC.ai [0] == 1f) {
                            AddGlow(20f, 0.5f, Color.BlueViolet);
                            SoundEngine.PlaySound(SoundID.NPCDeath43, NPC.Center);
                            SoundEngine.PlaySound(Main.rand.NextBool(3) ? SoundID.NPCDeath23 : SoundID.NPCDeath22, NPC.Center);
                            if (Main.netMode != NetmodeID.Server) {
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, Main.rand.NextFloat(0.9f, 1.4f));
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, Main.rand.NextFloat(0.9f, 1.4f));
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, Main.rand.NextFloat(0.9f, 1.4f));

                                for (int index1 = 0; index1 < 14; ++index1) {
                                    int index2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, DustID.Shadowflame, 0f, 0f, 100, default, Main.rand.NextFloat(1f, 2.5f));
                                    Main.dust [index2].noGravity = true;
                                    Main.dust [index2].noLight = false;
                                    Main.dust [index2].fadeIn = Main.rand.NextFloat(0, 1f);
                                    Main.dust [index2].position += new Vector2(Main.rand.Next(40, 120), 0).RotatedByRandom(rad);
                                    Main.dust [index2].velocity = Vector2.Normalize(NPC.Center - Main.dust [index2].position) * Main.rand.NextFloat(-10f, -1f);
                                }
                            }
                        }
                        if (NPC.ai [1] == 100f) {
                            NPC.ai [0] += 1f;
                            NPC.ai [1] = 0f;
                            if (NPC.ai [0] == 3f) {
                                NPC.ai [2] = 0f;
                            }
                            else {
                                accelerationR = 0;
                                showEye = true;
                                SoundEngine.PlaySound(SoundID.NPCHit1, NPC.Center);
                                if (Main.netMode != NetmodeID.Server) {
                                    for (int num373 = 0; num373 < 4; num373++) {
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, Main.rand.NextFloat(0.9f, 1.4f));
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, Main.rand.NextFloat(0.9f, 1.4f));
                                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, Main.rand.NextFloat(0.9f, 1.4f));
                                    }
                                    for (int num374 = 0; num374 < 20; num374++) {
                                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);
                                    }
                                }
                                SoundEngine.PlaySound(SoundID.Roar with { PitchVariance = 0.15f, MaxInstances = 0 }, NPC.Center);
                            }
                        }

                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);
                        NPC.velocity.X = NPC.velocity.X * 0.96f;
                        NPC.velocity.Y = NPC.velocity.Y * 0.96f;
                        if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1) {
                            NPC.velocity.X = 0f;
                        }
                        if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1) {
                            NPC.velocity.Y = 0f;
                        }
                    } //transformation spin ends, phase 2 ai begins
                    else {
                        NPC.damage = (int) (NPC.defDamage * 1.02);
                        if (NPC.ai [1] == 0f) {
                            float num375 = 14f;
                            float num376 = 0.1f;
                            int num377 = 1;
                            if (NPC.position.X + NPC.width / 2 < Main.player [NPC.target].position.X + Main.player [NPC.target].width) {
                                num377 = -1;
                            }
                            Vector2 newvel = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                            float num378 = Main.player [NPC.target].position.X + Main.player [NPC.target].width / 2 - num377 * 180 - newvel.X;
                            float num379 = Main.player [NPC.target].position.Y + Main.player [NPC.target].height / 2 - 300f - newvel.Y;
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
                                NPC.velocity *= 0.95f;
                                NPC.localAI [1] += 2f;
                                if (NPC.life < NPC.lifeMax * 0.5) {
                                    NPC.localAI [1] += 1f;
                                }
                                if (NPC.life < NPC.lifeMax * 0.25 || rage > 80) {
                                    NPC.localAI [1] += 2f;
                                }
                                if (NPC.localAI [1] > 8f) {
                                    NPC.localAI [1] = 0f;
                                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
                                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                                        float num363 = Main.player [NPC.target].position.X + Main.player [NPC.target].width / 2 - newvel.X - 70;
                                        float num364 = Main.player [NPC.target].position.Y + Main.player [NPC.target].height / 2 - newvel.Y;
                                        newvel = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                        num363 -= Main.rand.Next(-80, 81) - 70;
                                        num364 -= Main.rand.Next(-80, 81);
                                        float num365 = 10 / (float) Math.Sqrt((double) (num363 * num363 + num364 * num364));
                                        num363 *= num365;
                                        num364 *= num365;
                                        num363 += Main.rand.Next(-30, 31) * 0.05f;
                                        num364 += Main.rand.Next(-30, 31) * 0.05f;
                                        newvel.X += num363 * 3f;
                                        newvel.Y += num364 * 3f;
                                        vector8 += new Vector2(0, -30).RotatedBy(NPC.rotation);
                                        int laser2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, num363, num364, ModContent.ProjectileType<OcramLaser2>(), (int) (NPC.damage * 0.2f), 2.5f);
                                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser2);
                                    }
                                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                                    if (Main.netMode != NetmodeID.Server) {
                                        int index3 = Dust.NewDust(vector8, 0, 0, DustID.Shadowflame, 0f, 0f, 100, default, 1f + Main.rand.NextFloat(0, 1.5f));
                                        Main.dust [index3].noGravity = true;
                                        Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                    }
                                }
                            }

                            //float ai2Limit = isExpert ? 700f : 200f;
                            //if (isExpert && NPC.ai [2] > 200f) { //scythe bullet hell
                            if (NPC.ai [2] > 200f && NPC.ai [2] < 600f) {
                                float distance = 14f;
                                float velocityBoost = 0.7f;
                                int num230 = 1;
                                if (NPC.position.X + NPC.width / 2 < Main.player [NPC.target].position.X + Main.player [NPC.target].width) {
                                    num230 = -1;
                                }
                                Vector2 posVector = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                float posVectorX = Main.player [NPC.target].position.X + (Main.player [NPC.target].width / 2) - 350f - (num230 * 350) - posVector.X; //about 12 if boss is to the left and -12 if it is to the right
                                float posVectorY = Main.player [NPC.target].position.Y + (Main.player [NPC.target].height / 2) - 350f - posVector.Y;
                                float huyZna = (float) Math.Sqrt((posVectorX * posVectorX + posVectorY * posVectorY));
                                huyZna = distance / huyZna;
                                posVectorX *= huyZna;
                                posVectorY *= huyZna;

                                //adjust velocity.x and y between 12 and -12 depending on player position
                                if (NPC.velocity.X < posVectorX) {
                                    NPC.velocity.X = NPC.velocity.X + velocityBoost;
                                    if (NPC.velocity.X < 0f && posVectorX > 0f) {
                                        NPC.velocity.X = NPC.velocity.X + velocityBoost;
                                    }
                                }
                                else {
                                    if (NPC.velocity.X > posVectorX) {
                                        NPC.velocity.X = NPC.velocity.X - velocityBoost;
                                        if (NPC.velocity.X > 0f && posVectorX < 0f) {
                                            NPC.velocity.X = NPC.velocity.X - velocityBoost;
                                        }
                                    }
                                }
                                if (NPC.velocity.Y < posVectorY) {
                                    NPC.velocity.Y = NPC.velocity.Y + velocityBoost;
                                    if (NPC.velocity.Y < 0f && posVectorY > 0f) {
                                        NPC.velocity.Y = NPC.velocity.Y + velocityBoost;
                                    }
                                }
                                else {
                                    if (NPC.velocity.Y > posVectorY) {
                                        NPC.velocity.Y = NPC.velocity.Y - velocityBoost;
                                        if (NPC.velocity.Y > 0f && posVectorY < 0f) {
                                            NPC.velocity.Y = NPC.velocity.Y - velocityBoost;
                                        }
                                    }
                                }

                                if (Main.player [NPC.target].Center.Y < NPC.Center.Y && NPC.ai [2] > 250f) rage++;
                                if (rage > 45) NPC.ai [2] = 700;

                                //if (NPC.ai [2] == 230f) SoundEngine.PlaySound(SoundID.Item117, NPC.Center);

                                NPC.localAI [3]++;
                                if (NPC.localAI [3] % 15 == 0 && NPC.ai [2] > 250f && NPC.ai [2] < 550f) {
                                    ScytheAttack();
                                    NPC.localAI [3] = 0;
                                }

                                if (Main.netMode != NetmodeID.Server) {
                                    Vector2 dustOffset = new Vector2(30, 0).RotatedBy(NPC.rotation + rad / 4);
                                    int index4 = Dust.NewDust(NPC.Center + dustOffset, 0, 0, DustID.Shadowflame, Main.rand.NextFloat(-2f, 2f), -2f, 100, default, Main.rand.NextFloat(0.5f, 2.5f));
                                    Main.dust [index4].position += new Vector2(Main.rand.NextFloat(-45f, 45f), Main.rand.NextFloat(-10f, 10f));
                                    Main.dust [index4].noGravity = true;
                                    Main.dust [index4].fadeIn = Main.rand.NextFloat(0, 1f);
                                }

                                if (NPC.ai [2] <= 250f && pulseOpacity < 1f) pulseOpacity += 0.03f;
                                if (NPC.ai [2] >= 550f && pulseOpacity < 1f) pulseOpacity -= 0.03f;
                            }
                            if (NPC.ai [2] == 600f) {
                                pulseOpacity = 0f;
                                drawTrail = true; //trail activates
                                if (NPC.Center.X < Main.player [NPC.target].Center.X) {
                                    NPC.velocity.X = -14;
                                }
                                else {
                                    NPC.velocity.X = 14;
                                }
                                NPC.velocity.Y = 4;
                                if (Main.netMode != NetmodeID.Server) {
                                    int num23 = 36;
                                    if (isExpert) for (int index1 = 0; index1 < num23; ++index1) {
                                            Vector2 vector2_3 = (Vector2.Normalize(NPC.velocity) * new Vector2(NPC.width / 2f, NPC.height) * 0.75f * 0.5f).RotatedBy((index1 - (num23 / 2 - 1)) * 6.25 / num23, new Vector2()) + NPC.Center;
                                            Vector2 vector2_4 = vector2_3 - NPC.Center;
                                            int index2 = Dust.NewDust(vector2_3 + vector2_4, 0, 0, DustID.Shadowflame, vector2_4.X * 2f, vector2_4.Y * 2f, 100, new Color(), 1.4f);
                                            Main.dust [index2].noGravity = true;
                                            Main.dust [index2].noLight = true;
                                            Main.dust [index2].velocity = Vector2.Normalize(vector2_4) * 3f;
                                        }
                                }
                            }
                            if (isExpert && NPC.ai [2] > 600f && NPC.ai [2] <= 660 && NPC.ai [2] % 5 == 0) {
                                Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
                                if (Main.netMode != NetmodeID.MultiplayerClient) {
                                    vector8 += new Vector2(0, -30).RotatedBy(NPC.rotation);
                                    Vector2 vector9 = new Vector2(0, 1).RotatedBy(NPC.rotation + rad / 4 * ((NPC.ai [2] - 650) / 100));
                                    int laser2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8, vector9 * 12f, ModContent.ProjectileType<OcramLaser2>(), (int) (NPC.damage * 0.2f), 2.5f);
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laser2);
                                }
                                if (Main.netMode != NetmodeID.Server) {
                                    int index3 = Dust.NewDust(vector8, 0, 0, DustID.Shadowflame, 0f, 0f, 100, default, 1f + Main.rand.NextFloat(0, 1.5f));
                                    Main.dust [index3].noGravity = true;
                                    Main.dust [index3].fadeIn = Main.rand.NextFloat(0, 1f);
                                }
                            }
                            if (NPC.ai [2] >= 700f || !isExpert && NPC.ai [2] >= 650f) {
                                NPC.ai [1] = 1f;
                                NPC.ai [2] = 0f;
                                NPC.ai [3] = 0f;
                                NPC.target = 255;
                                NPC.netUpdate = true;
                            }
                        }
                        else {
                            if (NPC.ai [1] == 1f) { //scythe attack dash
                                if (Main.netMode != NetmodeID.MultiplayerClient) {
                                    int damage = (int) (NPC.damage * 0.25f);
                                    float knockback = 4f;
                                    float speed = 9f;
                                    Vector2 velocity = Vector2.Normalize(Main.player [NPC.target].Center - NPC.Center) * speed;
                                    foreach (Vector2 spawnOffset in scytheSpawnOffset)
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + spawnOffset.RotatedBy(scytheSpawnRotation), velocity, ModContent.ProjectileType<OcramScythe>(), damage, knockback);
                                }
                                SoundEngine.PlaySound(SoundID.Roar with { PitchVariance = 0.15f, MaxInstances = 0 }, NPC.Center);
                                NPC.rotation = num319;
                                float num384 = 18f;
                                if (rage > 45) num384 *= 1.25f;
                                Vector2 vector39 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                float num385 = Main.player [NPC.target].position.X + Main.player [NPC.target].width / 2 - vector39.X;
                                float num386 = Main.player [NPC.target].position.Y + Main.player [NPC.target].height / 2 - vector39.Y;
                                float num387 = (float) Math.Sqrt((double) (num385 * num385 + num386 * num386));
                                num387 = num384 / num387;
                                NPC.velocity.X = num385 * num387;
                                NPC.velocity.Y = num386 * num387;
                                scytheSpawnRotation = Main.rand.NextFloat(0, rad);
                                NPC.ai [1] = 2f; //switch to pause between dashes
                                return;
                            }
                            if (NPC.ai [1] == 2f) { //scythe attack pause
                                NPC.ai [2] += 1f; //timer between dashes
                                if (rage > 45) NPC.ai [2] += 1f;
                                if (NPC.ai [2] >= 50f) {
                                    if (NPC.ai [3] < 4f) {
                                        if (predictScale < 1f && NPC.ai [2] >= 60 && NPC.ai [2] < 75) predictScale += 0.1f;
                                        if (predictScale > 0 && NPC.ai [2] >= 85) predictScale -= 0.1f;
                                        if (Main.netMode != NetmodeID.Server) {
                                            int num23 = 2;
                                            for (int index1 = 0; index1 < num23; ++index1) {
                                                int index2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, DustID.Shadowflame, 0f, 0f, 100, default, 1f + Main.rand.NextFloat(0, 1.5f));
                                                Main.dust [index2].noGravity = true;
                                                Main.dust [index2].noLight = false;
                                                Main.dust [index2].fadeIn = Main.rand.NextFloat(0, 1f);
                                                Main.dust [index2].position += new Vector2(Main.rand.Next(80, 120), 0).RotatedByRandom(rad);
                                                Main.dust [index2].velocity = Vector2.Normalize(NPC.Center - Main.dust [index2].position) * Main.rand.NextFloat(0, 1.5f);
                                            }
                                        }
                                    }
                                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                                    NPC.velocity.Y = NPC.velocity.Y * 0.93f;
                                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1) {
                                        NPC.velocity.X = 0f;
                                    }
                                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1) {
                                        NPC.velocity.Y = 0f;
                                    }
                                }
                                else {
                                    if (predictScale > 0) predictScale -= 0.5f;
                                    NPC.rotation = (float) Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                                }
                                if (NPC.ai [2] >= 100f) { //timer runs out
                                    NPC.ai [3] += 1f; //count the dash with ai[3]
                                    NPC.ai [2] = 0f;
                                    NPC.target = 255;
                                    NPC.rotation = num319;
                                    if (NPC.ai [3] >= 5f) { //scythe attack ends after ai[3] reaches 5
                                        predictScale = 0;
                                        drawTrail = false; //trail deactivates
                                        rage = 0;
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
            if (bloodMoonMode && secondPhaseActive && Main.rand.NextBool(500)) { //dumb skull attack
                rage = 999;
                Vector2 Vector3 = new Vector2(NPC.position.X + NPC.width * 0.1f, NPC.position.Y + NPC.height * 0.1f);
                if (Collision.CanHit(Vector3, 1, 1, Main.player [NPC.target].position, Main.player [NPC.target].width, Main.player [NPC.target].height)) {
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        for (int playerIndex = 0; playerIndex < 255; playerIndex++) {
                            if (Main.player [playerIndex].active) {
                                for (int i = 0; i < MissileProjectiles + Main.rand.Next(0, 6); i++) {
                                    SoundEngine.PlaySound(SoundID.NPCDeath45, NPC.Center);
                                    Player player = Main.player [playerIndex];
                                    int Speed = 12;
                                    float SpawnX = Main.rand.Next(1000) - 500 + player.Center.X;
                                    float SpawnY = -2000 + player.Center.Y;
                                    Vector2 BaseSpawn = new Vector2(SpawnX, SpawnY);
                                    Vector2 BaseVelocity = player.Center - BaseSpawn;
                                    BaseVelocity.Normalize();
                                    BaseVelocity = BaseVelocity * Speed / 2;
                                    Vector2 Spawn = BaseSpawn;
                                    Spawn.X = Spawn.X + i * 30 - (MissileProjectiles * 20);
                                    Vector2 Velocity = BaseVelocity.RotatedBy(MathHelper.ToRadians(-MissileAngleSpread / 4 + (MissileAngleSpread * i / MissileProjectiles)));
                                    Velocity.X = Velocity.X + 2 * Main.rand.NextFloat() - 1.3f;
                                    int Num33 = Projectile.NewProjectile(NPC.GetSource_FromAI(), Spawn.X, Spawn.Y, Velocity.X, Velocity.Y, ModContent.ProjectileType<OcramSkull>(), (int) (NPC.damage * 0.25f), 4f);
                                    Main.projectile [Num33].velocity.X = Main.rand.Next(-200, 201) * 0.1f;
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Num33);
                                    Main.npc [Num33].velocity.Y = Main.rand.Next(-200, 201) * 0.02f;
                                    Main.npc [Num33].netUpdate = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ScytheAttack () { //da kto eta vasha trigonometriya
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                float scytheVel = 1;
                float scytheangle = rad / 6 * (float) Math.Sin(NPC.ai [2] / 32);
                Vector2 vel = new Vector2(0, scytheVel).RotatedBy(scytheangle);
                Vector2 projPos = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2)) + vel * 80;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), projPos.X, projPos.Y, vel.X, vel.Y, ModContent.ProjectileType<OcramScythe>(), (int) (NPC.damage * 0.25f), 4f);
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
            }
            SoundEngine.PlaySound(SoundID.Item8 with { Pitch = -0.1f, MaxInstances = 0 }, NPC.Center);
        }

        private void AddGlow (float boomOpacitySet, float boomOpacityLossSet, Color boomColorSet) {
            boomOpacity = boomOpacitySet;
            boomOpacityLoss = boomOpacityLossSet;
            boomColor = boomColorSet;
            for (int i = 0; i < 9; i++) {
                boomRot [i] = rad * Main.rand.NextFloat();
                boomScale [i] = Main.rand.NextFloat(1f, 3f);
                boomSpin [i] = Main.rand.NextFloat(-0.1f, 0.1f);
            }
        }

        public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/Ocram");
            Texture2D boom = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramBoom");
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new(texture.Width / 2, texture.Height / Main.npcFrameCount [NPC.type] / 2);
            Vector2 ocramPos = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale);
            if (drawTrail) {
                //Main.spriteBatch.Draw(texture, ocramPos, new Rectangle?(NPC.frame), Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);
                for (int i = 1; i < NPC.oldPos.Length; i++) {
                    Color color = Lighting.GetColor((int) (NPC.position.X + NPC.width * 0.5) / 16, (int) ((NPC.position.Y + NPC.height * 0.5) / 16.0));
                    if (NPC.ai [0] == 0f) color = Color.Red;
                    else color = Color.BlueViolet;
                    color = NPC.GetAlpha(color);
                    color *= (NPC.oldPos.Length - i) / 15f;
                    Main.spriteBatch.Draw(texture, ocramPos - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            if (pulseOpacity > 0f) {
                float multiplier = (float) (Math.Sin(NPC.ai [2] / 16) / 2 + 0.5);
                Color color2 = NPC.GetAlpha(Color.BlueViolet) * 0.75f * multiplier;
                Vector2 pulseOffset = new Vector2(10, 0).RotatedBy(rad * multiplier) * multiplier;
                Main.spriteBatch.Draw(texture, ocramPos + pulseOffset, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
                Main.spriteBatch.Draw(texture, ocramPos - pulseOffset, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
                pulseOffset = pulseOffset.RotatedBy(rad / 4) * 0.5f;
                Main.spriteBatch.Draw(texture, ocramPos + pulseOffset, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
                Main.spriteBatch.Draw(texture, ocramPos - pulseOffset, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            if (boomOpacity > 0f) {
                boomOpacity *= boomOpacityLoss;
                if (boomOpacity < 0.01f && showEye) AddGlow(1f, 0.95f, Color.BlueViolet);
                if (boomOpacity < 0.01f) boomOpacity = 0f;
                for (int i = 0; i < 9; i++) {
                    if (boomColor == Color.Red) {
                        Main.spriteBatch.Draw(boom, ocramPos, null, NPC.GetAlpha(boomColor) * boomOpacity, boomRot [i] + rad / 2, origin, boomScale [i] * 1.25f, effects, 0f);
                        Main.spriteBatch.Draw(boom, ocramPos, null, new Color(255, 255, 215, 0) * boomOpacity, boomRot [i] + rad / 2, origin, boomScale [i] * 0.75f, effects, 0f);
                    }
                    Main.spriteBatch.Draw(boom, ocramPos, null, NPC.GetAlpha(boomColor) * boomOpacity, boomRot [i], origin, boomScale [i], effects, 0f);
                    boomRot [i] += boomSpin [i];
                    boomSpin [i] *= boomOpacityLoss;
                }
            }
            return true;
        }

        public override void PostDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D eye = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramEye");
            Texture2D glow = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramGlow");
            Texture2D eyeGlow = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramEyeGlow");
            Texture2D predict = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/OcramPredict");

            Vector2 origin = new(eye.Width / 2, eye.Height / 2);
            Vector2 predictOrigin = new(predict.Width / 2, predict.Height / 2);
            Vector2 ocramPos = NPC.Center - Main.screenPosition;

            if (showEye) {
                float eyeRotation = (float) Math.Atan2(NPC.Center.Y - (Main.player [NPC.target].position.Y + (Main.player [NPC.target].height * 0.5f)), NPC.Center.X - (Main.player [NPC.target].position.X + (Main.player [NPC.target].width * 0.5f)));
                Vector2 topLayer = NPC.Center - Main.player [NPC.target].Center;
                float playerDistance = (float) Math.Sqrt(topLayer.X * topLayer.X + topLayer.Y * topLayer.Y);
                ocramPos -= new Vector2(Math.Min(500, playerDistance), 0).RotatedBy(eyeRotation) / 120;
                if (ocramOldPos == Vector2.Zero) ocramOldPos = ocramPos;
                Main.spriteBatch.Draw(eye, (ocramPos + ocramOldPos) / 2, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);

                if (NPC.ai [2] <= 200f && NPC.ai [1] == 0f || isExpert && NPC.ai [2] > 600 && NPC.ai [2] <= 660) {
                    float blink = (float) Math.Sin(NPC.ai [2] / 4) / 2 + 0.5f;
                    drawColor = new Color(1f - blink, 0.5f * blink, 2.5f * blink, 0);
                    Main.spriteBatch.Draw(eyeGlow, (ocramPos + ocramOldPos) / 2, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
                }

                ocramOldPos = ocramPos;
            }
            else {
                drawColor = new Color(1 * glowOpacity, 0, 0, 0);
                Main.spriteBatch.Draw(glow, ocramPos, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            if (predictScale > 0) {
                predictRotation += 0.1f;
                if (predictRotation > rad) predictRotation -= rad;
                foreach (Vector2 spawnOffset in scytheSpawnOffset) {
                    Main.spriteBatch.Draw(predict, ocramPos + spawnOffset.RotatedBy(scytheSpawnRotation), null, new Color(175, 85, 255, 0), predictRotation, predictOrigin, predictScale * 0.6f, SpriteEffects.None, 0f);
                }
            }
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < hit.Damage / NPC.lifeMax * 100; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SeaSnail, hit.HitDirection, -1f, 0, default, 1f);

                if (NPC.life <= 0) {
                    for (int k = 0; k < 30; k++)
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SeaSnail, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
                    for (int i = 0; i < 5; i++) {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore1").Type, Main.rand.NextFloat(1f, 1.4f));
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore2").Type, Main.rand.NextFloat(1.2f, 1.6f));
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>("Consolaria/OcramGore3").Type, Main.rand.NextFloat(1f, 1.4f));
                    }
                    SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                    SoundEngine.PlaySound(SoundID.Roar with { Pitch = 0.25f, MaxInstances = 0 }, NPC.Center);
                }
            }
        }

        public override void OnKill () {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedOcram, -1);
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                if (bloodMoonMode && Main.rand.NextBool(10))
                    Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.Consumables.HolyHandgrenade2>(), Main.rand.Next(5, 11), false, 0, false, false);
            }
        }

        public override void BossLoot (ref string name, ref int potionType)
            => potionType = ItemID.GreaterHealingPotion;

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OcramTrophy>(), 10));

            Conditions.NotExpert notExpert = new Conditions.NotExpert();
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<OcramBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<OcramRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<CursedFang>(), 4));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(new OneFromOptionsDropRule(1, 1, ModContent.ItemType<EternityStaff>(), ModContent.ItemType<DragonBreath>(), ModContent.ItemType<OcramsEye>(), ModContent.ItemType<Tizona>()));
            npcLoot.Add(notExpertRule);
            npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SoulofBlight>(), 1, 25, 40));
            npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OcramMask>(), 7));
        }
    }
}