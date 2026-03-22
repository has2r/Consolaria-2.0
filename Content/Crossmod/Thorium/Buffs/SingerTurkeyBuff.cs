using Consolaria.Content.Crossmod.Thorium.Weapons;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod.Items;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class SingerTurkeyBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        player.GetModPlayer<SingerTurkeyBuff_Handler>().IsEffectActive = true;

        int type = ModContent.ProjectileType<SingerTurkey.SingerTurkey_Summon>();
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
                Projectile.NewProjectileDirect(player.GetSource_Buff(buffIndex),
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

public sealed class SingerTurkeyBuff_Handler : ModPlayer {
    public bool IsEffectActive;
    public byte BardAttackCount;

    public override void ResetEffects() {
        IsEffectActive = false;
    }

    public override void PostUpdateEquips() {
        if (!IsEffectActive) {
            BardAttackCount = 0;
            return;
        }

        Item selectedItem = Player.HeldItem;
        if (Player.whoAmI == Main.myPlayer && !selectedItem.IsAir && selectedItem.ModItem is BardItem && Player.ItemAnimationJustStarted) {
            BardAttackCount++;
        }
    }
}
