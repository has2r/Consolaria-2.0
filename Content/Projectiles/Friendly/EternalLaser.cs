using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class EternalLaser : ModProjectile {
        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.EyeLaser;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.scale = 0.95f;
            Projectile.alpha = 255;

            Projectile.timeLeft = 900;
            Projectile.penetrate = 1;
        }

        public override void AI () {
            if (Projectile.timeLeft <= 895) Projectile.alpha = 0;
        }
    }
}