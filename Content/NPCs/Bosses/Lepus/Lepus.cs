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
            Stand,
            DoJump,
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

        private const short HITBOX_SIZE_X = 75;
        private const short HITBOX_SIZE_Y = 50;
        private const short FRAME_WIDTH = 90;
        private const short FRAME_HEIGHT = 76;
        private const short STANDING_FRAMES_COUNT = 4;

        private const float MAX_DISTANCE = 2000f;

        public bool AdvancedJump
        {
            get => NPC.localAI[0] == 1f;
            set => NPC.localAI[0] = value ? 1f : 0f;
        }

        public ref float JumpCount
            => ref NPC.ai[2];

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            NPCID.Sets.TrailingMode[Type] = 0;

            DisplayName.SetDefault(nameof(Lepus));

            Main.npcFrameCount[Type] = 7;

            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f
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
            NPC.defense = (int)(NPC.defense + numPlayers);
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
            target.AddBuff(BuffID.Slow, debuffTime);
        }

        public override void OnKill()
            => NPC.SetEventFlagCleared(ref DownedBossSystem.downedLepus, -1);

        public override void BossLoot(ref string name, ref int potionType)
            => potionType = ItemID.LesserHealingPotion;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LepusDropCondition lepusDropCondition = new LepusDropCondition();
            IItemDropRule conditionalRule = new LeadingConditionRule(lepusDropCondition);
            Conditions.NotExpert notExpert = new Conditions.NotExpert();
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

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            SpriteEffects effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(FrameWidth, FrameHeight) / 2f;
            bool didAdvancedJump = AdvancedJump && Math.Abs(NPC.velocity.X) > 0.5f;
            if (didAdvancedJump)
            {
                float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f;
                float alpha = Math.Max(0.1f, (Math.Abs(NPC.velocity.X) > 5f ? 5f : Math.Abs(NPC.velocity.X)) / 3f);
                for (int i = 1; i < NPC.oldPos.Length; i += 2)
                {
                    Color color = NPC.GetAlpha(Color.HotPink);
                    color *= (float)((NPC.oldPos.Length - i) / 15f) * 0.75f;
                    color *= alpha;
                    spriteBatch.Draw(texture, new Vector2(NPC.oldPos[i].X - screenPos.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.oldPos[i].Y - screenPos.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color, NPC.rotation, origin, scale * alpha * 0.5f, effects, 0f);
                }
            }
            float offsetY = -12f;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, offsetY), new Rectangle?(NPC.frame), drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
			return false;
		}

		public override void AI()
		{
            switch (State)
            {
                case (float)States.Stand:
                    Stagnant();
                    break;
                case (float)States.DoJump:
                    MakeJump();
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
                    NPC.direction = toPlayer;
                    double rate = 0.125;
                    if ((NPC.frameCounter += rate) > (double)(STANDING_FRAMES_COUNT * STANDING_FRAMES_COUNT)) 
                    {
                        NPC.frameCounter = 0.0;
                    }
                    currentFrame = (int)Frame.Stand1 + (int)(NPC.frameCounter % (double)STANDING_FRAMES_COUNT);
                    NPC.frame.Y = currentFrame * frameHeight;
                    break;
                case (float)States.DoJump:
                    NPC.frame.Y = (int)Frame.Stand3 * frameHeight;
                    break;
                case (float)States.Jumping:
                    break;
                case (float)States.Fall:
                    NPC.direction = NPC.velocity.X < 0f ? -1 : 1;
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
                    int gore = ModContent.Find<ModGore>(String.Concat("Consolaria/LPG", i.ToString())).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), gore);
                }
            }
        }

        private void Stagnant()
		{
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            NPC.velocity.X *= 0.925f;
            bool zeroVelocityX = Math.Abs(NPC.velocity.X) < 0.1f;
            if (zeroVelocityX)
            {
                NPC.velocity.X = 0.0f;
            }
            bool hasTargetAndClose = NPC.HasValidTarget && player.Distance(center) < MAX_DISTANCE;
            if (hasTargetAndClose)
            {
                bool zeroVelocity = NPC.velocity.Y != 0f | NPC.velocity.X != 0f;
                if (zeroVelocity)
				{
                    return;
				}
                int attackTime = (int)MathHelper.Lerp(30f, 100f, (float)NPC.life / (float)NPC.lifeMax);
                if (++StateTimer >= attackTime)
				{
                    AdvancedJump = false;
                    JumpCount++;
                    ChangeState((int)States.DoJump);
				}
            }
        }

        private void MakeJump()
        {
            if (TooFar())
            {
                float slow = 0.35f;
                int jumpStrength;
                NPC.velocity.Y *= slow;
                NPC.velocity.X *= slow;
                int attackTime = (int)MathHelper.Lerp(45f, 75f, (float)NPC.life / (float)NPC.lifeMax);
                if (++StateTimer > attackTime)
				{
                    jumpStrength = 15;
                    ChangeState((int)States.Jumping, jumpStrength);
                    return;
                }
            }
            else
            {
                ChangeState((int)States.Jumping);
            }
        }

        private void Jump()
		{
            NPC.noTileCollide = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
			{
                return;
			}
            Player player = Main.player[NPC.target];
            Vector2 center = NPC.Center;
            Vector2 playerCenter = player.Center;
            SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position);
            float offsetY = 220f;
            float rotation = (float)Math.Atan2(center.Y - (playerCenter.Y - offsetY), center.X - playerCenter.X);
            NPC.velocity.X = (float)(7 * NPC.direction);
            NPC.velocity.Y = -(float)(Math.Sin(rotation) * 14.0);
            NPC.netUpdate = true;
            if (--StateTimer > 0)
            {
                if (!AdvancedJump)
                {
                    AdvancedJump = true;
                }
                NPC.velocity.X += 5f * (float)NPC.direction;
                NPC.velocity.Y -= 2.5f;
                NPC.velocity *= 1.1f;
                return;
            }
            ChangeState((int)States.Fall, (int)NPC.velocity.X);
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
            if (AdvancedJump)
            {
                SpawnEggs();
            }
            bool flag = false;
            if (JumpCount >= 3)
            {
                JumpCount = 0;
                flag = true;
            }
            ChangeState(flag ? (int)States.Jumping : (int)States.Stand);
        }

        private void SpawnEggs()
		{
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

        private bool TooFar()
		{
            Player player = Main.player[NPC.target];
            return player.Center.Y > NPC.position.Y + NPC.height || player.Distance(NPC.Center) > MAX_DISTANCE / 4.25f;
        }
	}
}