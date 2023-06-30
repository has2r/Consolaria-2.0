using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class ArchFlames : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.Flames);

            int width = 12; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;

            Projectile.tileCollide = false;
        }

        public override void AI () => Projectile.velocity *= 0.95f;

        public override void OnHitPlayer (Player target, Player.HurtInfo info)
            => target.AddBuff(BuffID.OnFire, 180);
    }
}