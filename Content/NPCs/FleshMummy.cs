using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class FleshMummy : ModNPC {
		public override void SetStaticDefaults () {
			Main.npcFrameCount [NPC.type] = 16;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 0.85f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults () {
			int width = 40; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.damage = 60;
			NPC.defense = 15;

			NPC.lifeMax = 250;
			NPC.knockBackResist = 0.1f;

			NPC.value = Item.buyPrice(silver: 10);

			NPC.noGravity = false;
			NPC.lavaImmune = false;

			NPC.aiStyle = 3;
			AIType = NPCID.BloodMummy;
			AnimationType = NPCID.BloodMummy;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.FleshMummyBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonDesert,
				new FlavorTextBestiaryInfoElement("With the sands transmogrified by outside forces, those put to rest in the desert, whether good or evil, now rise to maim and kill.")
			});
		}

		public override void OnHitPlayer (Player target, int damage, bool crit) {
			if (Main.rand.NextBool(5))
				target.AddBuff(BuffID.Silenced, 60 * 8);
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Bleeding, 60 * 5);
		}

		public override void HitEffect (int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * (float) hitDirection, -2.5f, 0, default, 1f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * (float) hitDirection, -2.5f, 0, default, 1f);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			var mummysDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.BloodMummy, false);
			foreach (var mummyDropRule in mummysDropRules)
				npcLoot.Add(mummyDropRule);
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> (spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneDesert && spawnInfo.SpawnTileY < Main.rockLayer && Main.hardMode) ?
			SpawnCondition.Crimson.Chance * 0.025f : 0f;
	}
}
