using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldSirenHelmet : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(22, 24);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<OldSirenChestplate>() && legs.type == ModContent.ItemType<OldSirenLegs>();

    public override void UpdateArmorSet(Player player) {
        
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldSirenChestplate : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(30, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldSirenLegs : ThoriumItem_BardBase {
    public override void SetBardDefaults() {
        Item.SetSizeValues(22, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

