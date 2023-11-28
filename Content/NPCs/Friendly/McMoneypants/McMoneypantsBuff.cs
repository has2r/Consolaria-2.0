using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Friendly.McMoneypants;

public class McMoneypantsBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.debuff[Type] = false;
        Main.pvpBuff[Type] = true;
        Main.buffNoSave[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
        => player.luck += 0.2f;
}
