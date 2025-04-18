using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Legs)]

    public class AncientPlumbersPants : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PlumbersPants;
            ItemID.Sets.ShimmerTransformToItem[ItemID.PlumbersPants] = Type;
        }

        public override void SetDefaults() {
            int width = 30; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.buyPrice(gold: 25);
            Item.vanity = true;
        }
    }
}