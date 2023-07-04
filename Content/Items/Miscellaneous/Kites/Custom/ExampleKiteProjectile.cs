using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public sealed class ExampleKiteProjectile : BaseKiteProjectile {
    protected override KiteInfo KiteInfo()
        => new() {
            SegmentsCount = 12,
            SegmentsCountToDraw = 12,
            LengthBetweenBodySegments = 22f,
            BodyXPositionOffset = -12,
            HeadYPositionOffset = -6,
            LengthBetweenTailSegments = 10,
            TailLength = 8,
            WindResistance = 12
        };

    protected override float SetSegmentRotation()
        => Projectile.spriteDirection == 1 ? MathHelper.Pi / 2f : -MathHelper.Pi / 2f;

    protected override float SetHeadRotation()
        => MathHelper.Pi / 8f * (float)Projectile.spriteDirection;
}

