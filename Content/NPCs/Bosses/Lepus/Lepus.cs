using Consolaria.Common;
using Consolaria.Content.Items.Armor.Misc;
using Consolaria.Content.Items.BossDrops.Lepus;
using Consolaria.Content.Items.Miscellaneous;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    [AutoloadBossHead]
    internal class Lepus : ConsolariaModBoss {
        private enum States {
            Appearance,
            Appearance2,
            HeavyJump,
            Stand,
            DoJump,
            DoExtraJump,
            Jumping,
            Fall,
            PlayersDead
        }

        private enum Frame {
            Stand1,
            Stand2,
            Stand3,
            Stand4,
            Jump1,
            Jump2,
            Spawn
        }

        private const string MUSIC_PATH = "Assets/Music/Lepus";

        private const short STATE_APPEARANCE = (short) States.Appearance;
        private const short STATE_APPEARANCE2 = (short) States.Appearance2;
        private const short STATE_HEAVY_JUMP = (short) States.HeavyJump;
        private const short STATE_STAGNANT = (short) States.Stand;
        private const short STATE_JUMP = (short) States.DoJump;
        private const short STATE_ADVANCED_JUMP = (short) States.DoExtraJump;
        private const short STATE_JUMP2 = (short) States.Jumping;
        private const short STATE_FALLING = (short) States.Fall;
        private const short STATE_DEAD_PLAYERS = (short) States.PlayersDead;

        private const short HITBOX_SIZE_X = 60;
        private const short HITBOX_SIZE_Y = 50;
        private const short FRAME_WIDTH = 100;
        private const short FRAME_HEIGHT = 76;
        private const short STANDING_FRAMES_COUNT = 4;
        private const short MAX_JUMP_COUNT = 2;

        private readonly int MAX_ADVANCED_JUMP_COUNT = Main.expertMode ? 8 : 6;

        private const float MAX_DISTANCE = 2000f;

        public bool AdvancedJumped {
            get;
            set;
        } = false;

        public bool JustSpawned {
            get;
            set;
        } = true;

        public bool AdvancedJumped2 {
            get;
            set;
        } = false;

        public bool SpawnedStomp {
            get;
            set;
        } = false;

        public float Opacity {
            get;
            set;
        } = 1.15f;

        public override void ReceiveExtraAI (BinaryReader reader) {
            AdvancedJumped = reader.ReadBoolean();
            JustSpawned = reader.ReadBoolean();
            AdvancedJumped2 = reader.ReadBoolean();
            SpawnedStomp = reader.ReadBoolean();
            Opacity = reader.ReadSingle();
        }

        public override void SendExtraAI (BinaryWriter writer) {
            writer.Write(AdvancedJumped);
            writer.Write(JustSpawned);
            writer.Write(AdvancedJumped2);
            writer.Write(SpawnedStomp);
            writer.Write(Opacity);
        }

        public ref float JumpCount
            => ref NPC.ai [2];

        public ref float AdvancedJumpCount
            => ref NPC.ai [3];

        public override void SetStaticDefaults () {
            base.SetStaticDefaults();

            NPCID.Sets.TrailingMode [Type] = 1;

            DisplayName.SetDefault(nameof(Lepus));

            Main.npcFrameCount [Type] = 7;

            NPCID.Sets.MPAllowedEnemies [Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) {
                CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/Lepus_Bestiary",
                Position = new Vector2(24f, 12f),
                PortraitPositionXOverride = 10f,
                PortraitPositionYOverride = -5f,
                PortraitScale = 1f,
                Scale = 1f
            };
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int [] {
                    BuffID.Confused
                }
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            base.SetDefaults();

            NPC.width = HITBOX_SIZE_X;
            NPC.height = HITBOX_SIZE_Y;

            short lifeMax = 3000;
            NPC.lifeMax = lifeMax;
            short damage = 35;
            NPC.damage = damage;
            short defense = 8;
            NPC.defense = defense;

            NPC.value = Item.buyPrice(silver: 50);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.SpawnWithHigherTime(100);
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.Opacity = 0f;

            NPC.npcSlots = 50f;

            NPC.gfxOffY = -4;
            FrameWidth = FRAME_WIDTH;
            FrameHeight = FRAME_HEIGHT;

            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, MUSIC_PATH);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * 1.25f);
            NPC.damage = (int)(NPC.damage * 0.65f);
            if (numPlayers <= 1) return;
            float healthBoost = 0.35f;
            for (int k = 1; k < numPlayers; k++)
            {
                NPC.lifeMax += (int)(NPC.lifeMax * healthBoost);
                healthBoost += (1 - healthBoost) / 3;
            }
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry)
            => bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("As if its size wasn't troublesome enough, this rabbit can reproduce through colorful eggs, spread around the world in Spring for fools to pick up.")
            });

        public override void OnKill () {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedLepus, -1);
            if (RabbitInvasion.rabbitInvasion) {
                RabbitInvasion.rabbitInvasion = false;
                RabbitInvasion.rabbitKilledCount = 0;
                string text = "Bunnies are retreating!";
                if (Main.netMode == NetmodeID.SinglePlayer) {
                    Main.NewText(text, new Color(50, 255, 130));
                }
                else if (Main.netMode == NetmodeID.Server) {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), new Color(50, 255, 130));
                }
            }
            if (Main.netMode == NetmodeID.Server) {
                NetMessage.SendData(MessageID.WorldData);
            }
        }

        public override void BossLoot (ref string name, ref int potionType)
            => potionType = NPC.CountNPCS(Type) <= 1 ? ItemID.LesserHealingPotion : -1;

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            LepusDropCondition lepusDropCondition = new();
            IItemDropRule conditionalRule = new LeadingConditionRule(lepusDropCondition);
            Conditions.NotExpert notExpert = new();
            conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LepusTrophy>(), 10));
            conditionalRule.OnSuccess(new OneFromRulesRule(1, ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraHat>()), ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraJacket>()), ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraBoots>())));
            conditionalRule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<LepusBag>()));
            conditionalRule.OnSuccess(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LepusRelic>()));
            conditionalRule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<RabbitFoot>(), 4));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<EggCannon>(), 2));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<LepusMask>(), 8));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ItemID.BunnyHood, 10));
            conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SuspiciousLookingEgg>()));
            LepusDropCondition1 lepusDropCondition2 = new();
            IItemDropRule conditionalRule2 = new LeadingConditionRule(lepusDropCondition2);
            conditionalRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldenCarrot>(), minimumDropped: 10, maximumDropped: 20));
            npcLoot.Add(conditionalRule);
            npcLoot.Add(conditionalRule2);
        }

        public override bool CheckDead ()
            => NPC.CountNPCS(Type) <= 1 && State != STATE_DEAD_PLAYERS;

        public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            SpriteEffects effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(FrameWidth, FrameHeight) / 2f;
            bool didAdvancedJump = AdvancedJumped && Math.Abs(NPC.velocity.X) > 0.5f;
            bool playersDead = State == STATE_DEAD_PLAYERS;
            float offsetY = -10f;
            bool doJump = State == STATE_JUMP && StateTimer >= 25;
            Vector2 position = NPC.Center - screenPos + new Vector2(0f, offsetY);
            if (!playersDead) {
                if (Opacity > 0f) {
                    float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f;
                    float alphaX = Math.Max(0.1f, (Math.Abs(NPC.velocity.X) > 5f ? 5f : Math.Abs(NPC.velocity.X)) / 3f);
                    float alphaY = Math.Max(0.1f, (Math.Abs(NPC.velocity.Y) > 5f ? 5f : Math.Abs(NPC.velocity.Y)) / 3f);
                    if (doJump) {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                        spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, offsetY), new Rectangle?(NPC.frame), Utils.MultiplyRGB(Color.HotPink, drawColor) * NPC.Opacity * (Opacity < 0.5f ? 1f - Opacity : Opacity), NPC.rotation, origin, NPC.scale * (2f - (Opacity + 0.5f)), effects, 0f);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                    for (int i = 1; i < NPC.oldPos.Length - 1; i += 2) {
                        Color color = NPC.GetAlpha(Color.Multiply(Utils.MultiplyRGB(Color.HotPink, drawColor), (10 - i) / 15f * 1.5f));
                        float alpha = didAdvancedJump ? alphaX : alphaY;
                        color *= (float) ((NPC.oldPos.Length - i) / 15f) * 0.85f;
                        color *= alpha;
                        spriteBatch.Draw(texture, new Vector2(NPC.oldPos [i].X - screenPos.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.oldPos [i].Y - screenPos.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale), new Rectangle?(NPC.frame), color * 1.25f * NPC.Opacity * Opacity, NPC.rotation, origin, scale * Opacity * alpha * 0.5f, effects, 0f);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                        spriteBatch.Draw(texture, new Vector2(NPC.oldPos [i].X - screenPos.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.oldPos [i].Y - screenPos.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale), new Rectangle?(NPC.frame), NPC.GetAlpha(Color.Multiply(Utils.MultiplyRGB(Color.HotPink, drawColor), (10 - i) / 15f * 1.5f)) * 0.75f * NPC.Opacity * Opacity, NPC.rotation, origin, scale * Opacity * alpha * 0.3f, effects, 0f);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                    for (int i = 0; i < 5; i++) {
                        spriteBatch.Draw(texture, position + new Vector2(0f, 0.5f).RotatedBy((double) i * Math.PI + (double) Main.GlobalTimeWrappedHourly * (double) 4f) * 2f * 4f, new Rectangle?(NPC.frame), Utils.MultiplyRGB(Color.HotPink, drawColor) * 1.25f * NPC.Opacity * 0.666f * Opacity, NPC.rotation + Utils.NextFloat(Main.rand, -0.05f, 0.05f), origin, NPC.scale * Opacity * 0.5f * (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f, effects, 0f);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                        spriteBatch.Draw(texture, position + new Vector2(1f, -0.5f).RotatedBy((double) -i * Math.PI + (double) Main.GlobalTimeWrappedHourly * (double) 3f) * 1.75f * 4f, new Rectangle?(NPC.frame), Utils.MultiplyRGB(Color.HotPink, drawColor) * 0.75f * NPC.Opacity * 0.66f * Opacity, NPC.rotation + Utils.NextFloat(Main.rand, -0.05f, 0.05f), origin, NPC.scale * Opacity * 0.4f * (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f, effects, 0f);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                }
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, offsetY), new Rectangle?(NPC.frame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, effects, 0f);
            return false;
        }

        public override void AI () {
            switch (State) {
            case STATE_APPEARANCE:
            Appearance();
            break;
            case STATE_APPEARANCE2:
            Appearance2();
            break;
            case STATE_HEAVY_JUMP:
            HeavyJump();
            break;
            case STATE_STAGNANT:
            HasTroubles();
            int target = NPC.target;
            bool flag = target < 0 || target == 255;
            Player player = Main.player [target];
            if (player.dead || flag) {
                NPC.TargetClosest();
                player = Main.player [NPC.target];
                if (player.dead || flag) {
                    ChangeState(STATE_DEAD_PLAYERS);
                    return;
                }
            }
            Stagnant();
            break;
            case STATE_JUMP:
            HasTroubles();
            MakeJump();
            break;
            case STATE_ADVANCED_JUMP:
            HasTroubles();
            MakeExtraJump();
            break;
            case STATE_JUMP2:
            HasTroubles();
            Jump();
            break;
            case STATE_FALLING:
            HasTroubles();
            Falling();
            break;
            case STATE_DEAD_PLAYERS:
            ByeWords();
            break;
            }
            if (NPC.velocity.Y != 0f && SpawnedStomp) {
                SpawnedStomp = false;
            }
            bool didAdvancedJump = AdvancedJumped && Math.Abs(NPC.velocity.X) > 0.5f;
            bool doHeavyJump = State == STATE_HEAVY_JUMP;
            bool doSpawnBigEgg = AdvancedJumpCount >= MAX_JUMP_COUNT && !AdvancedJumped2;
            bool doJump = State == STATE_JUMP && StateTimer >= 25;
            if (didAdvancedJump || doHeavyJump || doSpawnBigEgg || doJump) {
                if (Opacity < 1f) {
                    Opacity += 0.025f;
                }
            }
            else {
                if (Opacity > 0f) {
                    Opacity -= 0.01f;
                }
            }
        }

        private void MoveBottom () {
            int tileX = Math.Max(10, Math.Min(Main.maxTilesX - 10, (int) ((NPC.position.X + (NPC.width * 0.5f) + (float) (((NPC.width * 0.5f) + 8f) * NPC.direction)) / 16f)));
            int tileY = Math.Max(10, Math.Min(Main.maxTilesY - 10, (int) ((NPC.position.Y + (float) NPC.height - 15f) / 16f)));
            if (Main.tile [tileX, tileY + 1].TileType == TileID.Platforms && Main.player [NPC.target].Bottom.Y - NPC.Top.Y - 50 > 50f) {
                NPC.position.Y += 1f;
            }
        }

        private void HasTroubles () {
            if (State != STATE_APPEARANCE && State != STATE_HEAVY_JUMP && ((Collision.SolidCollision(NPC.Center - new Vector2(8, 8), 16, 16) && !Collision.CanHitLine(NPC.Center, 0, 0, Main.player [NPC.target].Center, 0, 0)) || Vector2.Distance(Main.player [NPC.target].Center, NPC.Center) > 1500f)) {
                ChangeState(STATE_APPEARANCE2, 0f);
                NPC.netUpdate = true;
            }
            MoveBottom();
        }

        public override void FindFrame (int frameHeight) {
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player [NPC.target];
            Vector2 center = NPC.Center;
            int toPlayer = player.Center.X < center.X ? -1 : 1;
            int currentFrame;
            switch (State) {
            case STATE_STAGNANT:
            bool flag = AdvancedJumped2;
            if (Math.Abs(NPC.velocity.X) < 0.5f || flag) {
                NPC.direction = toPlayer;
            }
            double rate = 0.125;
            if ((NPC.frameCounter += rate) > STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT) {
                NPC.frameCounter = 0.0;
            }
            currentFrame = (int) Frame.Stand1 + (int) (NPC.frameCounter % STANDING_FRAMES_COUNT);
            NPC.frame.Y = (flag ? (int) Frame.Spawn : currentFrame) * frameHeight;
            break;
            case STATE_APPEARANCE:
            case STATE_APPEARANCE2:
            case STATE_HEAVY_JUMP:
            NPC.direction = 1;
            NPC.frame.Y = (int) Frame.Jump2 * frameHeight;
            break;
            case STATE_JUMP:
            NPC.frame.Y = (int) Frame.Stand3 * frameHeight;
            break;
            case STATE_ADVANCED_JUMP:
            rate = 0.125;
            if ((NPC.frameCounter += rate) > STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT) {
                NPC.frameCounter = 0.0;
            }
            currentFrame = (int) Frame.Stand1 + (int) (NPC.frameCounter % STANDING_FRAMES_COUNT);
            NPC.direction = toPlayer;
            NPC.frame.Y = (NPC.velocity.Y < 0f ? (int) Frame.Jump1 : NPC.velocity.Y == 0f ? currentFrame : (int) Frame.Jump2) * frameHeight;
            break;
            case STATE_JUMP2:
            break;
            case STATE_FALLING:
            NPC.direction = AdvancedJumpCount >= MAX_JUMP_COUNT ? toPlayer : NPC.velocity.X < 0f ? -1 : 1;
            NPC.frame.Y = (NPC.velocity.Y < 0f ? (int) Frame.Jump1 : (int) Frame.Jump2) * frameHeight;
            break;
            case STATE_DEAD_PLAYERS:
            rate = 0.125;
            if ((NPC.frameCounter += rate) > STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT) {
                NPC.frameCounter = 0.0;
            }
            currentFrame = StateTimer <= 90 ? (int) Frame.Spawn : (int) Frame.Stand1 + (int) (NPC.frameCounter % STANDING_FRAMES_COUNT);
            NPC.direction = toPlayer;
            NPC.frame.Y = (NPC.velocity.Y == 0f ? currentFrame : (NPC.velocity.Y < 0f ? (int) Frame.Jump1 : (int) Frame.Jump2)) * frameHeight;
            break;
            }
        }

        private void GettingHit () {
            if (State != STATE_JUMP) {
                return;
            }
            if (TooFar() && State != STATE_JUMP2) {
                Dusts();
                AdvancedJumped = false;
                ChangeState(STATE_JUMP2);
                NPC.netUpdate = true;
            }
        }

        private void Dusts () {
            SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/LepusFaildJump");
            SoundEngine.PlaySound(style, NPC.Center);
            if (Main.netMode != NetmodeID.Server) {
                for (int index1 = 0; index1 < 8; ++index1) {
                    int dust = Dust.NewDust(NPC.TopLeft - new Vector2(20, 60), NPC.width + 40, NPC.height + 40, ModContent.DustType<Dusts.EggDust>(), 0, 0, 0, default(Color), Main.rand.NextFloat(0.9f, 1.1f));
                    Main.dust [dust].velocity.X = 0;
                    Main.dust [dust].velocity.Y = 0.8f;
                }
            }
        }

        public override void HitEffect (int hitDirection, double damage) {
            if (Main.netMode == NetmodeID.Server || State == STATE_DEAD_PLAYERS) {
                return;
            }
            if (NPC.life <= 0) {
                for (int i = 1; i <= 7; i++) {
                    int gore = ModContent.Find<ModGore>(string.Concat("Consolaria/LPG", i.ToString())).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), gore);
                }
            }
        }

        private void Appearance () {
            NPC.dontTakeDamage = false;
            NPC.velocity.X = NPC.velocity.Y = 0f;
            int target = NPC.target;
            if (target < 0 || target == 255) {
                NPC.TargetClosest();
            }
            if (StateTimer != 1f) {
                float offsetY = Main.expertMode ? 350f : 425f;
                Vector2 center = Main.player [NPC.target].Center;
                if (StateTimer != -1f) {
                    NPC.Center = center - new Vector2(0f, offsetY);
                }
                else {
                    NPC.Center = center - new Vector2(Main.rand.Next(50, 150) * (Main.rand.NextBool() ? 1 : -1), offsetY);
                }
            }
            ChangeState(STATE_HEAVY_JUMP, 0f);
            NPC.netUpdate = true;
        }

        private void Appearance2 () {
            NPC.rotation = NPC.velocity.Y / 25f;
            NPC.dontTakeDamage = NPC.noTileCollide = true;
            if (NPC.Opacity > 0f) {
                NPC.Opacity -= 0.01f;
                NPC.Opacity *= 0.9f;
            }
            else {
                ChangeState(STATE_APPEARANCE, -1f);
                NPC.netUpdate = true;
            }
        }

        private void HeavyJump () {
            NPC.noTileCollide = Main.player [NPC.target].Bottom.Y - NPC.Top.Y > 100f;
            NPC.rotation = NPC.velocity.Y / 10f;
            if (NPC.Opacity != 1f) {
                NPC.Opacity += 0.01f;
                NPC.Opacity *= 1.1f;
            }
            float slow = Main.expertMode ? 0.895f : 0.925f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX) {
                NPC.velocity.X = 0f;
            }
            if (!NPC.collideY && NPC.velocity.Y != 0f) {
                float velocityYSpeed = Main.expertMode ? 5f : 2.5f;
                float acceleration = velocityYSpeed / 2.5f;
                NPC.velocity.Y += velocityYSpeed;
                NPC.velocity.Y *= acceleration;
                NPC.velocity.Y = Math.Min(NPC.velocity.Y, 20f);
            }
            if (NPC.velocity.Length() != 0f) {
                return;
            }
            SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 50; i++) {
                    int dust = Dust.NewDust(NPC.Bottom - new Vector2(NPC.width / 2, 30f), NPC.width, 30, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, Color.GhostWhite, Main.rand.NextFloat() * 1.5f + Main.rand.NextFloat() * 1.5f);
                    Main.dust [dust].noGravity = true;
                    Main.dust [dust].velocity.Y = -3.5f + Main.rand.NextFloat() * -3f;
                    Main.dust [dust].velocity.X *= 7f;
                }
            }
            ChangeState(STATE_STAGNANT);
            SpawnStomp();
            NPC.netUpdate = true;
        }

        private void Stagnant () {
            Player player = Main.player [NPC.target];
            Vector2 center = NPC.Center;
            float slow = Main.expertMode ? 0.895f : 0.925f;
            NPC.velocity.X *= slow;
            NPC.rotation = 0f;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (NPC.Opacity != 1f) {
                NPC.Opacity = 1f;
            }
            if (zeroVelocityX) {
                NPC.velocity.X = 0f;
            }
            bool hasTargetAndClose = NPC.HasValidTarget;
            if (hasTargetAndClose && player.active && !player.dead) {
                if (player.Distance(center) >= MAX_DISTANCE) {
                    return;
                }
                bool zeroVelocity = NPC.velocity.Y != 0f | NPC.velocity.X != 0f;
                if (zeroVelocity) {
                    return;
                }
                bool expertMode = Main.expertMode;
                int attackTime = (int) MathHelper.Lerp(!expertMode ? 30f : 15f, !expertMode ? 100f : 85f, NPC.life / (float) NPC.lifeMax);
                if (++StateTimer >= attackTime) {
                    if (AdvancedJumpCount >= MAX_JUMP_COUNT) {
                        if ((float) Main.rand.NextDouble() < 0.333f && Main.expertMode) {
                            JumpCount = 0;
                            ChangeState(STATE_ADVANCED_JUMP);
                            NPC.netUpdate = true;
                            return;
                        }
                    }
                    else {
                        JumpCount++;
                    }
                    ChangeState(STATE_JUMP);
                    NPC.netUpdate = true;
                }
                else if (StateTimer >= attackTime / 3) {
                    AdvancedJumped = false;
                    AdvancedJumped2 = false;
                }
            }
            else {
                NPC.TargetClosest(true);
            }
        }

        private void MakeJump () {
            NPC.rotation = NPC.velocity.Y / 25f;
            bool flag = AdvancedJumpCount >= MAX_JUMP_COUNT;
            bool expertMode = Main.expertMode;
            int attackTime = (int) MathHelper.Lerp(!expertMode ? 45f : 30f, !expertMode ? 75f : 60f, NPC.life / (float) NPC.lifeMax);
            bool flag2 = StateTimer <= attackTime / 2 + attackTime / 4;
            bool flag3 = StateTimer > attackTime / 10;
            if (TooFar() && !JustSpawned && !flag) {
                if (NPC.justHit && flag2) {
                    GettingHit();
                    return;
                }
                float slow = 0.35f;
                int jumpStrength;
                NPC.velocity.Y *= slow;
                NPC.velocity.X *= slow;
                if (++StateTimer > attackTime) {
                    jumpStrength = 15;
                    SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
                    ChangeState(STATE_JUMP2, jumpStrength);
                    NPC.netUpdate = true;
                    return;
                }
                else if (StateTimer % 3 == 0) {
                    Vector2 eyeCenter = NPC.Center + new Vector2(12 * NPC.direction, -12);
                    int dust2 = Dust.NewDust(NPC.position - new Vector2(20, 20), NPC.width + 40, NPC.height + 40, DustID.Smoke, 0, 0, 75, Color.HotPink, Main.rand.NextFloat(1.2f, 1.7f));
                    Main.dust [dust2].position = eyeCenter + new Vector2(0, -80).RotatedByRandom(Math.PI * 2f);
                    Main.dust [dust2].velocity = Vector2.Normalize(Main.dust [dust2].position - eyeCenter) * -2f;
                    Main.dust [dust2].fadeIn = 1f;
                }
            }
            else {
                if (!JustSpawned && !flag) {
                    Dusts();
                }
                SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
                ChangeState(STATE_JUMP2);
                NPC.netUpdate = true;
            }
        }

        private void MakeExtraJump () {
            NPC.rotation = NPC.velocity.Y / 25f;
            NPC.noTileCollide = false;
            Player player = Main.player [NPC.target];
            if ((NPC.Center.X > player.Center.X ? (NPC.Center.X - player.Center.X) : (player.Center.X - NPC.Center.X)) < 10 && NPC.Center.Y < player.Center.Y - 150 && JumpCount >= 3) {
                ChangeState(STATE_APPEARANCE, 1f);
                AdvancedJumpCount = 0;
                JumpCount = 0;
                JustSpawned = true;
                AdvancedJumped = false;
                AdvancedJumped2 = true;
                SpawnBigEgg();
                return;
            }
            if (NPC.velocity.Y != 0f) {
                return;
            }
            SpawnStomp();
            float slow = 0.85f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX) {
                NPC.velocity.X = 0f;
            }
            int rate = (int) MathHelper.Lerp(Main.rand.Next(5, 10), Main.rand.Next(1, 6), NPC.life / (float) NPC.lifeMax);
            if (++StateTimer % rate == 0) {
                if (JumpCount >= 4) {
                    JumpCount = 0;
                }
                int jumpStrength = (int) JumpCount + 2;
                StateTimer = jumpStrength;
                ExtraJump();
                NPC.netUpdate = true;
                return;
            }
            if (StateTimer <= 0) {
                NPC.velocity.Y += 6f;
            }
        }

        private void ExtraJump () {
            float slow = 0.85f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX) {
                NPC.velocity.X = 0f;
            }
            if (NPC.velocity.Y != 0f || NPC.velocity.X != 0f) {
                return;
            }
            SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
            NPC.noTileCollide = true;
            while (StateTimer > 0) {
                NPC.velocity.Y -= Main.rand.NextFloat(2f, 5f) * Main.rand.NextFloat(1.1f, 1.75f) * 0.5f;
                NPC.velocity.X += Main.rand.NextFloat(2f, 5f) * 0.75f * NPC.direction;
                StateTimer--;
                NPC.netUpdate = true;
            }
            JumpCount++;
        }

        private void Jump () {
            NPC.rotation = NPC.velocity.Y / 25f;
            JustSpawned = false;
            NPC.noTileCollide = true;
            Player player = Main.player [NPC.target];
            Vector2 center = NPC.Center;
            Vector2 playerCenter = player.Center;
            float offsetY = 220f;
            float rotation = (float) Math.Atan2(center.Y - (playerCenter.Y - offsetY), center.X - playerCenter.X);
            NPC.velocity.X = 7 * NPC.direction;
            NPC.velocity.Y = -(float) (Math.Sin(rotation) * 14.0);
            if (--StateTimer > 0) {
                if (!AdvancedJumped) {
                    AdvancedJumped = true;
                }
                if (!AdvancedJumped2) {
                    AdvancedJumped2 = true;
                }
                NPC.velocity.X += 4.25f * NPC.direction;
                NPC.velocity.Y -= 2f;
                NPC.velocity *= 1.075f;
                return;
            }
            if (AdvancedJumpCount >= MAX_ADVANCED_JUMP_COUNT + 1) {
                AdvancedJumpCount = 0;
                SpawnBigEgg();
            }
            ChangeState(STATE_FALLING, (int) NPC.velocity.X);
            NPC.netUpdate = true;
        }

        private void Falling () {
            NPC.rotation = NPC.velocity.Y / 25f;
            bool flag = false;
            bool flag2 = AdvancedJumpCount >= MAX_JUMP_COUNT;
            bool flag3 = AdvancedJumpCount >= MAX_JUMP_COUNT + 2;
            if (--StateTimer > 0) {
                return;
            }
            NPC.noTileCollide = false;
            if (NPC.velocity.Y != 0f) {
                return;
            }
            if (AdvancedJumped) {
                SpawnEggs();
            }
            if (JumpCount >= (Main.expertMode ? MAX_JUMP_COUNT : MAX_JUMP_COUNT + 1)) {
                JumpCount = 0;
                if (!flag2) {
                    AdvancedJumpCount++;
                }
                flag = true;
            }
            if (flag2) {
                AdvancedJumpCount++;
            }
            if (flag3) {
                flag = true;
            }
            SpawnStomp();
            ChangeState(flag ? STATE_JUMP2 : STATE_STAGNANT);
            NPC.TargetClosest();
            NPC.netUpdate = true;
            if (flag) {
                SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
            }
        }

        private void SpawnStomp () {
            if (NPC.oldVelocity.Length() < 8f || SpawnedStomp) {
                return;
            }
            SpawnedStomp = true;
            SoundEngine.PlaySound(SoundID.Item73 with { Volume = 0.5f, PitchRange = (-0.25f, 0.25f) }, NPC.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                bool expertMode = Main.expertMode;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Stomp>(), expertMode ? NPC.damage / 3 : 0, 2f, Main.myPlayer, 0f, (expertMode ? Main.rand.NextFloat(75f, 90f) : Main.rand.NextFloat(40f, 65f)) / 3f);
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
            }
        }

        private void SpawnEggs () {
            if (AdvancedJumped && !Main.expertMode) {
                AdvancedJumped = false;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                return;
            }
            for (int i = -2; i < 1; i++) {
                Vector2 velocity = new Vector2(13f, 0f);
                velocity = velocity.RotatedBy((double) ((float) (float) -i * MathHelper.TwoPi / 10f), Vector2.Zero);
                int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X, (int) NPC.Center.Y, ModContent.NPCType<SmallEgg>());
                int toPlayer = Main.player [NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                Main.npc [index].velocity.X = velocity.X * toPlayer;
                Main.npc [index].velocity.Y = -velocity.Y / 2f;
                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs) {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }
            JustSpawned = true;
        }

        private void SpawnBigEgg () {
            if (!AdvancedJumped2) {
                AdvancedJumped2 = true;
            }
            if (!AdvancedJumped && Main.expertMode) {
                AdvancedJumped = true;
            }
            int type = ModContent.NPCType<BigEgg>();
            if (Main.netMode == NetmodeID.MultiplayerClient || NPC.CountNPCS(type) > 1) {
                return;
            }
            int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X, (int) NPC.Center.Y, type);
            if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs) {
                NetMessage.SendData(MessageID.SyncNPC, number: index);
            }
        }

        private void ByeWords () {
            float slow = Main.expertMode ? 0.895f : 0.925f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX) {
                NPC.velocity.X = 0f;
            }
            if (StateTimer >= 270) NPC.noTileCollide = true;
            if (StateTimer >= 280) {
                if (NPC.Opacity != 0f) {
                    if (NPC.velocity.Y > 0f) {
                        NPC.Opacity -= 0.01f;
                        NPC.Opacity *= 0.9f;
                    }
                }
                else {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    if (RabbitInvasion.rabbitInvasion) {
                        RabbitInvasion.rabbitInvasion = false;
                        RabbitInvasion.rabbitKilledCount = 0;
                    }
                    if (Main.netMode == NetmodeID.Server) {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    if (Main.netMode == NetmodeID.Server) {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            else if (++StateTimer >= 150) {
                if (NPC.velocity.Y == 0f && JumpCount <= 5) {
                    SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
                    NPC.velocity.Y -= Main.rand.NextFloat(2f, 5f) * Main.rand.NextFloat(1.1f, 1.75f) * 0.5f * (JumpCount + 3) / 2;
                    NPC.velocity.X += Main.rand.NextFloat(2f, 5f) * Main.rand.NextFloat(1.1f, 1.75f) * 0.5f * NPC.direction;
                    JumpCount++;
                    NPC.netUpdate = true;
                }
            }
        }

        private bool TooFar () {
            Player player = Main.LocalPlayer;
            Vector2 center = NPC.Center;
            float f = Main.expertMode ? 8.5f : 7f;
            if (Main.netMode != NetmodeID.SinglePlayer) {
                if (Main.player.Length == 1) {
                    return (Main.expertMode && !Collision.CanHitLine(center, 10, 10, player.Center, 2, 2)) || player.Distance(center) > MAX_DISTANCE / f;
                }
                for (int i = 0; i < Main.player.Length; i++) {
                    Player player2 = Main.player [i];
                    if (i + 1 > Main.player.Length - 1) {
                        player = player2;
                        break;
                    }
                    Player player3 = Main.player [i + 1];
                    if (player2.active && !player2.dead && player3.active && !player3.dead) {
                        player = Math.Abs(player2.Center.X - center.X) > Math.Abs(player3.Center.X - center.X) ? player2 : player3;
                    }
                }
            }
            return (Main.expertMode && !Collision.CanHitLine(center, 10, 10, player.Center, 2, 2)) || player.Distance(center) > MAX_DISTANCE / f;
        }
    }
}