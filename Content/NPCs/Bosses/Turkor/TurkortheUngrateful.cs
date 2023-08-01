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
using System.IO;
using Terraria.Chat;
using Terraria.Localization;

namespace Consolaria.Content.NPCs.Bosses.Turkor {
    [AutoloadBossHead]
	public class TurkortheUngrateful : ModNPC {
		public override void SetStaticDefaults () {
			Main.npcFrameCount [Type] = 3;

			NPCID.Sets.MPAllowedEnemies [Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int [] {
					BuffID.Poisoned,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/Turkor_Bestiary",
				PortraitScale = 1f,
				Position = new Vector2(30, 18f),
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults () {
			int width = 200; int height = 100;
			NPC.Size = new Vector2(width, height);

			NPC.lifeMax = 7000;
			NPC.damage = 40;

			NPC.defense = 1;
			NPC.knockBackResist = 0f;

			NPC.aiStyle = -1;
			NPC.dontTakeDamage = false;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = Item.buyPrice(gold: 5);

			NPC.boss = true;
			NPC.npcSlots = 10f;

			NPC.lavaImmune = true;
			NPC.noTileCollide = false;

			NPC.noGravity = false;
			NPC.netAlways = true;

			NPC.SpawnWithHigherTime(30);

			if (!Main.dedServ) Music = ModContent.GetInstance<ConsolariaConfig>().vanillaBossMusic ? MusicID.Boss1 : MusicLoader.GetMusicSlot(Mod, "Assets/Music/Turkor");
		}

		public override void ApplyDifficultyAndPlayerScaling (int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */ {
			NPC.lifeMax = (int) (NPC.lifeMax * 0.5f * 1.4f);
			NPC.damage = (int) (NPC.damage * 0.65f);
			if (numPlayers <= 1) return;
			float healthBoost = 0.35f;
			for (int k = 1; k < numPlayers; k++) {
				NPC.lifeMax += (int) (NPC.lifeMax * healthBoost);
				healthBoost += (1 - healthBoost) / 3;
			}
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Existing out of pure spite, this monstrocity is on a yearly vendetta against those who dare to take a bite of Thanksgiving turkey.")
			});
		}

		public override void FindFrame (int frameHeight) {
			if (!NPC.AnyNPCs(ModContent.NPCType<TurkortheUngratefulHead>())) {
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= Main.npcFrameCount [NPC.type];
				int frame = (int) NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
		}

		private float posBX = 0f;
		private float posBY = 0f;
		private float h = 0f;

		public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount [NPC.type] / 2);
			h += 0.1f;
			posBX += (float) Math.Cos(h) * 4;
			posBY += (float) Math.Sin(h) * 4;

			if (enraged) {
				for (int i = 0; i < 1; i++) {
					Color color2 = drawColor;
					color2 = NPC.GetAlpha(color2) * colo;
					Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X + 20 + posBX - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - 40 + posBY - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
					Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X - 20 - posBX - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y + 40 - posBY - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}

		private void HalfCircle () {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				ushort type = (ushort) ModContent.ProjectileType<TurkorKnife>();
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 8, 0, type, NPC.damage / 2, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 6, -4, type, NPC.damage / 2, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -6, type, NPC.damage / 2, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -8, 0, type, NPC.damage / 2, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -6, -4, type, NPC.damage / 2, 1, Main.myPlayer, 0, 0);
			}
			SoundEngine.PlaySound(SoundID.Item71, NPC.position);
			if (Main.netMode != NetmodeID.Server) {
				for (int num623 = (int) NPC.position.X - 20; num623 < (int) NPC.position.X + NPC.width + 40; num623 += 20) {
					for (int num624 = 0; num624 < 4; num624 = num + 1) {
						int dust = Dust.NewDust(new Vector2(NPC.position.X - 20f, NPC.position.Y + NPC.height), NPC.width + 20, 4, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
						Dust dust3 = Main.dust [dust];
						dust3.velocity *= 0.2f;
						num = num624;
					}
					int num626 = Gore.NewGore(NPC.GetSource_FromAI(), new Vector2((num623 - 20), NPC.position.Y + NPC.height - 8f), default, Main.rand.Next(61, 64), 1f);
					Gore gore = Main.gore [num626];
					gore.velocity *= 0.4f;
				}
			}
			SoundEngine.PlaySound(SoundID.Item88, NPC.position);
			NPC.netUpdate = true;
		}


		private readonly int turkorHead = ModContent.NPCType<TurkortheUngratefulHead>();

		private float colo = 0f;

		//head related
		private bool headSpawned;
		private int headNumber;

		//idling phase stuff
		private ref float timer => ref NPC.ai [0];
		private ref float timer2 => ref NPC.ai [3];
		//bool onAir = false;
		//bool Despawning = false;
		private bool ground_ = false;
		private int num = 0;
		private bool teleport = false;

		//jumptimer 
		private int jumpTimer = 1200;

		//player old position stuff
		private bool findPlayer = false;
		private float posX = 0f;
		private float posY = 0f;

		private float spreadAngle = 1f;

		//enraged mode
		private bool enraged;


		public override void ReceiveExtraAI (BinaryReader reader) {
			colo = reader.ReadSingle();
			headSpawned = reader.ReadBoolean();
			headNumber = reader.ReadInt32();
			ground_ = reader.ReadBoolean();
			num = reader.ReadInt32();
			teleport = reader.ReadBoolean();
			findPlayer = reader.ReadBoolean();
			posX = reader.ReadSingle();
			posY = reader.ReadSingle();
			enraged = reader.ReadBoolean();
		}

		public override void SendExtraAI (BinaryWriter writer) {
			writer.Write(colo);
			writer.Write(headSpawned);
			writer.Write(headNumber);
			writer.Write(ground_);
			writer.Write(num);
			writer.Write(teleport);
			writer.Write(findPlayer);
			writer.Write(posX);
			writer.Write(posY);
			writer.Write(enraged);
		}

		public static int GetFirstTileFloor (int x, int startY, bool solid = true) {
			for (int y = startY; y < Main.maxTilesY; y++) {
				Tile tile = Main.tile [x, y];
				if (tile != null && tile.HasTile && (!solid || Main.tileSolid [(int) tile.TileType])) { return y; }
			}
			return Main.maxTilesY;
		}


		public override void AI () {
			Player player = Main.player [NPC.target];

            NPC.TargetClosest(true);
            int target = NPC.target;
            bool flag = target < 0 || target == 255;
            player = Main.player[target];
            if (player.dead || flag) {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
                if (player.dead || flag) {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    if (Main.netMode == NetmodeID.Server) {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                    }
                    return;
                }
            }

            if (NPC.localAI [0] == 0f) {
				NPC.localAI [0] = 1f;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                string typeName = NPC.TypeName;
                if (Main.netMode == 0)
                    Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
                else if (Main.netMode == 2)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", NPC.GetTypeNetName()), new Color(175, 75, 255));
                float x = player.position.X + Main.rand.NextFloat(50f, 150f) * 5f * (Main.rand.NextBool() ? -1f : 1f);
				int y = GetFirstTileFloor((int) x / 16, (int) (NPC.Center.Y / 16f) + 3);
				Vector2 position = new Vector2 { X = x, Y = y * 16f };
				NPC.position = position;
				if (NPC.CountNPCS(turkorHead) <= 0 || !headSpawned) {
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.position.X, (int) NPC.position.Y, turkorHead, 0, 0, NPC.whoAmI);
						NetMessage.SendData(MessageID.SyncNPC, number: npc);
					}
					headSpawned = true;
					headNumber++;
				}
				NPC.netUpdate = true;
				return;
			}

			int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y - 20), NPC.width, NPC.height, DustID.Smoke, 0f, -6f, 60, Color.White, 1f);
			Main.dust [dust].velocity *= 0.2f;

			bool isNotMpClient = (Main.netMode != NetmodeID.MultiplayerClient);

			//if player too far then becomes enraged
			Vector2 vector101 = new Vector2(NPC.Center.X, NPC.Center.Y);
			float num855 = Main.player [NPC.target].Center.X - vector101.X;
			float num856 = Main.player [NPC.target].Center.Y - vector101.Y;
			float num857 = (float) Math.Sqrt((double) (num855 * num855 + num856 * num856));
			if (num857 > 600f) {
				if (isNotMpClient) NPC.ai [2] = 40;
				if (colo < .5f) colo += 0.05f;
				enraged = true;
			}
			else {

				if (colo > 0f) colo -= 0.05f;
				else {
					colo = 0;
					if (isNotMpClient) NPC.ai [2] = 0;
					enraged = false;
					if (NPC.AnyNPCs(turkorHead)) timer = 0;
				}
			}

			//check if stuck underground
			if (!ground_ && timer2 < 1200) {
				if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
					NPC.noTileCollide = true;
					NPC.velocity.Y = -6;
				}
				else {
					if (NPC.ai [2] != 40) { timer = 0; }
					NPC.noTileCollide = false;
					ground_ = true;
				}
				NPC.netUpdate = true;
			}

			NPC.dontTakeDamage = NPC.AnyNPCs(turkorHead) || NPC.ai [1] == 40 || enraged;

			//spawn heads
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (!headSpawned) {
					for (int i = 0; i < headNumber; ++i) {
						int turkorHead_ = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.position.X, (int) NPC.position.Y, turkorHead);
						NPC npc = Main.npc [turkorHead_];
						npc.velocity.X = Main.rand.Next(-6, 7);
						npc.velocity.Y = Main.rand.Next(-6, 7);
						npc.ai [1] = NPC.whoAmI;
						npc.TargetClosest(true);
						npc.netUpdate = true;
						NetMessage.SendData(MessageID.SyncNPC, number: turkorHead_);
					}
					headSpawned = true;
				}
			}

			//shoot projectiles at player 
			if ((!NPC.AnyNPCs(turkorHead) || enraged) && (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.SinglePlayer)) {
				timer++;
				if (timer >= 140 && NPC.ai [1] != 40) {
					if (!findPlayer && timer < 160) {
						posX = Main.player [NPC.target].position.X;
						posY = Main.player [NPC.target].position.Y;
						Vector2 Velocity = Vector2.Normalize(Main.player [NPC.target].Center - NPC.Center) * 14;
						int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Velocity, ModContent.ProjectileType<Pointer>(), 0, 1, Main.myPlayer, 0, 0);
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);

						if (headNumber == 3) {
							int proj2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Velocity.RotatedBy(spreadAngle), ModContent.ProjectileType<Pointer>(), 0, 1, Main.myPlayer, 0, 0);
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj2);
							int proj3 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Velocity.RotatedBy(-spreadAngle), ModContent.ProjectileType<Pointer>(), 0, 1, Main.myPlayer, 0, 0);
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj3);
						}

						findPlayer = true;
					}
					if (findPlayer && timer >= 160) {
						Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
						float rotation0 = (float) Math.Atan2((vector8.Y) - (posY + (Main.player [NPC.target].height * 0.5f)), (vector8.X) - (posX + (Main.player [NPC.target].width * 0.5f)));
						if (timer % 5 == 0) {
							SoundEngine.PlaySound(SoundID.Item42, NPC.position);
							int proj4 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<TurkorFeather>(), NPC.damage / 3, 1, Main.myPlayer, 0, 0);
							Main.projectile [proj4].aiStyle = -1;
							Main.projectile [proj4].velocity.X = (float) (Math.Cos(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
							Main.projectile [proj4].velocity.Y = (float) (Math.Sin(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj4);

							if (headNumber == 3) {
								int proj5 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<TurkorFeather>(), NPC.damage / 3, 1, Main.myPlayer, 0, 0);
								Main.projectile[proj5].aiStyle = -1;
								Main.projectile[proj5].velocity.X = (float)(Math.Cos(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
								Main.projectile[proj5].velocity.Y = (float)(Math.Sin(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
								Main.projectile[proj5].velocity = Main.projectile[proj5].velocity.RotatedBy(spreadAngle);
								NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj5);

								int proj6 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<TurkorFeather>(), NPC.damage / 3, 1, Main.myPlayer, 0, 0);
								Main.projectile[proj6].aiStyle = -1;
								Main.projectile[proj6].velocity.X = (float)(Math.Cos(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
								Main.projectile[proj6].velocity.Y = (float)(Math.Sin(rotation0) * 18) * -1 + Main.rand.Next(-3, 3);
								Main.projectile[proj6].velocity = Main.projectile[proj6].velocity.RotatedBy(-spreadAngle);
								NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj6);
							}
						}
						if (timer >= 180) {
							posX = 0;
							posY = 0;
							timer = NPC.AnyNPCs(turkorHead) ? 0 : 70;
							findPlayer = false;
						}
					}
				}
			}

			//idling phase
			if (!NPC.AnyNPCs(turkorHead)) {
				timer2++;

				if (timer2 >= jumpTimer - 100) {
					if (isNotMpClient) NPC.ai [1] = 40;
					NPC.rotation = Vector2.UnitY.RotatedBy((double) (timer / 25f * 6.3f), default).Y * 0.2f;
				}

				//preventing melee cheese
				if (NPC.ai [1] != 40 && timer2 < jumpTimer && (NPC.life <= (int) (NPC.lifeMax * 0.75f) && headNumber < 2 || NPC.life <= (int) (NPC.lifeMax * 0.4f) && headNumber < 3)) {
					if (isNotMpClient) {
						timer2 = jumpTimer - 100;
						NPC.ai [1] = 40;
					}
				}

				if (timer2 >= jumpTimer) {
					NPC.rotation += 0.2f;
					NPC.noGravity = true;
					if (timer2 <= jumpTimer) {
						HalfCircle();
						NPC.noTileCollide = true;
						SoundEngine.PlaySound(SoundID.Roar, NPC.position);
						NPC.velocity.Y = -32;
						NPC.netUpdate = true;
					}
					if (timer2 >= (jumpTimer + 20) && !teleport) {
						if (NPC.alpha <= 255) NPC.alpha += 5;
						else {
							NPC.velocity.Y = 0;
							teleport = true;
							timer2 = jumpTimer + 60;
						}
					}
					if (teleport && timer2 >= (jumpTimer + 60)) {
						if (timer2 <= (jumpTimer + 60)) {
							NPC.position.X = Main.player [NPC.target].Center.X - 40;
							NPC.position.Y = Main.player [NPC.target].Center.Y - 800;
							NPC.netUpdate = true;
						}
						NPC.alpha -= 8;

						NPC.velocity.Y = 26;
						if (Main.player [NPC.target].position.Y <= NPC.position.Y) {
							if (Main.tile [(int) (NPC.Center.X / 16), (int) (NPC.Center.Y / 16)].HasTile || Collision.SolidCollision(NPC.position, NPC.width, NPC.height)) {
								//reset everything
								NPC.noTileCollide = false;
								timer2 = 0;

								NPC.alpha = 0;
								NPC.rotation = 0;
								ground_ = false;
								if (headNumber < 3) headNumber += 1;
								headSpawned = false;
								NPC.noGravity = false;
								NPC.ai [1] = 0;
								HalfCircle();
								teleport = false;
								posX = 0;
								posY = 0;
								timer = 0;
								findPlayer = false;
								NPC.velocity = Vector2.Zero;
								NPC.netUpdate = true;
							}
						}
					}
				}
			}
		}

		public override void HitEffect (NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server)
				return;

			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorMeatGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorMeatGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
			}
		}

		public override void OnKill ()
			=> NPC.SetEventFlagCleared(ref DownedBossSystem.downedTurkor, -1);

		public override void BossLoot (ref string name, ref int potionType)
			=> potionType = ItemID.HealingPotion;

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TurkorTrophy>(), 10));

			Conditions.NotExpert notExpert = new Conditions.NotExpert();
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TurkorBag>()));
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TurkorRelic>()));
			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<FruitfulPlate>(), 4));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(new OneFromOptionsDropRule(1, 1, ModContent.ItemType<FeatherStorm>(), ModContent.ItemType<GreatDrumstick>(), ModContent.ItemType<TurkeyStuff>()));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<SpicySauce>(), 2, 15, 34));
			npcLoot.Add(ItemDropRule.ByCondition(notExpert, ModContent.ItemType<TurkorMask>(), 7));
		}
	}
}