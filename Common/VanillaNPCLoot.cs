using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Items.Summons;

namespace Consolaria.Common
{
	public class VanillaNPCLoot : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if (npc.type == NPCID.Harpy) {
				int itemType = ModContent.ItemType<CursedStuffing>();
				int chance = 10;

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
				int chance = 10;

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
		}
	}
}
