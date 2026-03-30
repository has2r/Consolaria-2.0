using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using Terraria;

namespace Consolaria.Content.Crossmod.Thorium.Projectiles;

public sealed class SirenSeaCreature : ThoriumProjectile_BardBase {
    private static ushort TIMELEFT => Helper.SecondsToFrames(15);

    public enum SeaCreatureType : byte {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple
    }

    public override void SetStaticDefaults() {
        Projectile.SetFrameCount(6);
    }

    public override void SetBardDefaults() {
        Projectile.SetSizeValues(10);

        Projectile.friendly = true;
        Projectile.tileCollide = false;

        Projectile.Opacity = 0f;

        Projectile.timeLeft = TIMELEFT;
    }

    public override bool? CanCutTiles() => false;
    public override bool? CanDamage() => false;

    public override void AI() {
        if (Projectile.localAI[0] == 0f) {
            Projectile.localAI[0] = 1f;

            Projectile.localAI[2] = Main.rand.NextFloat(-0.25f, 0f);
        }
        Projectile.frame = (byte)(SeaCreatureType)Projectile.ai[2];

        float lerpValue = 0.01f;
        if (Projectile.localAI[2] >= 0f) {
            float to = 1f;
            to *= Utils.GetLerpValue(0, 150, Projectile.timeLeft, true);
            Projectile.Opacity = Helper.Approach(Projectile.Opacity, to, lerpValue);
            Main.NewText(Projectile.Opacity);
            if (Projectile.Opacity >= 1f) {
                Projectile.localAI[1] = Helper.Approach(Projectile.localAI[1], 1f, 0.025f);
            }
        }
        Projectile.localAI[2] = Helper.Approach(Projectile.localAI[2], 0f, lerpValue);

        Player closestPlayer = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
        Projectile.direction = Projectile.spriteDirection = ((Projectile.Center.X - closestPlayer.Center.X) > 0).ToDirectionInt();

        Player owner = Main.player[Projectile.owner];

        Projectile.rotation = Projectile.velocity.Y * 0.1f + MathHelper.TwoPi * Ease.CubeInOut(Projectile.localAI[1]) * -Projectile.direction;

        Vector2 targetPosition = owner.Center - new Vector2(0, 100f);
        Vector2 targetPosition2 = closestPlayer.Center + new Vector2(Projectile.ai[0], 0f);
        float distance = Projectile.Center.Distance(targetPosition);
        bool far = distance > 700;
        (float mod, float factor, float fade) = far ? (9, 0.25f, 0) : (6, 0.15f, 1);
        if (distance < 40) {
            Projectile.velocity.Y *= 0.97f;
        }
        else {
            Projectile.velocity.Y = Vector2.SmoothStep(Projectile.velocity, (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * mod, factor).Y;
            Projectile.velocity.X = Vector2.SmoothStep(Projectile.velocity, (targetPosition2 - Projectile.Center).SafeNormalize(Vector2.Zero) * mod, factor * 0.5f).X;
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        Projectile.QuickDrawAnimated(lightColor * 4f * Projectile.Opacity);

        return false;
    }
}
