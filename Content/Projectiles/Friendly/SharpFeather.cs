using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class SharpFeather : ModProjectile
    {       
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.HarpyFeather);

            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            AIType = ProjectileID.HarpyFeather;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1f;
        }

        public override void Kill(int timeLeft) {
            if (Projectile.owner == Main.myPlayer) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 1, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);    
                SoundEngine.PlaySound(0, Projectile.Center, 0);
            }
        }
    }
}