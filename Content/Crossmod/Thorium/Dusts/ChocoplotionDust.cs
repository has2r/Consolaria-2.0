using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

sealed class ChocoplotionDust : ModDust {
    public override void SetStaticDefaults() => UpdateType = DustID.Bone;
}
