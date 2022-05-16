using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{
	public class ShadowMummy : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 15;

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

			NPC.damage = 60;
			NPC.defense = 15;

			NPC.lifeMax = 220;
			NPC.knockBackResist = 0.1f;

			NPC.value = Item.buyPrice(silver: 7);

			NPC.noGravity = false;
			NPC.lavaImmune = false;

			NPC.aiStyle = 3;
			AIType = NPCID.DarkMummy;
			AnimationType = NPCID.DarkMummy;

			//banner = NPC.type;
			//bannerItem = mod.ItemType("ShadowMummyBanner");
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
				new FlavorTextBestiaryInfoElement("With the sands transmogrified by outside forces, those put to rest in the desert, whether good or evil, now rise to maim and kill.")
			});
		}

        public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.Silenced, 60 * 8);
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Darkness, 60 * 15);
		}

        public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, 109, 2.5f * (float)hitDirection, -2.5f, 0, default, 1f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 109, 2.5f * (float)hitDirection, -2.5f, 0, default, 1f);	
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			var mummysDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DarkMummy, false);
			foreach (var mummyDropRule in mummysDropRules)
				npcLoot.Add(mummyDropRule);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> ((spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) && spawnInfo.Player.ZoneDesert && Main.hardMode) ? SpawnCondition.DarkMummy.Chance * 0.33f : 0f;	
	}
}
