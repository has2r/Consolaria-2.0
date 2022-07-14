using Consolaria.Content.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{
	public class VampireMiner : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 15;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.Bleeding
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

			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath2;

			NPC.damage = 25;
			NPC.defense = 3;

			NPC.lifeMax = 90;
			NPC.knockBackResist = 0.3f;

			NPC.value = Item.buyPrice(silver: 5);

			NPC.aiStyle = 3;
			AIType = 21;
			AnimationType = 21;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.VampireMinerBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("Some evidence indicates some spelunkers got bitten by vampire bats before they passed away... and now their rotten remains hunt anyone for tasty blood.")
			});
		}

		public override void AI()
			=> Lighting.AddLight(NPC.Center, new Vector3(0.8f, 0f, 0.7f));
		
		public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (Main.rand.NextBool(5))
				target.AddBuff(BuffID.Bleeding, 60 * 5);

			if (Main.rand.NextBool(2) && NPC.life < NPC.lifeMax) {
				for (int i = 0; i < 10; i++)
					Dust.NewDust(NPC.position, i, i, DustID.Blood, 2, 2, 100, default, 0.9f);

				int healLife = damage / 5;
				if (healLife > 0) {
					NPC.life += healLife;
					NPC.HealEffect(healLife, true);
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/vampgore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/vampgore2").Type, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			var minerDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.UndeadMiner, false);
			foreach (var minerDropRule in minerDropRules)
				npcLoot.Add(minerDropRule);
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VialOfBlood>(), 5));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> SpawnCondition.Underground.Chance * 0.15f;
	}
}
