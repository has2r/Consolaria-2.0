using Microsoft.Xna.Framework;

using Terraria;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public sealed class MythicalWyvernProjectile : BaseKiteProjectile {
    protected override KiteInfo SetKiteInfo()
        => new() {
            SegmentsCount = 11,
            SegmentsCountToDraw = 11,
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

    protected override Color SetLineColor() {
        float f = (float)((double)Main.GlobalTimeWrappedHourly * 5.0 % 5.0);
        return Color.Lerp(new Color(194, 9, 9), new Color(249, 194, 16), (double)f <= 0.5 ? f / 0.5f : (float)(1.0 - ((double)f - 0.5) / 0.5));
    }
}

