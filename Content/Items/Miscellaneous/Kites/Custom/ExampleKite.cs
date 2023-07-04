using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public sealed class ExampleKite : BaseKite {
    protected override int SetKiteProjectileType()
        => ModContent.ProjectileType<ExampleKiteProjectile>();
}
