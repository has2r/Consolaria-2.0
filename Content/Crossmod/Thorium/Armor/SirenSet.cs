using Consolaria.Content.Crossmod.Thorium.Projectiles;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium.Armor;

[AutoloadEquip(EquipType.Head)]
public sealed class SirenHelmet : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSirenHelmet>();
    }

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

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 5));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }


    public static void ApplyEquipEffects(Player player) {
        player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.08f;
        player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 4;
        player.GetThoriumPlayer().inspirationRegenBonus += 0.2f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
        => (body.type == ModContent.ItemType<SirenChestplate>() || body.type == ModContent.ItemType<OldSirenChestplate>())
        && (legs.type == ModContent.ItemType<SirenLegs>() || legs.type == ModContent.ItemType<OldSirenLegs>());

    public override void UpdateArmorSet(Player player) {
        ApplySirenSetBonus(player);
    }

    public void ApplySirenSetBonus(Player player) {
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
public sealed class SirenChestplate : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSirenChestplate>();
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(34, 22);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 6, silver: 40));

        int defense = 20;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
        player.GetThoriumPlayer().bardResourceDropBoost += 0.1f;
        player.GetThoriumPlayer().bardBuffDuration += 360;
    }
}

[AutoloadEquip(EquipType.Legs)]
public sealed class SirenLegs : ThoriumItem_BardBase {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSirenLegs>();

        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(30, 18);

        Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 4));

        int defense = 18;
        Item.defense = defense;
    }

    public override void UpdateEquip(Player player) {
        ApplyEquipEffects(player);
    }

    public static void ApplyEquipEffects(Player player) {
        player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
        player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 6;
        player.moveSpeed += 0.15f;
    }
}

