using Consolaria.Content.Crossmod.Thorium.Projectiles;

using System;

using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class OldSirenHelmet : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SirenHelmet>();
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(22, 24);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SirenHelmet.ApplyEquipEffects(player);
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => (body.type == ModContent.ItemType<SirenChestplate>() || body.type == ModContent.ItemType<OldSirenChestplate>())
        && (legs.type == ModContent.ItemType<SirenLegs>() || legs.type == ModContent.ItemType<OldSirenLegs>());

    public override void UpdateArmorSet(Player player) {
        ApplySirenSetBonus(player);
    }

    public void ApplySirenSetBonus(Player player) {
        player.setBonus = SirenHelmet.SetBonusText.ToString();

        player.GetModPlayer<ThoriumPlayer_Consolaria>().IsSirenSetBonusActive = true;

        ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
        thoriumPlayer.setCyberPunk = true;
        ApplyEmpowerments(player);
        Player player2 = player;
        if (Main.netMode == 1) {
            player2 = Main.LocalPlayer;
        }

        ThoriumPlayer thoriumPlayer2 = player2.GetThoriumPlayer();
        foreach (Projectile projectile in Main.ActiveProjectiles) {
            if (projectile.type != ModContent.ProjectileType<SirenSeaCreature>()) {
                continue;
            }
            if (projectile.Opacity < 1f) {
                continue;
            }
            switch ((SirenSeaCreature.SeaCreatureType)projectile.ai[2]) {
                case SirenSeaCreature.SeaCreatureType.Red:
                    SetFade(thoriumPlayer2.GetEmpTimer<Damage>());
                    break;
                case SirenSeaCreature.SeaCreatureType.Orange:
                    SetFade(thoriumPlayer2.GetEmpTimer<AttackSpeed>());
                    break;
                case SirenSeaCreature.SeaCreatureType.Yellow:
                    SetFade(thoriumPlayer2.GetEmpTimer<CriticalStrikeChance>());
                    break;
                case SirenSeaCreature.SeaCreatureType.Green:
                    SetFade(thoriumPlayer2.GetEmpTimer<MovementSpeed>());
                    break;
                case SirenSeaCreature.SeaCreatureType.Blue:
                    SetFade(thoriumPlayer2.GetEmpTimer<DamageReduction>());
                    break;
                case SirenSeaCreature.SeaCreatureType.Purple:
                    SetFade(thoriumPlayer2.GetEmpTimer<ResourceRegen>());
                    break;
            }
        }
    }

    private static void SetFade(EmpowermentTimer timer) {
        //if (timer.level >= 2) {
        //    timer.fade = false;
        //}
    }

    public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool) {
        byte level = Convert.ToByte(1);
        foreach (Projectile projectile in Main.ActiveProjectiles) {
            if (projectile.type != ModContent.ProjectileType<SirenSeaCreature>()) {
                continue;
            }
            if (projectile.Opacity < 1f) {
                continue;
            }
            switch ((SirenSeaCreature.SeaCreatureType)projectile.ai[2]) {
                case SirenSeaCreature.SeaCreatureType.Red:
                    empPool.Add<Damage>(level);
                    break;
                case SirenSeaCreature.SeaCreatureType.Orange:
                    empPool.Add<AttackSpeed>(level);
                    break;
                case SirenSeaCreature.SeaCreatureType.Yellow:
                    empPool.Add<CriticalStrikeChance>(level);
                    break;
                case SirenSeaCreature.SeaCreatureType.Green:
                    empPool.Add<MovementSpeed>(level);
                    break;
                case SirenSeaCreature.SeaCreatureType.Blue:
                    empPool.Add<DamageReduction>(level);
                    break;
                case SirenSeaCreature.SeaCreatureType.Purple:
                    empPool.Add<ResourceRegen>(level);
                    break;
            }
        }
    }

    public override void ModifyEmpowerment(ThoriumPlayer player, ThoriumPlayer target, byte type, ref byte level, ref short duration) {
        duration = 300;
    }

    public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
}

[AutoloadEquip(EquipType.Body)]
public sealed class OldSirenChestplate : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SirenChestplate>();
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(30, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 20;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SirenChestplate.ApplyEquipEffects(player);
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class OldSirenLegs : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SirenLegs>();
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(22, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        SirenLegs.ApplyEquipEffects(player);
    }
}

