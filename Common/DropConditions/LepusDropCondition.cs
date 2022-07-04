using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Consolaria.Common
{
	public class LepusDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
					return NPC.CountNPCS(ModContent.NPCType<Content.NPCs.Bosses.Lepus.Lepus>()) == 1;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => true;
		
		public string GetConditionDescription() => "";	
	}
}
