using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public sealed class MythicalWyvern : BaseKiteItem {
    protected override int SetKiteProjectileType()
        => ModContent.ProjectileType<MythicalWyvernProjectile>();
}
