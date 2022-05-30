using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Consolaria.Common
{
	public class EasterDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
					return SeasonalEvents.isEaster;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => true;
		
		public string GetConditionDescription() => "Drops during Easter";	
	}

	public class ThanksgivingDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
					return SeasonalEvents.isThanksgiving;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => "Drops during Thanksgiving";
	}
}
