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
				int chance = 4; // 25%

				if (SeasonalEvents.enabled) {
					ThanksgivingDropCondition thanksgivingDropCondition = new ThanksgivingDropCondition();
					IItemDropRule conditionalRule = new LeadingConditionRule(thanksgivingDropCondition);
					IItemDropRule rule = ItemDropRule.Common(itemType, chance);
					conditionalRule.OnSuccess(rule);
					npcLoot.Add(conditionalRule);
				}
				else npcLoot.Add(ItemDropRule.Common(itemType, chance));
			}

			if (npc.type == NPCID.CorruptBunny || npc.type == NPCID.CrimsonBunny) {
				int itemType = ModContent.ItemType<SuspiciousLookingEgg>();
				int chance = 4;

				if (SeasonalEvents.enabled) {
					EasterDropCondition easterDropCondition = new EasterDropCondition();
					IItemDropRule conditionalRule = new LeadingConditionRule(easterDropCondition);
					IItemDropRule rule = ItemDropRule.Common(itemType, chance);
					conditionalRule.OnSuccess(rule);
					npcLoot.Add(conditionalRule);
				}
				else npcLoot.Add(ItemDropRule.Common(itemType, chance));
			}

			if (npc.type == NPCID.FireImp)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShirenHat>(), 250));
			if (npc.type == NPCID.Werewolf)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WolfFang>(), 15));
			if (npc.type == NPCID.ToxicSludge)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetriDish>(), 25));
			if (NPCID.Sets.Zombies [npc.type])
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Brain>(), 125));
		}

		public override void ModifyGlobalLoot (GlobalLoot globalLoot) {
			int itemType = ModContent.ItemType<RedEnvelope>();
			int chance = 10;
			if (SeasonalEvents.enabled) {
				ChineseNewYearDropCondition chineseNewYearDropCondition = new ChineseNewYearDropCondition();
				IItemDropRule conditionalRule = new LeadingConditionRule(chineseNewYearDropCondition);
				IItemDropRule rule = ItemDropRule.Common(itemType, chance);
				conditionalRule.OnSuccess(rule);
				globalLoot.Add(conditionalRule);
			}
			else {
				LanternNightDropCondition lanernNightDropCondition = new LanternNightDropCondition();
				IItemDropRule conditionalRule = new LeadingConditionRule(lanernNightDropCondition);
				IItemDropRule rule = ItemDropRule.Common(itemType, chance);
				conditionalRule.OnSuccess(rule);
				globalLoot.Add(conditionalRule);
			}
		}
	}
}
