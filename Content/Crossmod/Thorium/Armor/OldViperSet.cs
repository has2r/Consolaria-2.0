using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldViperHelmet : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ViperHelmet>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(28, 24);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 15;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ViperHelmet.ApplyEquipEffects(player);
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => (body.type == ModContent.ItemType<ViperChestplate>() || body.type == ModContent.ItemType<OldViperChestplate>())
        && (legs.type == ModContent.ItemType<ViperLegs>() || legs.type == ModContent.ItemType<OldViperLegs>());

    public override void UpdateArmorSet(Player player) {
        player.setBonus = ViperHelmet.SetBonusText.ToString();

        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsViperSetBonusActive = true;
    }

    public override void ArmorSetShadows(Player player) {

    }
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldViperChestplate : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ViperChestplate>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(30, 20);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 22;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ViperChestplate.ApplyEquipEffects(player);
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldViperLegs : ThoriumItem_ThrowerBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ViperLegs>();
    }

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(22, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ViperLegs.ApplyEquipEffects(player);
    }
}

