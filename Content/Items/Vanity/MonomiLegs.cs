using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Legs)]

    public class MonomiLegs : ModItem {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(silver: 15);
            Item.vanity = true;
        }
    }
}
