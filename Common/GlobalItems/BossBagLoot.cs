using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Pets;

namespace Consolaria.Common.GlobalItems
{
	public class BossBagLoot : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot) 
        {
			if(item.type == ItemID.EaterOfWorldsBossBag) 
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuspiciousLookingApple>(), 20));
		}
	}
}