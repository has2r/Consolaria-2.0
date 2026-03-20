using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class ViperHelmet : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(32, 28);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<ViperChestplate>() && legs.type == ModContent.ItemType<ViperLegs>();

    public override void UpdateArmorSet(Player player) {
        
    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class ViperChestplate : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(38, 24);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class ViperLegs : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(26, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

