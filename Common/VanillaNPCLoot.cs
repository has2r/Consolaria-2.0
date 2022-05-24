using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Vanity;

namespace Consolaria.Common
{
	public class VanillaNPCLoot : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if (npc.type == NPCID.FireImp) 
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShirenHat>(), 250));	
		}
	}
}
