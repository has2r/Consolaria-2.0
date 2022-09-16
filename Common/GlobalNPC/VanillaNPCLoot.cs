using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Consumables;

namespace Consolaria.Common {
	public class VanillaNPCLoot : GlobalNPC {
		public override void ModifyNPCLoot (NPC npc, NPCLoot npcLoot) {
			if (npc.type == NPCID.Harpy) {
				int itemType = ModContent.ItemType<CursedStuffing>();
				int dropChance = !DownedBossSystem.downedTurkor ? 4 : 10;
				ThanksgivingDropCondition thanksgivingDropCondition = new ThanksgivingDropCondition();

				if (SeasonalEvents.configEnabled)
					npcLoot.Add(ItemDropRule.ByCondition(thanksgivingDropCondition, itemType, dropChance));
				else npcLoot.Add(ItemDropRule.Common(itemType, dropChance));
			}

			if (npc.type == NPCID.CorruptBunny || npc.type == NPCID.CrimsonBunny) {
				int itemType = ModContent.ItemType<SuspiciousLookingEgg>();
				int dropChance = 4;
				LepusDropCondition lepusDropCondition = new LepusDropCondition();
				EasterDropCondition easterDropCondition = new EasterDropCondition();

				if (SeasonalEvents.configEnabled) {
					IItemDropRule easterConditionalRule = new LeadingConditionRule(easterDropCondition);
					easterConditionalRule.OnSuccess(ItemDropRule.ByCondition(lepusDropCondition, itemType, dropChance));
					npcLoot.Add(easterConditionalRule);
				}
				else npcLoot.Add(ItemDropRule.ByCondition(lepusDropCondition, itemType, dropChance));
			}
			if (npc.type == NPCID.FireImp)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShirenHat>(), 250));
			if (npc.type == NPCID.Werewolf)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WolfFang>(), 150));
			if (npc.type == NPCID.ToxicSludge)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetriDish>(), 150));
			if (NPCID.Sets.Zombies [npc.type])
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Brain>(), 750));
		}

		public override void ModifyGlobalLoot (GlobalLoot globalLoot) {
			int itemType = ModContent.ItemType<RedEnvelope>();
			int chance = 30;
			ChineseNewYearDropCondition chineseNewYearDropCondition = new ChineseNewYearDropCondition();
			LanternNightDropCondition lanernNightDropCondition = new LanternNightDropCondition();

			if (SeasonalEvents.configEnabled)
				globalLoot.Add(ItemDropRule.ByCondition(chineseNewYearDropCondition, itemType, chance));
			else
				globalLoot.Add(ItemDropRule.ByCondition(lanernNightDropCondition, itemType, chance / 2));
		}
	}
}