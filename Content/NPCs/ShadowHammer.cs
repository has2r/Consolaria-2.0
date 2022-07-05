using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{	
	public class ShadowHammer : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 6;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Confused,
					BuffID.OnFire,
					BuffID.Poisoned,
					BuffID.OnFire3
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 0.8f,
				Position = new Vector2(20f, 10f),
				PortraitPositionXOverride = -5f,
                PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			int width = 40; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.damage = 80;
			NPC.defense = 30;

			NPC.lifeMax = 200;
			NPC.knockBackResist = 0.6f;

			NPC.value = Item.buyPrice(silver: 15);

			NPC.noGravity = true;
			NPC.lavaImmune = true;

			NPC.aiStyle = 23;
			AIType = 83;
			AnimationType = 83;

			//banner = NPC.type;
			//bannerItem = mod.ItemType("ShadowHammerBanner");
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
				new FlavorTextBestiaryInfoElement("Magicked through the pure hatred of the world evil, this weapon flails about attempting to smash anything that breathes.")
			});
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Cursed, 60 * 5);
		}

		public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, 2.5f * (float)hitDirection, -2.5f, 0, default, 1f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, 2.5f * (float)hitDirection, -2.5f, 0, default, 1f);	
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			var hammerDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CursedHammer, false);
			foreach (var hammerDropRule in hammerDropRules)
				npcLoot.Add(hammerDropRule);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			int y = spawnInfo.SpawnTileY;
			return ((spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) && Main.hardMode && y < Main.rockLayer) ? SpawnCondition.EnchantedSword.Chance * 0.33f : 0f;
		}
	}
}
