using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class ViperHelmet : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldViperHelmet>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(32, 28);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 15;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Throwing) += 0.2f;
        player.GetThoriumPlayer().techPointsMax += 2;
        player.ThrownCost33 = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => (body.type == ModContent.ItemType<ViperChestplate>() || body.type == ModContent.ItemType<OldViperChestplate>())
        && (legs.type == ModContent.ItemType<ViperLegs>() || legs.type == ModContent.ItemType<OldViperChestplate>());

    public override void UpdateArmorSet(Player player) {
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsViperSetBonusActive = true;
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlinesForbidden = true;
}

[AutoloadEquip(EquipType.Body)]
public sealed class ViperChestplate : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldViperChestplate>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(38, 24);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 22;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Throwing) += 0.1f;
        player.ThrownVelocity += 0.2f;
        player.GetAttackSpeed(DamageClass.Throwing) += 0.15f;
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class ViperLegs : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldViperLegs>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(26, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(DamageClass.Throwing) += 0.2f;
        player.GetCritChance(DamageClass.Throwing) += 25f;
        player.moveSpeed += 0.15f;
    }
}

