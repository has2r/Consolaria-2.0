using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class SeraphimHelmet : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSeraphimHelmet>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(28, 26);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 16;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Generic) -= 0.2f;
        player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.35f;
        player.manaCost -= 0.2f;
        player.GetThoriumPlayer().healBonus += 2;
        player.statManaMax2 += 60;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => head.type == Type && body.type == ModContent.ItemType<SeraphimChestplate>() && legs.type == ModContent.ItemType<SeraphimLegs>();

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimSetBonusActive = true;
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
}

[AutoloadEquip(EquipType.Body)]
public sealed class SeraphimChestplate : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSeraphimChestplate>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(38, 26);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 22;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Generic) -= 0.3f;
        player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.5f;
        player.manaCost -= 0.2f;
        player.GetThoriumPlayer().healBonus += 3;
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class SeraphimLegs : ThoriumItem_HealerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSeraphimLegs>();
    }

    public override void SetHealerDefaults() {
        Item.SetSizeValues(26, 20);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Generic) -= 0.2f;
        player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.35f;
        player.manaCost -= 0.15f;
        player.GetThoriumPlayer().healBonus += 2;
        player.moveSpeed += 0.15f;
    }
}

