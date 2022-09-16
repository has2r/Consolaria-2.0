using Consolaria.Content.NPCs.Bosses.Lepus;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Consolaria.Common {
	public class LepusDropCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return !NPC.AnyNPCs(ModContent.NPCType<Lepus>()) && !RabbitInvasion.rabbitInvasion;
			return false;
		}

		public bool CanShowItemDropInUI () => true;

		public string GetConditionDescription () => "Drops if Lepus is not present";
	}

	public class RabbitInvasionDropCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return RabbitInvasion.rabbitInvasion;
			return false;
		}

		public bool CanShowItemDropInUI () => true;

		public string GetConditionDescription () => "Drops during an invasion of rabbits";
	}
}