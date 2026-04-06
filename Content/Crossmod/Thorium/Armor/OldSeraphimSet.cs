using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldSeraphimHelmet : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SeraphimHelmet>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(22, 20);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 16;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SeraphimHelmet.ApplyEquipEffects(player);
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => (body.type == ModContent.ItemType<SeraphimChestplate>() || body.type == ModContent.ItemType<OldSeraphimChestplate>())
        && (legs.type == ModContent.ItemType<SeraphimLegs>() || legs.type == ModContent.ItemType<OldSeraphimLegs>());

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimSetBonusActive = true;
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldSeraphimChestplate : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SeraphimChestplate>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(30, 22);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 22;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SeraphimChestplate.ApplyEquipEffects(player);
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldSeraphimLegs : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SeraphimLegs>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(26, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SeraphimLegs.ApplyEquipEffects(player);
    }
}

