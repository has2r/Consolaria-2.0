using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

using ThoriumMod;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class SeraphimHelmet : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(28, 26);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<SeraphimChestplate>() && legs.type == ModContent.ItemType<SeraphimLegs>();

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimSetBonusActive = true;
    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class SeraphimChestplate : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(38, 26);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class SeraphimLegs : ThoriumItem_HealerBase {
    public override void SetHealerDefaults() {
        Item.SetSizeValues(26, 20);

        Item.SetShopValues(ItemRarityColor.White0, Item.sellPrice());

        int defense = 0;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {

    }
}

