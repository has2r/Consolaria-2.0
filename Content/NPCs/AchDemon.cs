using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{	
	public class AchDemon : ModNPC
	{
		private float aiTimer;

		public override void SetStaticDefaults()  {
			DisplayName.SetDefault("Arch Demon");
			Main.npcFrameCount[NPC.type] = 2;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.OnFire,
					BuffID.OnFire3,
					BuffID.ShadowFlame,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			int width = 90; int height = 50;
			NPC.Size = new Vector2(width, height);

			NPC.damage = 42;
			NPC.defense = 8;
			NPC.lifeMax = 300;

			NPC.value = Item.buyPrice(silver: 10);
			NPC.knockBackResist = 0.8f;
			NPC.noGravity = true;

			NPC.aiStyle = 14;
			AnimationType = NPCID.DemonEye;

			NPC.lavaImmune = true;

			NPC.HitSound = SoundID.NPCHit21;
			NPC.DeathSound = SoundID.NPCDeath24;

		    //Banner = NPC.type;
		    //BannerItem = mod.ItemType("ArchDemonBanner");
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("Demon, but more powerful, I guess...")
			});
		}

		public override void AI() {
			Player player = Main.player[NPC.target];

			aiTimer++;
			NPC.TargetClosest();

			if (aiTimer >= 25f && aiTimer <= 95f) NPC.dontTakeDamage = false;
			else {
				int dust = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f + 30, NPC.position.Y), 2, 2, 6, 0, -1f, 0, default, 1.75f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 0.95f;
				int dust2 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f - 30, NPC.position.Y), 2, 2, 6, 0, -1f, 0, default, 1.75f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].scale *= 0.95f;
				int dust3 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + 30), 2, 2, 6, 0, -1f, 0, default, 1.75f);
				Main.dust[dust3].noGravity = true;
				Main.dust[dust3].scale *= 0.95f;
				int dust4 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y - 30), 2, 2, 6, 0, -1f, 0, default, 1.75f);
				Main.dust[dust4].noGravity = true;
				Main.dust[dust4].scale *= 0.95f;
				
				NPC.dontTakeDamage = true;
			}

			if (aiTimer == 30f || aiTimer == 50f || aiTimer == 70f || aiTimer == 90f) {
				if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) {
					float vel_ = 0.2f;
					Vector2 shootVector = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
					float velX = Main.player[NPC.target].position.X + player.width * 0.5f - shootVector.X + Main.rand.Next(-100, 101);
					float velY = Main.player[NPC.target].position.Y + player.height * 0.5f - shootVector.Y + Main.rand.Next(-100, 101);
					float vel2_ = (float)Math.Sqrt((velX * velX + velY * velY));
					vel2_ = vel_ / vel2_;
					velX *= vel2_;
					velY *= vel2_;
					ushort pro = (ushort)Projectile.NewProjectile(NPC.GetSource_FromAI(), shootVector.X, shootVector.Y, velX, velY, ModContent.ProjectileType<ArchScythe>(), 52, 0f, player.whoAmI);
					Main.projectile[pro].timeLeft = 300;
					SoundEngine.PlaySound(SoundID.Item8, NPC.position);
				}
			}

			else if (aiTimer % 15 == 0) {
				int randomIdleSound = Main.rand.Next(0, 3);
				if (randomIdleSound == 0) SoundEngine.PlaySound(SoundID.Zombie, NPC.position, 26);
				if (randomIdleSound == 1) SoundEngine.PlaySound(SoundID.Zombie, NPC.position, 27);
				if (randomIdleSound == 2) SoundEngine.PlaySound(SoundID.Zombie, NPC.position, 28);
				if (randomIdleSound == 3) SoundEngine.PlaySound(SoundID.Zombie, NPC.position, 29);
			}

			else if (aiTimer >= (300 + Main.rand.Next(300))) aiTimer = 0;
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.OnFire, 180);
		
        public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, 185, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/ArchdemonGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/ArchdemonGore2").Type);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 185, 2.5f * hitDirection, -2.5f, 0, default(Color), 1f);		
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
			=> npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArchDemonMask>(), 15));
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> SpawnCondition.Underworld.Chance * 0.33f;
	}
}
