using Terraria;

namespace Consolaria.Content.Crossmod.Thorium.Accessories;

public sealed class PortableSpecialCorn : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.DefaultToAccessory();

        Item.SetSizeValues(36, 34);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
    }
}
