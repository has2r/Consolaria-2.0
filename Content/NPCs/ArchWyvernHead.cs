using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Consolaria.Content.Items.Vanity;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Consolaria.Content.Projectiles.Enemies;

namespace Consolaria.Content.NPCs
{
	public class ArchWyvernHead : ModNPC
	{
		private int shootTimer;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Arch Wyvern");

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.OnFire,
					BuffID.OnFire3,
					BuffID.ShadowFlame,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/ArchWyvern_Bestiary",
				Position = new Vector2(20f, 14f),
                PortraitPositionXOverride = 30f,
                PortraitPositionYOverride = -6f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			int width = 32; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.aiStyle = NPCAIStyleID.Worm;

			NPC.damage = 90;
			NPC.defense = 20;
			NPC.lifeMax = 8000;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath8;

			NPC.knockBackResist = 0.0f;
			NPC.npcSlots = 5f;

			NPC.netAlways = true;

			NPC.alpha = 255;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ArchWyvernBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("Looks like a Digger fell into some aqua-colored paint. Oh well.")
			});
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
			scale = 1.1f;
			return new bool?();
		}

		public override void AI() {
			Player player = Main.player[NPC.target];

			if (NPC.target < 0 || NPC.target == 250 || player.dead) NPC.TargetClosest(true);	
			if (player.dead && NPC.timeLeft > 300) NPC.timeLeft = 300;	

			shootTimer++;
			if (shootTimer >= 50 && shootTimer % 5 == 0) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 5, NPC.Center.Y, NPC.velocity.X * 2f, NPC.velocity.Y * 2f, ModContent.ProjectileType<ArchFlames>(), NPC.damage / 2, 4f, 255, 0, 0);
				SoundEngine.PlaySound(SoundID.Item20, NPC.position);
			}
			if (shootTimer >= 70) shootTimer = 0;

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (NPC.ai[0] == 0f) {
					NPC.ai[2] = NPC.whoAmI;
					NPC.realLife = NPC.whoAmI;
					int num96 = NPC.whoAmI;
					for (int num97 = 0; num97 < 20; num97++) {
						int num98 = ModContent.NPCType<ArchWyvernBody1>();
						if (num97 == 4 || num97 == 16) num98 = ModContent.NPCType<ArchWyvernLegs>();
						else {
							if (num97 == 17) num98 = ModContent.NPCType<ArchWyvernBody2>();
							else
							{
								if (num97 == 18) num98 = ModContent.NPCType<ArchWyvernBody3>();
								else {
									if (num97 == 19) num98 = ModContent.NPCType<ArchWyvernTail>();
								}
							}
						}
						int num99 = NPC.NewNPC(NPC.GetSource_FromAI(), ((int)NPC.position.X + (NPC.width / 2)), (int)(NPC.position.Y + NPC.height), num98, NPC.whoAmI);
						Main.npc[num99].ai[2] = NPC.whoAmI;
						Main.npc[num99].realLife = NPC.whoAmI;
						Main.npc[num99].ai[1] = num96;
						Main.npc[num96].ai[0] = num99;
						NetMessage.SendData(23, -1, -1, null, num99);
						num96 = num99;
					}
				}
			}

			int num107 = (int)(NPC.position.X / 16f) - 1;
			int num108 = (int)((NPC.position.X + NPC.width) / 16f) + 2;
			int num109 = (int)(NPC.position.Y / 16f) - 1;
			int num110 = (int)((NPC.position.Y + NPC.height) / 16f) + 2;

			if (num107 < 0) num107 = 0;	
			if (num108 > Main.maxTilesX) num108 = Main.maxTilesX;
			if (num109 < 0) num109 = 0;	
			if (num110 > Main.maxTilesY) num110 = Main.maxTilesY;
			if (NPC.velocity.X < 0f) NPC.spriteDirection = 1;	
			if (NPC.velocity.X > 0f) NPC.spriteDirection = -1;		

			float num115 = 16f;
			float num116 = 0.4f;

			Vector2 vector14 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			float num118 = Main.rand.Next(-500, 501) + player.position.X + (player.width / 2);
			float num119 = Main.rand.Next(-500, 501) + player.position.Y + (player.height / 2);
			num118 = ((int)(num118 / 16f) * 16);
			num119 = ((int)(num119 / 16f) * 16);
			vector14.X = ((int)(vector14.X / 16f) * 16);
			vector14.Y = ((int)(vector14.Y / 16f) * 16);
			num118 -= vector14.X;
			num119 -= vector14.Y;
			float num120 = (float)Math.Sqrt((num118 * num118 + num119 * num119));

			float num123 = Math.Abs(num118);
			float num124 = Math.Abs(num119);
			float num125 = num115 / num120;
			num118 *= num125;
			num119 *= num125;

			bool flag14 = false;
			if (((NPC.velocity.X > 0f && num118 < 0f) || (NPC.velocity.X < 0f && num118 > 0f) || (NPC.velocity.Y > 0f && num119 < 0f) || (NPC.velocity.Y < 0f && num119 > 0f)) && Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) > num116 / 2f && num120 < 300f) {
				flag14 = true;
				if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num115) NPC.velocity *= 1.1f;			
			}
			if (NPC.position.Y > player.position.Y || (player.position.Y / 16f) > Main.worldSurface || player.dead) {
				flag14 = true;
				if (Math.Abs(NPC.velocity.X) < num115 / 2f) {
					if (NPC.velocity.X == 0f) NPC.velocity.X = NPC.velocity.X - NPC.direction;		
					NPC.velocity.X = NPC.velocity.X * 1.1f;
				}
				else 
				{
					if (NPC.velocity.Y > -num115) NPC.velocity.Y = NPC.velocity.Y - num116;		
				}
			}
			if (!flag14) {
				if ((NPC.velocity.X > 0f && num118 > 0f) || (NPC.velocity.X < 0f && num118 < 0f) || (NPC.velocity.Y > 0f && num119 > 0f) || (NPC.velocity.Y < 0f && num119 < 0f)) {
					if (NPC.velocity.X < num118) NPC.velocity.X = NPC.velocity.X + num116;	
					else 
					{
						if (NPC.velocity.X > num118) NPC.velocity.X = NPC.velocity.X - num116;			
					}
					if (NPC.velocity.Y < num119) NPC.velocity.Y = NPC.velocity.Y + num116;	
					else 
					{
						if (NPC.velocity.Y > num119) NPC.velocity.Y = NPC.velocity.Y - num116;			
					}
					if (Math.Abs(num119) < num115 * 0.2 && ((NPC.velocity.X > 0f && num118 < 0f) || (NPC.velocity.X < 0f && num118 > 0f))) {
						if (NPC.velocity.Y > 0f) NPC.velocity.Y = NPC.velocity.Y + num116 * 2f;	
						else NPC.velocity.Y = NPC.velocity.Y - num116 * 2f;	
					}
					if (Math.Abs(num118) < num115 * 0.2 && ((NPC.velocity.Y > 0f && num119 < 0f) || (NPC.velocity.Y < 0f && num119 > 0f))) {
						if (NPC.velocity.X > 0f) NPC.velocity.X = NPC.velocity.X + num116 * 2f;	
						else NPC.velocity.X = NPC.velocity.X - num116 * 2f;			
					}
				}
				else 
				{
					if (num123 > num124) {
						if (NPC.velocity.X < num118) NPC.velocity.X = NPC.velocity.X + num116 * 1.1f;	
						else {
							if (NPC.velocity.X > num118) NPC.velocity.X = NPC.velocity.X - num116 * 1.1f;					
						}
						if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num115 * 0.5) {
							if (NPC.velocity.Y > 0f) NPC.velocity.Y = NPC.velocity.Y + num116;				
							else NPC.velocity.Y = NPC.velocity.Y - num116;			
						}
					}
					else 
					{
						if (NPC.velocity.Y < num119) NPC.velocity.Y = NPC.velocity.Y + num116 * 1.1f;		
						else 
						{
							if (NPC.velocity.Y > num119) NPC.velocity.Y = NPC.velocity.Y - num116 * 1.1f;			
						}
						if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num115 * 0.5) {
							if (NPC.velocity.X > 0f) NPC.velocity.X = NPC.velocity.X + num116;	
							else NPC.velocity.X = NPC.velocity.X - num116;	
						}
					}
				}
			}
			NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == 1) effects = SpriteEffects.FlipHorizontally;	
			spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale + 4f + origin.Y * NPC.scale + 56f), new Rectangle?(NPC.frame), drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
			return false;
		}

		public override void HitEffect(int hitDirection, double damage) {
			if (NPC.life <= 0) {
				for (int i = 0; i < 4; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);	
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArchWyvernMask>(), 15));
			npcLoot.Add(ItemDropRule.Common(ItemID.SoulofFlight, 1, 5, 20));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> Main.hardMode ? SpawnCondition.Sky.Chance * 0.025f : 0;
	}
}