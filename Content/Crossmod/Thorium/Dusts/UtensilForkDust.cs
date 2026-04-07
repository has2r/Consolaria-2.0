using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

sealed class UtensilForkDust : ModDust {
    public override void SetStaticDefaults() => UpdateType = DustID.Silver;
}
