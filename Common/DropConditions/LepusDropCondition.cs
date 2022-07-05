using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Consolaria.Common
{
	public class LepusDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) 
		{
			return true;
		}

		public bool CanShowItemDropInUI() => true;
		
		public string GetConditionDescription() => "";	
	}
}
