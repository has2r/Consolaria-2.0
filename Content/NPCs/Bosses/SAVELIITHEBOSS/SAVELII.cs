using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace Consolaria.Content.NPCs.Bosses.SAVELIITHEBOSS
{
	internal class SAVELII : ConsolariaModBoss
	{
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            NPCID.Sets.TrailingMode[Type] = 1;

            DisplayName.SetDefault("SAVELII THE BOSS");

            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/SAVELII_Bestiary",
                PortraitScale = 2.5f,
				PortraitPositionXOverride = 30f,
				PortraitPositionYOverride = 10f,
				Scale = 0.8f,
				Position = new Vector2(18f, 40f)
			};
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override bool CheckDead()
		{
			return true;
		}

		public override void SetDefaults()
        {
            base.SetDefaults();

            NPC.width = 158;
            NPC.height = 134;

            short lifeMax = 1;
            NPC.lifeMax = lifeMax;
            short damage = 1;
            NPC.damage = damage;
            short defense = 1;
            NPC.defense = defense;

            NPC.value = (float)Item.buyPrice(platinum: 999);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.SpawnWithHigherTime(30);
            NPC.timeLeft = NPC.activeTime * 30;
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	=> bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
	{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("savelii elerium creator")
	});

		public override void AI()
		{
			float maxSpeedX = 5.25f;
			float maxSpeedY = 2.1f;
			float moveIntervalX = 0.35f;
			float moveIntervalY = 0.175f;
			Vector2 velocity = NPC.velocity;
			if (NPC.direction == 1 && velocity.X > -maxSpeedX)
			{
				velocity.X -= moveIntervalX;
				if (velocity.X > maxSpeedX)
				{
					velocity.X -= moveIntervalX;
				}
				else if (velocity.X > 0f)
				{
					velocity.X += moveIntervalX * 0.5f;
				}
				if (velocity.X < -maxSpeedX)
				{
					velocity.X = -maxSpeedX;
				}
			}
			else if (NPC.direction == -1 && velocity.X < maxSpeedX)
			{
				velocity.X += moveIntervalX;
				if (velocity.X < -maxSpeedX)
				{
					velocity.X += moveIntervalX;
				}
				else if (velocity.X < 0f)
				{
					velocity.X -= moveIntervalX * 0.5f;
				}
				if (velocity.X > maxSpeedX)
				{
					velocity.X = maxSpeedX;
				}
			}
			if (NPC.directionY == 1 && velocity.Y > -maxSpeedY)
			{
				velocity.Y -= moveIntervalY;
				if (velocity.Y > maxSpeedY)
				{
					velocity.Y -= moveIntervalY;
				}
				else if (velocity.Y > 0f)
				{
					velocity.Y += moveIntervalY * 0.5f;
				}
				if (velocity.Y < -maxSpeedY)
				{
					velocity.Y = -maxSpeedY;
				}
			}
			else if (NPC.directionY == -1 && velocity.Y < maxSpeedY)
			{
				velocity.Y += moveIntervalY;
				if (velocity.Y < -maxSpeedY)
				{
					velocity.Y += moveIntervalY;
				}
				else if (velocity.Y < 0f)
				{
					velocity.Y -= moveIntervalY * 0.5f;
				}
				if (velocity.Y > maxSpeedY)
				{
					velocity.Y = maxSpeedY;
				}
			}
			NPC.velocity = -velocity;
			NPC.TargetClosest();
			int toPlayer = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
			NPC.direction = toPlayer;
			if (Main.rand.NextBool(200))
			{
				Vector2 distance = Main.player[NPC.target].Center - NPC.Center;
				SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
				float dashStrength = 10f;
				float speed = dashStrength * 3f;
				float x = Main.player[NPC.target].Center.X - NPC.Center.X;
				float y = Main.player[NPC.target].Center.Y - NPC.Center.Y;
				float acceleration = Math.Abs(Main.player[NPC.target].velocity.X) + Math.Abs(Main.player[NPC.target].velocity.Y) / 4f;
				acceleration += 10f - acceleration;
				x -= Main.player[NPC.target].velocity.X * acceleration;
				y -= Main.player[NPC.target].velocity.Y * acceleration / 4f;
				float sqrt = (float)Math.Sqrt(x * x + y * y);
				sqrt = speed / sqrt;
				velocity.X = x * sqrt;
				velocity.Y = y * sqrt;
			}
		}
	}
}
