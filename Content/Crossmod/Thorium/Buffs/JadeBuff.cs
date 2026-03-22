using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class JadeBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoTimeDisplay[Type] = true;

        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        player.statDefense += 10;
        player.GetDamage(DamageClass.Generic) += 0.2f;
    }
}
