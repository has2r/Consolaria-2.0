using Consolaria.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {

    public class HeartArrow : ModProjectile {

        public override void SetDefaults() {
            int width = 5; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (!target.buffImmune[BuffID.Confused] && Main.rand.Next(2) == 0)
                target.AddBuff(ModContent.BuffType<Stunned>(), 90);
        }

        public override void Kill(int timeLeft) {
            if (Projectile.owner == Main.myPlayer) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f, 100, default, 1f);   
                SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            }
        }
    }
}
