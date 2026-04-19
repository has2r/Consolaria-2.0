using Terraria;
using Terraria.ID;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class SeraphimBuff : ThoriumBuff_Base {
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }
}
