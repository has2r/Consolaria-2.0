using Consolaria.Content.Crossmod.Thorium.Accessories;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class SingerTurkeyBuff : ThoriumBuff_Base {
    public override void SetStaticDefaults() {
        Main.buffNoTimeDisplay[Type] = true;

        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        SummonTurkeyHead(player, player.GetSource_Buff(buffIndex));
    }

    public static void SummonTurkeyHead(Player player, IEntitySource source) {
        player.GetModPlayer<SingerTurkeyBuff_Handler>().IsEffectActive = true;

        int type = ModContent.ProjectileType<PortableSpecialCorn.PortableSpecialCorn_Summon>();
        int count = player.ownedProjectileCounts[type];
        if (count >= 2) {
            foreach (Projectile projectile in Main.ActiveProjectiles) {
                if (projectile.owner == player.whoAmI && projectile.type == type) {
                    projectile.Kill();
                }
            }
            count = 0;
        }
        if (count < 1) {
            if (player.whoAmI == Main.myPlayer) {
                Projectile.NewProjectileDirect(source,
                                               player.Center,
                                               Vector2.Zero,
                                               type,
                                               0,
                                               0,
                                               player.whoAmI);
            }
        }
    }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public sealed class SingerTurkeyBuff_Handler : ModPlayer {
    public bool IsEffectActive;
    public byte BardAttackCount;

    public override void ResetEffects() {
        IsEffectActive = false;
    }

    public static bool JustUsedBardWeapon(Player player) {
        Item selectedItem = player.HeldItem;
        return !selectedItem.IsAir && selectedItem.ModItem is BardItem && player.ItemAnimationJustStarted;
    }

    public override void PostUpdateEquips() {
        if (!IsEffectActive) {
            BardAttackCount = 0;
            return;
        }

        if (Player.whoAmI == Main.myPlayer && JustUsedBardWeapon(Player)) {
            BardAttackCount++;
        }
    }
}
