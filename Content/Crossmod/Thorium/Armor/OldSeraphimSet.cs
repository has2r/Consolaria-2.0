using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldSeraphimHelmet : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(22, 20);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<OldSeraphimChestplate>() && legs.type == ModContent.ItemType<OldSeraphimLegs>();

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimSetBonusActive = true;
    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldSeraphimChestplate : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(30, 22);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldSeraphimLegs : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(26, 18);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

