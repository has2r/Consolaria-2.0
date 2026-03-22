using Consolaria.Content.Crossmod.Thorium.Weapons;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class SingerTurkeyBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoTimeDisplay[Type] = true;

        BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        player.GetModPlayer<SingerTurkeyBuff_Handler>().IsEffectActive = true;

        int type = ModContent.ProjectileType<SingerTurkey.SingerTurkey_Summon>();
        if (player.ownedProjectileCounts[type] < 1) {
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

    public override void ResetEffects() {
        IsEffectActive = false;
    }
}
