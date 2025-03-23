using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class EternalLaser : ModProjectile {
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.PurpleLaser;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.timeLeft = 900;
            Projectile.penetrate = 1;

            Projectile.light = 0.1f;
        }

        public override void AI() {
            if (Projectile.timeLeft <= 870) Projectile.alpha = 0;
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(8))
                    Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, -Projectile.velocity.X * 0.4f, -Projectile.velocity.Y * 0.4f, 120, default, 0.6f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
            => Color.White;
    }
}