using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class SirenHelmet : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(30, 28);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<SirenChestplate>() && legs.type == ModContent.ItemType<SirenLegs>();

    public override void UpdateArmorSet(Player player) {
        
    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class SirenChestplate : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(34, 22);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class SirenLegs : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(30, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

