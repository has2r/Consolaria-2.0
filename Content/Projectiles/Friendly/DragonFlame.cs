using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class DragonFlame : ModProjectile
    {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults() {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }

        public override void AI() {
            if (Math.Abs(Projectile.velocity.X) <= 0.15f && Math.Abs(Projectile.velocity.Y) <= 0.15f) Projectile.Kill();

            for (int dustCount = 0; dustCount < 3 ; ++dustCount) {
                Dust dust = Dust.NewDustDirect(Projectile.position, 12, 8, DustID.Shadowflame, 0f, -4.5f, 50, new Color(255, 255, 255, 200), Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = true;
            }

            Projectile.velocity *= 0.975f;
            Projectile.rotation = -Projectile.velocity.X * 0.05f;    
        }

        public override void Kill(int timeLeft) {
            for (int dustCount = 6; dustCount > 0; --dustCount) {
                int dust = Dust.NewDust(Projectile.position, 2, 2, DustID.Shadowflame, 0.0f, 0.0f, 50, new Color(255, 255, 255, 200), 2f);
                Main.dust[dust].noGravity = true;
                Vector2 velocity = Projectile.velocity;
                Main.dust[dust].velocity = velocity.RotatedBy(15 * (dustCount + 2), new Vector2());
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPvp(Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);
    }
}