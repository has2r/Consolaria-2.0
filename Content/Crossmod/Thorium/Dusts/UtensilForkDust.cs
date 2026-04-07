using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
sealed class UtensilForkDust : ModDust {
    public override void SetStaticDefaults() => UpdateType = DustID.Silver;
}
