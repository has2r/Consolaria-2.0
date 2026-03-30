using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class SirenHelmet : ThoriumItem_BardBase {
    public override void Load() {
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += On_PlayerDrawLayers_DrawPlayer_13_Leggings;
    }

    private void On_PlayerDrawLayers_DrawPlayer_13_Leggings(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo) {
        Player player = drawinfo.drawPlayer;
        bool wearingSirenLegs = player.legs == EquipLoader.GetEquipSlot(Mod, nameof(SirenLegs), EquipType.Legs);

        Vector2 previousPosition = drawinfo.Position;
        if (wearingSirenLegs) {
            drawinfo.Position.X -= 6f * player.direction;
        }

        orig(ref drawinfo);

        if (wearingSirenLegs) {
            drawinfo.Position = previousPosition;
        }
    }

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
        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSirenSetBonusActive = true;
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
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

