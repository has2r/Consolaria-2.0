using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class SpicyExplosion : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        
        public override void SetDefaults()  {
            int width = 90; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.tileCollide = false;

            Projectile.timeLeft = 420;
            Projectile.penetrate = -1;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
             
        public override void AI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.Kill();
        }
  
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.OnFire, 180);

        public override void OnHitPvp(Player target, int damage, bool crit)
           => target.AddBuff(BuffID.OnFire, 180);

        public override void Kill(int timeLeft)
            => SoundEngine.PlaySound(0, Projectile.Center, 0);
    } 
}