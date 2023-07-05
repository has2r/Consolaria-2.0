using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common.GlobalItems {
    public class BossBagLoot : GlobalItem {
        public override void ModifyItemLoot (Item item, ItemLoot itemLoot) {
            if (item.type == ItemID.EaterOfWorldsBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Pets.SuspiciousLookingApple>(), 20));
            if (item.type == ItemID.Present)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Placeable.Topper505>(), 50));
        }
    }
}