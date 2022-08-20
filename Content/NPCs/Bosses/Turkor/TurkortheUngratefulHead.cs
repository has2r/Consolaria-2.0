using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Consolaria.Content.Projectiles.Enemies;
using Terraria.DataStructures;

namespace Consolaria.Content.NPCs.Bosses.Turkor {
	[AutoloadBossHead]
	public class TurkortheUngratefulHead : ModNPC {
		private int turntimer = 0;
		private ref float timer => ref NPC.ai [0];

		private bool spawn = false;
		private bool charge {
			get => NPC.ai[2] == 1f;
			set => NPC.ai[2] = value ? 1f : 0f;
		}
		private bool chase = false;
		private bool projSpam {
			get => NPC.ai[3] == 1f;
			set => NPC.ai[3] = value ? 1f : 0f;
		}
		private bool attackingPhase = false;

		private int hurtFrame = 0;
		private float rotatepoint = 0;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Turkor the Ungrateful Head");
			Main.npcFrameCount [NPC.type] = 4;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int [] {
					BuffID.Poisoned,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults () {
			int width = 50; int height = 100;
			NPC.Size = new Vector2(width, height);

			NPC.aiStyle = -1;

			NPC.damage = 40;
			NPC.defense = 10;
			NPC.lifeMax = 1200;

			NPC.dontTakeDamage = false;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble");

			NPC.knockBackResist = 0f;
			NPC.noTileCollide = true;

			NPC.alpha = 255;

			NPC.lavaImmune = true;
			NPC.noGravity = true;

			NPC.BossBar = Main.BigBossProgressBar.NeverValid;
		}
        
        public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			NPC.lifeMax = 2000 + (int) (numPlayers > 1 ? NPC.lifeMax * 0.2 * numPlayers : 0);
			NPC.damage = (int) (NPC.damage * 0.65f);
		}

		private Rectangle GetFrame (int number)
			=> new Rectangle(0, NPC.frame.Height * (number - 1), NPC.frame.Width, NPC.frame.Height);

		public override void FindFrame (int frameHeight) {
			if (!attackingPhase || charge && timer > 230 || projSpam) {
				if (!projSpam && NPC.velocity.X * NPC.direction < 0 && turntimer < 15) {
					turntimer++;
					NPC.frame = GetFrame(4);
				}
				else if (hurtFrame > 0) {
					NPC.frame = GetFrame(3);
					hurtFrame--;
				}
				else {
					if (NPC.velocity.X * NPC.direction > 0) { turntimer = 0; }
					NPC.spriteDirection = NPC.direction;
					if (charge) {
						NPC.frameCounter += 0.08f;
						NPC.frameCounter %= 2;
						int frame = (int) NPC.frameCounter;
						NPC.frame.Y = frame * frameHeight;
					}
					else if (projSpam && timer % 80 < 20) NPC.frame = GetFrame(2);
					else NPC.frame = GetFrame(1);
				}
			}
			if (charge && timer <= 230) NPC.frame = GetFrame(4);
		}

		public override bool CanHitPlayer (Player target, ref int cooldownSlot) => charge;

		public override void AI () {
			NPC.direction = Main.player [NPC.target].Center.X < NPC.Center.X ? -1 : 1;
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (!spawn) {
					NPC.realLife = NPC.whoAmI;
					int neck = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.position.X, (int) NPC.position.Y, ModContent.NPCType<TurkorNeck>(), NPC.whoAmI, 0, NPC.whoAmI); //, 1, NPC.ai[1]);
					Main.npc [neck].ai [2] = 30;
					Main.npc [neck].ai [3] = -1f;
					Main.npc [neck].realLife = NPC.whoAmI;
					Main.npc [neck].ai [0] = NPC.whoAmI;
					Main.npc [neck].ai [1] = NPC.whoAmI;
					NetMessage.SendData(MessageID.SyncNPC, number: neck);
					spawn = true;
					NPC.netUpdate = true;
				}
			}
			if (!Main.npc [(int) NPC.ai [1]].active) {
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.active = false;
			}
			if (NPC.alpha >= 0) NPC.alpha -= 15;

			timer++;
			if (Main.player [NPC.target].dead) {
				timer = 0;
				charge = false;
				NPC.TargetClosest(false);
				NPC.velocity.Y -= 0.1f;
				if (NPC.timeLeft > 10 && Main.player [NPC.target].dead) {
					NPC.timeLeft = 10;
					return;
				}
				NPC.netUpdate = true;
			}
			else if (!Main.player [NPC.target].dead) NPC.TargetClosest(true);

			if (timer > 200 && Main.rand.NextBool(50) && !attackingPhase) {
				attackingPhase = true;
				//pick random attack
				switch (Main.rand.Next(2)) {
				case 0:
				charge = true;
				break;

				case 1:
				projSpam = true;
				break;

				default:
				break;
				}
				timer = 200;
				NPC.velocity *= 0.46f;
				NPC.rotation = 0;
				NPC.netUpdate = true;
			}

			//attack1: charge at player
			if (charge) {
				if (timer <= 230) {
					NPC.rotation = Vector2.UnitY.RotatedBy((double) (timer / 40f * 6.2f), default).Y * 0.2f;
				}
				if (timer >= 230) {
					NPC.rotation = 0;
					if (timer <= 230) SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/TurkorGobble"), NPC.position); // SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y, 10);

					NPC.velocity.X *= 0.98f;
					NPC.velocity.Y *= 0.98f;
					Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
					{
						float rotation = (float) Math.Atan2((vector8.Y) - (Main.player [NPC.target].position.Y + (Main.player [NPC.target].height * 0.5f)), (vector8.X) - (Main.player [NPC.target].position.X + (Main.player [NPC.target].width * 0.5f)));
						NPC.velocity.X = (float) (Math.Cos(rotation) * 14) * -1;
						NPC.velocity.Y = (float) (Math.Sin(rotation) * 14) * -1;
					}
				}

				//if near player then bounce back
				if (timer >= 230 && Main.player [NPC.target].Distance(NPC.Center) <= 30) {
					timer = 0;
					charge = false;
					attackingPhase = false;
					NPC.velocity.X *= -0.38f;
					NPC.velocity.Y *= -0.38f;
				}

				//if hit/running out of time then bounce back
				if (timer > 270 || timer > 230 && NPC.justHit) {
					if (timer <= 270) hurtFrame = 20;
					timer = 0;
					charge = false;
					attackingPhase = false;
					NPC.velocity.X *= -0.38f;
					NPC.velocity.Y *= -0.38f;
				}
				NPC.netUpdate = true;
			}

			//attck2: spawn feather around it self while slowly drift toward the player
			if (projSpam) {
				if (NPC.spriteDirection == -1) NPC.rotation = rotatepoint;
				else NPC.rotation = -rotatepoint;
				if (rotatepoint <= 1.5f && timer < 360) rotatepoint += 0.1f;
				if (!chase) {
					NPC.velocity.X *= 0.86f;
					NPC.velocity.Y *= 0.86f;
					NPC.netUpdate = true;
				}

				if (timer % 80 == 0 && rotatepoint >= 1.5f) {
					if (Main.netMode != NetmodeID.MultiplayerClient)
						for (int i = 0; i < 3; i++) {
							int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Main.rand.Next(0, 8) * NPC.direction, -10 + Main.rand.Next(-3, 3), ModContent.ProjectileType<TurkorFeather>(), NPC.damage / 3, 1, Main.myPlayer);
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
						}
					NPC.velocity.Y = 5;
					SoundEngine.PlaySound(SoundID.NPCDeath48, NPC.position);
					NPC.netUpdate = true;
				}
				if (timer >= 360) {
					rotatepoint -= 0.1f;
					if (rotatepoint <= 0) {
						timer = 0;
						projSpam = false;
						attackingPhase = false;
						NPC.rotation = 0;
					}
				}
				NPC.netUpdate = true;
			}

			if (!charge) {
				if (!chase) {
					if (Main.player [NPC.target].Center.X - Main.rand.Next(-200, 201) < NPC.Center.X) {
						if (NPC.velocity.X > -6) NPC.velocity.X -= 0.08f;
					}
					else if (Main.player [NPC.target].Center.X - Main.rand.Next(-200, 201) > NPC.Center.X) {
						if (NPC.velocity.X < 6) NPC.velocity.X += 0.08f;
					}
					if (Main.player [NPC.target].Center.Y - Main.rand.Next(-150, 201) < NPC.Center.Y) {
						if (NPC.velocity.Y > -6) NPC.velocity.Y -= 0.14f;
					}
					else if (Main.player [NPC.target].Center.Y - Main.rand.Next(-150, 201) > NPC.Center.Y) {
						if (NPC.velocity.Y < 6) NPC.velocity.Y += 0.14f;
					}
				}
				else {
					if (Main.npc [(int) NPC.ai [1]].Center.X - Main.rand.Next(-200, 201) < NPC.Center.X) {
						if (NPC.velocity.X > -6) NPC.velocity.X -= 0.08f;
					}
					else if (Main.npc [(int) NPC.ai [1]].Center.X - Main.rand.Next(-200, 201) > NPC.Center.X) {
						if (NPC.velocity.X < 6) NPC.velocity.X += 0.08f;
					}
					if (Main.npc [(int) NPC.ai [1]].Center.Y - Main.rand.Next(-150, 201) < NPC.Center.Y) {
						if (NPC.velocity.Y > -6) NPC.velocity.Y -= 0.14f;
					}
					else if (Main.npc [(int) NPC.ai [1]].Center.Y - Main.rand.Next(-150, 201) > NPC.Center.Y) {
						if (NPC.velocity.Y < 6) NPC.velocity.Y += 0.14f;
					}
				}
				Vector2 vector101 = new Vector2(NPC.Center.X, NPC.Center.Y);
				float num855 = Main.npc [(int) NPC.ai [1]].Center.X - vector101.X;
				float num856 = Main.npc [(int) NPC.ai [1]].Center.Y - vector101.Y;
				float num857 = (float) Math.Sqrt((double) (num855 * num855 + num856 * num856));
				if (num857 > 600f) chase = true;
				if (num857 <= 400 && chase) chase = false;
				NPC.netUpdate = true;
			}
		}

		public override void HitEffect (int hitDirection, double damage) {
			if (Main.netMode == NetmodeID.Server)
				return;

			if (NPC.life <= 0) {
				if (NPC.life <= 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-3, 4), Main.rand.Next(-3, 4)), ModContent.Find<ModGore>("Consolaria/TurkorBeakGore").Type);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorEyeGore").Type);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/TurkorEyeGore").Type);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.Find<ModGore>("Consolaria/TurkorFeatherGore").Type);
				}
				for (int k = 0; k < 10; k++) {
					int dust_ = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 3f * hitDirection, -3f, 0, default, 2f);
					Main.dust [dust_].velocity *= 0.2f;
				}
			}
		}
	}
}
