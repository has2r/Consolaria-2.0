using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Consolaria.Common {
	public class DrunkWorldCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return Main.drunkWorld;
            return false;
		}

		public bool CanShowItemDropInUI () => true;

		public string GetConditionDescription () => "Drops  only in 'drunk' worlds";
	}

	public class ForTheWorthyCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
                return Main.getGoodWorld;
            return false;
		}

		public bool CanShowItemDropInUI () => true;

		public string GetConditionDescription () => "If you are worthy enough";
	}

	public class AnniversaryWorldCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return Main.tenthAnniversaryWorld;
			return false;
		}

		public bool CanShowItemDropInUI () => true;

		public string GetConditionDescription () => "Drops in Celebration worlds";
	}

	public class GetFixedBoiCondition : IItemDropRuleCondition {
		public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return Main.zenithWorld;
			return false;
		}

		public bool CanShowItemDropInUI () => false;

		public string GetConditionDescription () => "Drops on Legendary mode";
	}

    public class UpsideDownWorldCondition : IItemDropRuleCondition {
        public bool CanDrop (DropAttemptInfo info) {
            if (!info.IsInSimulation)
                return Main.remixWorld;
            return false;
        }

        public bool CanShowItemDropInUI () => false;

        public string GetConditionDescription () => "Drops if you are upside down";
    }

    public class HardSeedsCondition : IItemDropRuleCondition {
        public bool CanDrop (DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return Main.zenithWorld || Main.remixWorld || Main.getGoodWorld;
            return false;
        }

        public bool CanShowItemDropInUI () => false;

        public string GetConditionDescription () => "Drops in any of the hardcore seed worlds";
    }
}