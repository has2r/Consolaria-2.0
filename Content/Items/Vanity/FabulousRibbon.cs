using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Head)]

    public class FabulousRibbon : ModItem {
        public override void SetStaticDefaults() {

            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(gold: 15);
            Item.vanity = true;
        }
    }
}

