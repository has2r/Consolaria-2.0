using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using Terraria;

namespace Consolaria.Content.Crossmod.Thorium.Projectiles;

public sealed class SirenSeaCreature : ThoriumProjectile_BardBase {
    public override void SetStaticDefaults() {
        Projectile.SetFrameCount(6);
    }

    public override void SetBardDefaults() {
        Projectile.SetSizeValues(10);

        Projectile.friendly = true;
        Projectile.tileCollide = false;
    }

    public override bool? CanCutTiles() => false;
    public override bool? CanDamage() => false;

    public override void AI() {
        if (Projectile.localAI[0] == 0f) {
            Projectile.localAI[0] = 1f;

        }
        Projectile.frame = (int)Projectile.ai[2];

        Player closestPlayer = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
        Projectile.direction = Projectile.spriteDirection = ((Projectile.Center.X - closestPlayer.Center.X) > 0).ToDirectionInt();

        Player owner = Main.player[Projectile.owner];

        Projectile.rotation = Projectile.velocity.Y * 0.1f;

        Vector2 targetPosition = owner.Center - new Vector2(0, 80);
        float distance = Projectile.Center.DistanceSQ(targetPosition);
        bool far = distance > 700 * 700;
        (float mod, float factor, float fade) = far ? (9, 0.25f, 0) : (6, 0.15f, 1);
        if (distance < 40 * 40) {
            Projectile.velocity.Y *= 0.97f;
        }
        else {
            Projectile.velocity.Y = Vector2.SmoothStep(Projectile.velocity, (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * mod, factor).Y;
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        Projectile.QuickDrawAnimated(Color.White * Projectile.Opacity);

        return false;
    }
}
