using Consolaria.Content.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{	
	public class SpectralGastropod : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Spectrapod");
			Main.npcFrameCount[NPC.type] = 11;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
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
			int width = 40; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.damage = 45;
			NPC.defense = 5;

			NPC.lifeMax = 250;
			NPC.knockBackResist = 0.3f;

			NPC.value = Item.buyPrice(silver: 8);

			NPC.noGravity = true;

			NPC.aiStyle = 22;
			AIType = -1;
			AnimationType = 122;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.SpectralGastropodBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("Unlike Gastropods, these slimes can spit out balls of ghostly energy that are capable of chasing nearby living beings.")
			});
		}

		public override void AI() {
			Player player = Main.player[NPC.target];

			if (NPC.justHit) {
				NPC.ai[3] = 0f;
				NPC.localAI[1] = 0f;
			}

			Vector2 vector = player.Center + new Vector2(NPC.Center.X, NPC.Center.Y);
			Vector2 vector2 = NPC.Center + new Vector2(NPC.Center.X, NPC.Center.Y);
			NPC.netUpdate = true;

			if (player.position.X > NPC.position.X)
				NPC.spriteDirection = 1;	
			else NPC.spriteDirection = -1;
			
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[3] == 32f && !player.npcTypeNoAggro[NPC.type] && !player.dead) {
				SoundEngine.PlaySound(SoundID.Item29, NPC.position);
				float vel = (float)Math.Atan2((vector2.Y - vector.Y), (vector2.X - vector.X));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(vel) * 12 * -1.0), (float)(Math.Sin(vel) * 12 * -1.0), ModContent.ProjectileType<SpectrallBall>(), 60, 2f, player.whoAmI);
				NPC.netUpdate = true;
			}

			if (NPC.ai[3] > 0f) {
				NPC.ai[3] += 1f;
				if (NPC.ai[3] >= 64f)
					NPC.ai[3] = 0f;	
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[3] == 0f) {
				NPC.localAI[1] += 1f;
				if (NPC.localAI[1] > 120f && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && !player.npcTypeNoAggro[NPC.type]) {
					NPC.localAI[1] = 0f;
					NPC.ai[3] = 1f;
					NPC.netUpdate = true;
				}
			}

			Lighting.AddLight(NPC.Center, new Vector3(0f, 0.7f, 0.9f));
		}

		public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueFairy, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2, 11, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2, 12, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2, 13, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueFairy, 2.5f * hitDirection, -2.5f, 0, default, 1f);	
			}
		}

		public override Color? GetAlpha(Color lightColor)
			=> Color.White * 0.8f;

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			var gastropodDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Gastropod, false);
			foreach (var gastropodDropRule in gastropodDropRules)
				npcLoot.Add(gastropodDropRule);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> (spawnInfo.Player.ZoneHallow &&  Main.hardMode) ?
			SpawnCondition.OverworldHallow.Chance * 0.25f : 0f;
	}
}
