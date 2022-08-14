using Consolaria.Content.Items.Pets;
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

namespace Consolaria.Content.NPCs {
	public class DragonHornet : ModNPC {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Dragon Hornet");
			Main.npcFrameCount [NPC.type] = 3;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int [] {
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

		public override void SetDefaults () {
			int width = 36; int height = 34;
			NPC.Size = new Vector2(width, height);

			NPC.damage = 20;
			NPC.defense = 16;
			NPC.lifeMax = 75;

			NPC.value = Item.buyPrice(silver: 1);
			NPC.knockBackResist = 0.4f;
			NPC.noGravity = true;

			NPC.aiStyle = 14;
			AnimationType = NPCID.Hornet;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.DragonHornetBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				new FlavorTextBestiaryInfoElement("Relatives of common Hornets, these insects developed an unusual appearance to scare off enemies. If that's not enough, the poisonous stingers should do the job.")
			});
		}

		public override void AI () {
			Player player = Main.player [NPC.target];

			Vector2 vector = player.Center + new Vector2(NPC.Center.X, NPC.Center.Y);
			Vector2 vector2 = NPC.Center + new Vector2(NPC.Center.X, NPC.Center.Y);

			if (player.position.X > NPC.position.X) NPC.spriteDirection = 1;
			else NPC.spriteDirection = -1;

			NPC.ai [2] += 1f;
			if (NPC.ai [2] >= 0f && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) {
				float vel = (float) Math.Atan2((vector2.Y - vector.Y), (vector2.X - vector.X));
				SoundEngine.PlaySound(SoundID.Item17, NPC.position);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float) (Math.Cos(vel) * 10.0 * -1.0), (float) (Math.Sin(vel) * 10.0 * -1.0), ProjectileID.Stinger, 20, 1f);
				NPC.ai [2] = -120f;
				NPC.netUpdate = true;
			}
		}

		public override void OnHitPlayer (Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.Poisoned, 180);

		public override void HitEffect (int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/DragonHornetGore").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/DSnatcherGore1").Type);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, 2.5f * hitDirection, -2.5f, 0, default, 1f);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Beeswax>(), 15));
			npcLoot.Add(ItemDropRule.Common(ItemID.Bezoar, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.Stinger, 2));
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> SpawnCondition.UndergroundJungle.Chance * 0.025f;
	}
}