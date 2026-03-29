using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Consolaria.Content.Crossmod.Thorium.Projectiles;

public sealed class SeraphimHeal : ThoriumProjectile_HealerBase {
    public override string Texture => "Consolaria/Assets/Textures/Empty";

    public ref float HealTime => ref Projectile.localAI[0];
    public ref float AICounter_ForDusts => ref Projectile.localAI[1];

    public override void SetHealerDefaults() {
        Projectile.SetSizeValues(10);

        Projectile.netImportant = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
    }

    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    public override bool? CanCutTiles() => false;

    public override void AI() {
        Player owner = Projectile.GetOwnerAsPlayer();
        if (!owner.IsAlive() || owner.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimEffectOnCooldown) {
            Projectile.Kill();
        }

        Projectile.Center = owner.GetPlayerCorePoint();

        float healTime = 15f;
        if (++HealTime > healTime) {
            int radius = 75;
            Projectile.ThoriumHeal(1, radius, onHealEffects: true, bonusHealing: true, delegate {
                SoundEngine.PlaySound(in SoundID.Item85, Projectile.position);
            }, null, -1, ignoreHealer: false);

            HealTime = 0f;
        }

        AICounter_ForDusts++;

        Projectile projectile = Projectile;
        int num4 = 150;
        Vector2 vector2 = new Vector2(projectile.Top.X, projectile.position.Y + (float)num4);
        for (int j = 0; j < 4; j++) {
            if (Main.rand.NextBool()) {
                continue;
            }
            Vector2 vector3 = Main.rand.NextVector2Unit();
            //if (!(Math.Abs(vector3.X) < 0.12f))
            {
                Vector2 targetPosition = projectile.Center + vector3 * new Vector2((projectile.height - num4) / 2) * 0.75f;
                Dust dust = Dust.NewDustDirect(targetPosition, 0, 0, DustID.TintableDustLighted, 0f, 0f, 100, newColor:
                    Color.Lerp(ThoriumPlayer_Consolaria.SERAPHIMGLOWCOLOR, Color.Lerp(Color.White, Color.LightYellow, Main.rand.NextFloat(0.5f, 1f)), 0.5f) with { A = 100 });
                dust.position = targetPosition;
                dust.velocity = (vector2 - dust.position).SafeNormalize(Vector2.Zero);
                dust.velocity += dust.position.DirectionTo(projectile.Center).RotatedBy(-owner.direction * 0.5f) * Main.rand.NextFloat(2.5f, 5f) * 0.125f;
                //dust.velocity += dust.position.DirectionTo(projectile.Center).RotatedBy(0f) * Main.rand.NextFloat(2.5f, 5f) * 0.5f;
                dust.scale = 0.7f + 0.7f * Main.rand.NextFloatDirection();
                dust.scale *= Main.rand.NextFloat(1f, 1.5f);
                dust.fadeIn = 1f;
                dust.noGravity = true;
                dust.customData = projectile;
                dust.velocity.X += Main.rand.NextFloat() * 3.5f * (AICounter_ForDusts % 2 == 0).ToDirectionInt();
                dust.velocity += owner.velocity;
            }
        }
    }
}
