using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public sealed class MythicalWyvernKite : BaseKiteItem {
    protected override int SetKiteProjectileType()
        => ModContent.ProjectileType<MythicalWyvernKiteProjectile>();
}
