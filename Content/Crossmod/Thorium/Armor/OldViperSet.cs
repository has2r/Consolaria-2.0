using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldViperHelmet : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(28, 24);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<OldViperChestplate>() && legs.type == ModContent.ItemType<OldViperLegs>();

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsViperSetBonusActive = true;
    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldViperChestplate : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(30, 20);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldViperLegs : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(22, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

