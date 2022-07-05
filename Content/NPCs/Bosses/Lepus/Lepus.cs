using Consolaria.Common;
using Consolaria.Content.Items.Armor.Misc;
using Consolaria.Content.Items.BossDrops.Lepus;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus
{
    [AutoloadBossHead]
    internal class Lepus : ConsolariaModBoss
	{
        private enum States
        {
            Appearance,
            HeavyJump,
            Stand,
            DoJump,
            DoExtraJump,
            Jumping,
            Fall
        }

        private enum Frame
        {
            Stand1,
            Stand2,
            Stand3,
            Stand4,
            Jump1,
            Jump2,
            Spawn
        }

        private const string MUSIC_PATH = "Assets/Music/Lepus";

        private const short HITBOX_SIZE_X = 60;
        private const short HITBOX_SIZE_Y = 50;
        private const short FRAME_WIDTH = 100;
        private const short FRAME_HEIGHT = 76;
        private const short STANDING_FRAMES_COUNT = 4;
        private const short MAX_JUMP_COUNT = 2;
        private const short MAX_ADVANCED_JUMP_COUNT = 6;

        private const float MAX_DISTANCE = 2000f;

        public bool AdvancedJumped
        {
            get => NPC.localAI[0] == 1f;
            set => NPC.localAI[0] = value ? 1f : 0f;
        }

        public bool JustSpawned
        {
            get => NPC.localAI[1] == 0f;
            set => NPC.localAI[1] = value ? 0f : 1f;
        }

        public bool AdvancedJumped2
        {
            get => NPC.localAI[2] == 1f;
            set => NPC.localAI[2] = value ? 1f : 0f;
        }

        public ref float JumpCount
            => ref NPC.ai[2];

        public ref float AdvancedJumpCount
            => ref NPC.ai[3];

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            NPCID.Sets.TrailingMode[Type] = 1;

            DisplayName.SetDefault(nameof(Lepus));

            Main.npcFrameCount[Type] = 7;

            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                CustomTexturePath = "Consolaria/Assets/Textures/NPCs/Lepus_Bestiary",
                Position = new Vector2(24f, 12f),
                PortraitPositionXOverride = 10f,
                PortraitPositionYOverride = -5f,
                PortraitScale = 1.25f,
                Rotation = (float)Math.PI / 2f + 0.5f,
                Scale = 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            NPC.width = HITBOX_SIZE_X;
            NPC.height = HITBOX_SIZE_Y;

            short lifeMax = 3500;
            NPC.lifeMax = lifeMax;
            short damage = 40;
            NPC.damage = damage;
            short defense = 8;
            NPC.defense = defense;

            NPC.value = (float)Item.buyPrice(gold: 4);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.SpawnWithHigherTime(30);
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.Opacity = 0f;

            NPC.gfxOffY = -4;
            FrameWidth = FRAME_WIDTH;
            FrameHeight = FRAME_HEIGHT;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, MUSIC_PATH);
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale + numPlayers);
            NPC.damage = (int)(NPC.damage * 0.6f);
            NPC.defense += numPlayers;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
            => bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> 
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("A hare of incredible size, capable of producing weak copies of itself as quickly as chewing the unfortunate Terrarian with its huge teeth.")
            });

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            short debuffTime = 120;
            short debuffTime2 = 180;
            target.AddBuff(BuffID.Slow, Main.expertMode ? debuffTime2 : debuffTime);
        }

        public override void OnKill()
            => NPC.SetEventFlagCleared(ref DownedBossSystem.downedLepus, -1);

        public override void BossLoot(ref string name, ref int potionType)
            => potionType = NPC.CountNPCS(Type) <= 1 ? ItemID.LesserHealingPotion : -1;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LepusDropCondition lepusDropCondition = new();
            IItemDropRule conditionalRule = new LeadingConditionRule(lepusDropCondition);
            Conditions.NotExpert notExpert = new();
            conditionalRule.OnSuccess(new OneFromRulesRule(1, ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraHat>()), ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraJacket>()), ItemDropRule.ByCondition(notExpert, ModContent.ItemType<OstaraBoots>())));
            conditionalRule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<LepusBag>()));
            conditionalRule.OnSuccess(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LepusRelic>()));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<EggCannon>(), 2));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<LepusMask>(), 8));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<LepusTrophy>(), 10));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ItemID.BunnyHood, 10));
            conditionalRule.OnSuccess(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SuspiciousLookingEgg>()));
            npcLoot.Add(conditionalRule);
        }

        public override bool CheckDead()
            => NPC.CountNPCS(Type) <= 1;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            SpriteEffects effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(FrameWidth, FrameHeight) / 2f;
            bool didAdvancedJump = AdvancedJumped && Math.Abs(NPC.velocity.X) > 0.5f;
            bool doHeavyJump = State == (int)States.HeavyJump;
            bool doSpawnBigEgg = AdvancedJumpCount >= MAX_JUMP_COUNT;
            if (didAdvancedJump || doHeavyJump)
            {
                float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f;
                float alphaX = Math.Max(0.1f, (Math.Abs(NPC.velocity.X) > 5f ? 5f : Math.Abs(NPC.velocity.X)) / 3f);
                float alphaY = Math.Max(0.1f, (Math.Abs(NPC.velocity.Y) > 5f ? 5f : Math.Abs(NPC.velocity.Y)) / 3f);
                for (int i = 1; i < NPC.oldPos.Length - 1; i += 2)
                {
                    Color color = NPC.GetAlpha(Color.Multiply(Utils.MultiplyRGB(Color.HotPink, drawColor), (float)(10 - i) / 15f * 1.5f));
                    float alpha = didAdvancedJump ? alphaX : alphaY;
                    color *= (float)((NPC.oldPos.Length - i) / 15f) * 0.85f;
                    color *= alpha;
                    spriteBatch.Draw(texture, new Vector2(NPC.oldPos[i].X - screenPos.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.oldPos[i].Y - screenPos.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color * 1.25f * NPC.Opacity, NPC.rotation, origin, scale * alpha * 0.5f, effects, 0f);
                }
            }
            float offsetY = -10f;
            if (doSpawnBigEgg && NPC.velocity.Y == 0f)
			{
                Color color = NPC.GetAlpha(Utils.MultiplyRGB(Color.HotPink, drawColor));
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, offsetY), new Rectangle?(NPC.frame), color * NPC.Opacity, NPC.rotation, origin, NPC.scale * 0.525f * ((Main.mouseTextColor / 200f - 0.35f) * 0.75f + 0.8f) * 1.5f, effects, 0f);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, offsetY), new Rectangle?(NPC.frame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, effects, 0f);
			return false;
		}

		public override void AI()
		{
            switch (State)
            {
                case (float)States.Appearance:
                    Appearance();
                    break;
                case (float)States.HeavyJump:
                    HeavyJump();
                    break;
                case (float)States.Stand:
                    Stagnant();
                    break;
                case (float)States.DoJump:
                    MakeJump();
                    break;
                case (float)States.DoExtraJump:
                    MakeExtraJump();
                    break;
                case (float)States.Jumping:
                    Jump();
                    break;
                case (float)States.Fall:
                    Falling();
                    break;
            }
        }

		public override void FindFrame(int frameHeight)
		{
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            int toPlayer = player.Center.X < center.X ? -1 : 1;
            int currentFrame;
            switch (State)
            {
                case (float)States.Stand:
                    bool flag = AdvancedJumped2;
                    if (Math.Abs(NPC.velocity.X) < 0.5f || flag)
                    {
                        NPC.direction = toPlayer;
                    }
                    double rate = 0.125;
                    if ((NPC.frameCounter += rate) > (double)(STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT)) 
                    {
                        NPC.frameCounter = 0.0;
                    }
                    currentFrame = (int)Frame.Stand1 + (int)(NPC.frameCounter % (double)STANDING_FRAMES_COUNT);
                    NPC.frame.Y = (flag ? (int)Frame.Spawn : currentFrame) * frameHeight;
                    break;
                case (float)States.Appearance:
                case (float)States.HeavyJump:
                    NPC.direction = 1;
                    NPC.frame.Y = (int)Frame.Jump2 * frameHeight;
                    break;
                case (float)States.DoJump:
                    NPC.frame.Y = (int)Frame.Stand3 * frameHeight;
                    break;
                case (float)States.DoExtraJump:
                    rate = 0.125;
                    if ((NPC.frameCounter += rate) > (double)(STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT))
                    {
                        NPC.frameCounter = 0.0;
                    }
                    currentFrame = (int)Frame.Stand1 + (int)(NPC.frameCounter % (double)STANDING_FRAMES_COUNT);
                    NPC.direction = toPlayer;
                    NPC.frame.Y = (NPC.velocity.Y < 0f ? (int)Frame.Jump1 : NPC.velocity.Y == 0f ? currentFrame : (int)Frame.Jump2) * frameHeight;
                    break;
                case (float)States.Jumping:
                    break;
                case (float)States.Fall:
                    NPC.direction = AdvancedJumpCount >= MAX_JUMP_COUNT ? toPlayer : NPC.velocity.X < 0f ? -1 : 1;
                    NPC.frame.Y = (NPC.velocity.Y < 0f ? (int)Frame.Jump1 : (int)Frame.Jump2) * frameHeight;
                    break;
            }
        }

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (State != (int)States.DoJump)
			{
                return;
			}
            if (TooFar())
			{
                ChangeState((int)States.Jumping);
			}
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                for (int i = 1; i <= 7; i++)
                {
                    int gore = ModContent.Find<ModGore>(string.Concat("Consolaria/LPG", i.ToString())).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), gore);
                }
            }
        }

        private void Appearance()
		{
            NPC.velocity.X = NPC.velocity.Y = 0f;
            if (NPC.target < 0 || NPC.target == 255)
			{
                NPC.TargetClosest();
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            if (StateTimer != 1f)
            {
                float offsetY = 350f;
                NPC.Center = Main.player[NPC.target].Center - new Vector2(0f, offsetY);
            }
            ChangeState((int)States.HeavyJump);
            NPC.netUpdate = true;
        }

        private void HeavyJump()
		{
            NPC.rotation = NPC.velocity.Y / 10f;
            if (NPC.Opacity != 1f)
            {
                NPC.Opacity += 0.01f;
                NPC.Opacity *= 1.1f;
            }
            if (!NPC.collideY)
            {
                float velocityYSpeed = 10f;
                float acceleration = velocityYSpeed / 5f;
                NPC.velocity.Y += velocityYSpeed;
                NPC.velocity.Y *= acceleration;
                NPC.velocity.Y = Math.Min(NPC.velocity.Y, 20f);
            }
            if (NPC.velocity.Length() != 0f)
			{
                return;
			}
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.Bottom - new Vector2(NPC.width / 2, 30f), NPC.width, 30, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, Color.GhostWhite, Main.rand.NextFloat() * 1.5f + Main.rand.NextFloat() * 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity.Y = -3.5f + Main.rand.NextFloat() * -3f;
                    Main.dust[dust].velocity.X *= 7f;
                }
            }
            ChangeState((int)States.Stand);
            SpawnStomp();
            NPC.netUpdate = true;
        }

        private void Stagnant()
		{
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            float slow = Main.expertMode ? 0.895f : 0.925f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX)
            {
                NPC.velocity.X = 0f;
            }
            bool hasTargetAndClose = NPC.HasValidTarget;
            if (hasTargetAndClose)
            {
                if (player.Distance(center) >= MAX_DISTANCE)
				{
                    return;
                }
                bool zeroVelocity = NPC.velocity.Y != 0f | NPC.velocity.X != 0f;
                if (zeroVelocity)
                {
                    return;
                }
                bool expertMode = !Main.expertMode;
                int attackTime = (int)MathHelper.Lerp(!expertMode ? 30f : 15f, !expertMode ? 100f : 85f, (float)NPC.life / (float)NPC.lifeMax);
                if (++StateTimer >= attackTime)
                {
                    if (AdvancedJumpCount >= MAX_JUMP_COUNT)
					{
                        if (Main.rand.NextBool() && Main.expertMode)
						{
                            JumpCount = 0;
                            ChangeState((int)States.DoExtraJump);
                            NPC.netUpdate = true;
                            return;
                        }
					}
                    else
                    {
                        JumpCount++;
                    }
                    ChangeState((int)States.DoJump);
                    NPC.netUpdate = true;
                }
                else if (StateTimer >= attackTime / 3)
				{
                    AdvancedJumped = false;
                    AdvancedJumped2 = false;
                }
            }
            else
			{
                NPC.TargetClosest(true);
            }
        }

        private void MakeJump()
        {
            NPC.rotation = NPC.velocity.Y / 25f;
            bool flag = AdvancedJumpCount >= MAX_JUMP_COUNT;
            if (TooFar() && !JustSpawned && !flag)
            {
                float slow = 0.35f;
                int jumpStrength;
                NPC.velocity.Y *= slow;
                NPC.velocity.X *= slow;
                bool expertMode = !Main.expertMode;
                int attackTime = (int)MathHelper.Lerp(!expertMode ? 45f : 30f, !expertMode ? 75f : 60f, (float)NPC.life / (float)NPC.lifeMax);
                if (++StateTimer > attackTime)
				{
                    jumpStrength = 15;
                    ChangeState((int)States.Jumping, jumpStrength);
                    NPC.netUpdate = true;
                    return;
                }
            }
            else
            {
                ChangeState((int)States.Jumping);
                NPC.netUpdate = true;
            }
        }

        private void MakeExtraJump()
		{
            NPC.rotation = NPC.velocity.Y / 25f;
            NPC.noTileCollide = false;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            Player player = Main.player[NPC.target];
            if ((NPC.Center.X > player.Center.X ? (NPC.Center.X - player.Center.X) : (player.Center.X - NPC.Center.X)) < 10 && NPC.Center.Y < player.Center.Y - 150 && JumpCount >= 3)
            {
                ChangeState((int)States.Appearance, 1f);
                AdvancedJumpCount = 0;
                JumpCount = 0;
                JustSpawned = true;
                AdvancedJumped = false;
                AdvancedJumped2 = true;
                SpawnBigEgg();
                return;
            }
            if (NPC.velocity.Y != 0f)
            {
                return;
            }
            float slow = 0.85f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX)
            {
                NPC.velocity.X = 0f;
            }
            int rate = (int)MathHelper.Lerp(Main.rand.Next(5, 10), Main.rand.Next(1, 6), (float)NPC.life / (float)NPC.lifeMax);
            if (++StateTimer % rate == 0)
            {
                if (JumpCount >= 4)
                {
                    JumpCount = 0;
                }
                int jumpStrength = (int)JumpCount + 2;
                StateTimer = jumpStrength;
                ExtraJump();
                NPC.netUpdate = true;
                return;
            }
            if (StateTimer <= 0)
            {
                NPC.velocity.Y += 6f;
            }
        }

        private void ExtraJump()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            float slow = 0.85f;
            NPC.velocity.X *= slow;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX)
            {
                NPC.velocity.X = 0f;
            }
            if (NPC.velocity.Y != 0f || NPC.velocity.X != 0f)
			{
                return;
			}
            NPC.noTileCollide = true;
            SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
            while (StateTimer > 0)
            {
                NPC.velocity.Y -= Main.rand.NextFloat(2f, 5f) * Main.rand.NextFloat(1.1f, 1.75f) * 0.5f;
                NPC.velocity.X += Main.rand.NextFloat(2f, 5f) * 0.75f * NPC.direction;
                StateTimer--;
                NPC.netUpdate = true;
            }
            JumpCount++;
        }

        private void Jump()
		{
            NPC.rotation = NPC.velocity.Y / 25f;
            JustSpawned = false;
            NPC.noTileCollide = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
			{
                return;
			}
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            Vector2 playerCenter = player.Center;
            float offsetY = 220f;
            float rotation = (float)Math.Atan2(center.Y - (playerCenter.Y - offsetY), center.X - playerCenter.X);
            NPC.velocity.X = (float)(7 * NPC.direction);
            NPC.velocity.Y = -(float)(Math.Sin(rotation) * 14.0);
            if (--StateTimer > 0)
            {
                if (!AdvancedJumped)
                {
                    AdvancedJumped = true;
                }
                if (!AdvancedJumped2)
                {
                    AdvancedJumped2 = true;
                }
                NPC.velocity.X += 4.25f * (float)NPC.direction;
                NPC.velocity.Y -= 2f;
                NPC.velocity *= 1.075f;
                return;
            }
            SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
            if (AdvancedJumpCount >= MAX_ADVANCED_JUMP_COUNT + 1)
            {
                AdvancedJumpCount = 0;
                SpawnBigEgg();
            }
            ChangeState((int)States.Fall, (int)NPC.velocity.X);
            NPC.netUpdate = true;
        }

        private void Falling()
		{
            NPC.rotation = NPC.velocity.Y / 25f;
            if (--StateTimer > 0)
			{
                return;
			}
            NPC.noTileCollide = false;
            if (NPC.velocity.Y != 0f)
			{
                return;
			}
            if (AdvancedJumped)
            {
                SpawnEggs();
            }
            bool flag = false;
            bool flag2 = AdvancedJumpCount >= MAX_JUMP_COUNT;
            if (JumpCount >= MAX_JUMP_COUNT)
            {
                JumpCount = 0;
                if (!flag2)
                {
                    AdvancedJumpCount++;
                }
                flag = true;
            }
            if (flag2)
			{
                AdvancedJumpCount++;
            }
            bool flag3 = AdvancedJumpCount >= MAX_JUMP_COUNT + 2;
            if (flag3)
			{
                flag = true;
            }
            ChangeState(flag ? (int)States.Jumping : (int)States.Stand);
            if (flag)
			{
                SpawnStomp();
            }
            NPC.netUpdate = true;
        }

        private void SpawnStomp()
		{
            if (NPC.oldVelocity.Y < 1f)
			{
                return;
			}
            SoundEngine.PlaySound(SoundID.DD2_OgreGroundPound, NPC.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                bool expertMode = Main.expertMode;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Stomp>(), expertMode ? NPC.damage / 3 : 0, 2f, Main.myPlayer, 0f, (expertMode ? Main.rand.NextFloat(75f, 90f) : Main.rand.NextFloat(40f, 65f)) / 3f);
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
            }
        }

        private void SpawnEggs()
		{
            if (AdvancedJumped && !Main.expertMode)
            {
                AdvancedJumped = false;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            for (int i = -2; i < 1; i++)
            {
                Vector2 velocity = new Vector2(13f, 0f);
                velocity = velocity.RotatedBy((double)((float)(float)-i * MathHelper.TwoPi / 10f), Vector2.Zero);
                int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SmallEgg>());
                int toPlayer = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                Main.npc[index].velocity.X = velocity.X * toPlayer;
                Main.npc[index].velocity.Y = -velocity.Y / 2f;
                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }
        }

        private void SpawnBigEgg()
        {
            if (!AdvancedJumped2)
			{
                AdvancedJumped2 = true;
            }
            if (!AdvancedJumped && Main.expertMode)
            {
                AdvancedJumped = true;
            }
            int type = ModContent.NPCType<BigEgg>();
            if (Main.netMode == NetmodeID.MultiplayerClient || NPC.CountNPCS(type) > 1)
            {
                return;
            }
            int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, type);
            if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: index);
            }
        }

        private bool TooFar()
		{
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            return (Main.expertMode && !Collision.CanHitLine(center, NPC.width, NPC.height, player.Center, 2, 2)) || player.Distance(center) > MAX_DISTANCE / (Main.expertMode ? 6f : 5f);
        }
	}
}