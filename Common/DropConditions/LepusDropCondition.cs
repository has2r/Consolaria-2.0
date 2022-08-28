using Consolaria.Content.NPCs.Bosses.Lepus;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Consolaria.Common
{
	public class LepusDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			return !info.IsInSimulation;
		}

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => /*"This can be dropped if Lepus is not alive"*/"";
	}

	public class LepusDropCondition2 : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) 
		{
			if (!info.IsInSimulation)
			return !NPC.AnyNPCs(ModContent.NPCType<Lepus>()) && !RabbitInvasion.rabbitInvasion;
			return false;
		}

		public bool CanShowItemDropInUI() => true;
		
		public string GetConditionDescription() => "This can be dropped if Lepus is not alive";	
	}

	public class LepusDropCondition1 : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
				return RabbitInvasion.rabbitInvasion;
			return false;
		}

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => "This can be dropped during invasion of rabbits";
	}
}
