using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Consolaria.Content.Projectiles.Enemies;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.DataStructures;
using Consolaria.Common;
using Terraria.GameContent.ItemDropRules;
using Consolaria.Content.Items.BossDrops.Turkor;
using Consolaria.Content.Items.Weapons.Magic;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Summon;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Consumables;

namespace Consolaria.Content.NPCs.Turkor
{
	public class TurkortheUngrateful : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Turkor the Ungrateful");
			Main.npcFrameCount[Type] = 3;

			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.Confused 
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/Turkor_Bestiary",
				PortraitScale = 0.6f, 
				Position = new Vector2(0, 10f),
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			int width = 200; int height = 100;
			NPC.Size = new Vector2(width, height);

			NPC.lifeMax = 7000;
			NPC.damage = 40;

			NPC.defense = 0;
			NPC.knockBackResist = 0f;

			NPC.aiStyle = -1;
			NPC.dontTakeDamage = false;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;

			NPC.boss = true;
			NPC.npcSlots = 10f;

			NPC.lavaImmune = true;
			NPC.noTileCollide = false;
			NPC.noGravity = false;

			NPC.SpawnWithHigherTime(30);

			if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Turkor");
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			NPC.lifeMax = 9500 + (int) (numPlayers > 1 ? NPC.lifeMax * 0.2 * numPlayers : 0);
			NPC.damage = (int) (NPC.damage * 0.65f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Empowered by unknown thanksgiving spirits, this mutant turkey poses a great danger to those who dare to cut a piece or two from its crunchy body.")
			});
		}

		public override void FindFrame(int frameHeight) {
			if (!NPC.AnyNPCs(ModContent.NPCType<TurkortheUngratefulHead>())) {
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= Main.npcFrameCount[NPC.type];
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
		}

		private float posBX = 0f;
		private float posBY = 0f;
		private float h = 0f;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
			h += 0.1f;
			posBX += (float)Math.Cos(h) * 4;
			posBY += (float)Math.Sin(h) * 4;

			if (enraged) {
				for (int i = 0; i < 1; i++) {
					Color color2 = drawColor;
					color2 = NPC.GetAlpha(color2) * colo;
					Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X + 20 + posBX - Main.screenPosition.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - 40 + posBY - Main.screenPosition.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
					Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X - 20 - posBX - Main.screenPosition.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y + 40 - posBY - Main.screenPosition.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * (float)i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}

		private void HalfCircle() {
			SoundEngine.PlaySound(SoundID.Item71, NPC.position);
			ushort type = (ushort)ModContent.ProjectileType<TurkorKnife>();
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 8, 0, type, (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 6, -4, type, (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -6, type, (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -8, 0, type, (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -6, -4, type, (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
			for (int num623 = (int)NPC.position.X - 20; num623 < (int)NPC.position.X + NPC.width + 40; num623 += 20) {
				for (int num624 = 0; num624 < 4; num624 = num + 1) {
					int dust = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, 31, 0f, 0f, 100, default(Color), 1.5f);
					Dust dust3 = Main.dust[dust];
					dust3.velocity *= 0.2f;
					num = num624;
				}
				int num626 = Gore.NewGore(NPC.GetSource_FromAI(), new Vector2((num623 - 20), NPC.position.Y + NPC.height - 8f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Gore gore = Main.gore[num626];
				gore.velocity *= 0.4f;
			}
			SoundEngine.PlaySound(SoundID.Item88, NPC.position);
		}

		private float colo = 0f;

		//head related
		private bool headSpawned {
			get => NPC.ai[0] == 1f;
			set => NPC.ai[0] = value ? 1f : 0f;
		}
		public ref float headNumber => ref NPC.ai[2];
		public ref float turkorHead_ => ref NPC.ai[3];

		//idling phase stuff
		private int timer = 0;
		private int timer2 = 0;
		//bool onAir = false;
		//bool Despawning = false;
		private bool ground_ = false;
		private int num = 0;
		private bool teleport = false;

		//jumptimer 
		private int jumptimer = 1200;

		//player old position stuff
		private bool findplayer = false;
		private float posX = 0f;
		private float posY = 0f;

		//enraged mode
		private bool enraged = false;

		public override void AI() {
			Player player = Main.player[NPC.target];

			ushort turkorHead = (ushort)ModContent.NPCType<TurkortheUngratefulHead>();
			int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y - 20), NPC.width, NPC.height, 31, 0f, -6f, 60, Color.White, 1f);
			Main.dust[dust].velocity *= 0.2f;

			NPC.TargetClosest(true);
			if (player.dead) {
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.active = false;
			}

			//if player too far then becomes enraged
			Vector2 vector101 = new Vector2(NPC.Center.X, NPC.Center.Y);
			float num855 = Main.player[NPC.target].Center.X - vector101.X;
			float num856 = Main.player[NPC.target].Center.Y - vector101.Y;
			float num857 = (float)Math.Sqrt((double)(num855 * num855 + num856 * num856));
			if (num857 > 600f) {
				NPC.localAI[2] = 40;
				if (colo < .5f) colo += 0.05f; 
				enraged = true;
			}
			else {

				if (colo > 0f) colo -= 0.05f;
				else {
					colo = 0;
					NPC.localAI[2] = 0;
					enraged = false;
				}
			}

			//check if stuck underground
			if (!ground_ && timer2 < 1200) {
				if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
					NPC.noTileCollide = true;
					NPC.velocity.Y = -6;
				}
				else {
					if (NPC.localAI[2] != 40) { timer = 0; }
					NPC.noTileCollide = false;
					ground_ = true;
				}
			}
			
			NPC.dontTakeDamage = NPC.AnyNPCs(turkorHead) || NPC.localAI[1] == 40 || enraged;

			if (!headSpawned) {
				headSpawned = true;
				for (int i = 0; i < (int)headNumber; ++i) {
					turkorHead_ = (float)NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, turkorHead);
					NPC npc = Main.npc[(int)turkorHead_];
					npc.velocity.X = Main.rand.Next(-6, 7);
					npc.velocity.Y = Main.rand.Next(-6, 7);
					npc.ai[1] = (float)NPC.whoAmI;
					npc.netUpdate = true;
				}
			}

			//shoot projectiles at player 
			if (!NPC.AnyNPCs(turkorHead) || enraged) {
				timer++;
				if (timer >= 140 && NPC.localAI[1] != 40) {
					if (!findplayer && timer < 160) {
						posX = Main.player[NPC.target].position.X;
						posY = Main.player[NPC.target].position.Y;
						Vector2 Velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 14;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Velocity.X, Velocity.Y, ModContent.ProjectileType<Pointer>(), 0, 1, Main.myPlayer, 0, 0);
						findplayer = true;
					}
					if (findplayer && timer >= 160) {
						Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
						float rotation0 = (float)Math.Atan2((vector8.Y) - (posY + (Main.player[NPC.target].height * 0.5f)), (vector8.X) - (posX + (Main.player[NPC.target].width * 0.5f)));
						if (timer % 5 == 0) {
							SoundEngine.PlaySound(SoundID.Item42, NPC.position);
							int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<TurkorFeather>(), (int)(NPC.damage / 2), 1, Main.myPlayer, 0, 0);
							Main.projectile[a].aiStyle = -1;
							Main.projectile[a].velocity.X = (float)(Math.Cos(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
							Main.projectile[a].velocity.Y = (float)(Math.Sin(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
						}
						if (timer >= 180) {
							posX = 0;
							posY = 0;
							timer = 0;
							findplayer = false;
						}
					}
				}
			}

			//idling phase
			if (!NPC.AnyNPCs(turkorHead)) {
				timer2++;

				if (timer2 >= jumptimer - 100) {
					NPC.localAI[1] = 40;
					NPC.rotation = Vector2.UnitY.RotatedBy((double)(timer / 25f * 6.3f), default(Vector2)).Y * 0.2f;
				}

				//preventing melee cheese
				if (NPC.localAI[1] != 40 && timer2 < jumptimer && (NPC.life <= (int)(NPC.lifeMax * 0.75f) && headNumber < 2 || NPC.life <= (int)(NPC.lifeMax * 0.4f) && headNumber < 3)) {
					timer2 = jumptimer - 100;
					NPC.localAI[1] = 40;
				}

				if (timer2 >= jumptimer) {
					NPC.rotation += 0.2f;
					NPC.noGravity = true;
					if (timer2 <= jumptimer) {
						HalfCircle();
						NPC.noTileCollide = true;
						SoundEngine.PlaySound(SoundID.Roar, NPC.position);
						NPC.velocity.Y = -32;
					}
					if (timer2 >= (jumptimer + 20) && !teleport) {
						if (NPC.alpha <= 255) NPC.alpha += 5;					
						else {
							NPC.velocity.Y = 0;
							teleport = true;
							timer2 = jumptimer + 60;
						}
					}
					if (teleport && timer2 >= (jumptimer + 60)) {
						if (timer2 <= (jumptimer + 60)) {
							NPC.position.X = Main.player[NPC.target].Center.X - 40;
							NPC.position.Y = Main.player[NPC.target].Center.Y - 800;
						}
						NPC.alpha -= 8;

						NPC.velocity.Y = 26;
						if (Main.player[NPC.target].position.Y <= NPC.position.Y) {
							if (Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].HasTile || Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
								//reset everything
								NPC.noTileCollide = false;
								timer2 = 0;

								NPC.alpha = 0;
								NPC.rotation = 0;
								ground_ = false;
								if (headNumber < 3) headNumber += 1;								
								headSpawned = false;
								NPC.noGravity = false;
								NPC.localAI[1] = 0;
								HalfCircle();
								teleport = false;
								posX = 0;
								posY = 0;
								timer = 0;
								findplayer = false;
								NPC.velocity = Vector2.Zero;
								NPC.netUpdate = true;
							}
						}
					}
				}
			}
		}

        public override void HitEffect(int hitDirection, double damage) {
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorMeatGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorMeatGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
			}
		}

		public override void OnKill()
			=> NPC.SetEventFlagCleared(ref DownedBossSystem.downedTurkor, -1);

		public override void BossLoot(ref string name, ref int potionType)
			=> potionType = ItemID.HealingPotion;

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			Conditions.NotExpert notExpert = new Conditions.NotExpert();
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TurkorBag>()));
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TurkorRelic>()));
			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<FruitfulPlate>(), 4));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(new OneFromOptionsDropRule(1, 1, ModContent.ItemType<FeatherStorm>(), ModContent.ItemType<GreatDrumstick>(), ModContent.ItemType<TurkeyStuff>()));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SpicySauce>(), 2, 15, 34));
			npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<TurkorMask>(), 7));

			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TurkorTrophy>(), 10));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wishbone>(), 5));
		}
	}
}
