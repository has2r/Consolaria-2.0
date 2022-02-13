using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class ShadowFlames : ModProjectile
    {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private bool spawnDust;

        public override void SetDefaults() {
            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.penetrate = 3;
            Projectile.timeLeft = 60;

            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }

        public override void AI() {
            if (Projectile.timeLeft > 45)
                Projectile.timeLeft = 45;

            if (Projectile.ai[0] > 7.0) {
                ++Projectile.ai[0];
                float scale = 1f;

                if (Projectile.ai[0] == 8.0) scale = 0.25f;
                else if (Projectile.ai[0] == 9.0) scale = 0.5f;
                else if (Projectile.ai[0] == 10.0) scale = 0.75f;

                spawnDust = true;
                if (Main.rand.Next(1) == 0) {
                    for (int index1 = 0; index1 < 2; ++index1) {
                        int _pos = 4;
                        int _dust = Dust.NewDust(new Vector2(Projectile.position.X + (float)_pos, Projectile.position.Y + (float)_pos), Projectile.width - _pos * 3, Projectile.height - _pos * 3, DustID.Shadowflame , 0.0f, 0.0f, 125, default, 0.95f);
                        if (spawnDust && Main.rand.Next(3) == 0) {
                            Main.dust[_dust].noGravity = true;
                            Main.dust[_dust].scale *= 2f;
                            Dust dust1 = Main.dust[_dust];
                            dust1.velocity.X = dust1.velocity.X * 2f;
                            Dust dust2 = Main.dust[_dust];
                            dust2.velocity.Y = dust2.velocity.Y * 2f;
                        }
                        else {
                            Main.dust[_dust].noGravity = true;
                            Main.dust[_dust].scale *= 1;
                        }
                        Dust dust3 = Main.dust[_dust];
                        dust3.velocity.X = dust3.velocity.X * 1.2f;
                        Dust dust4 = Main.dust[_dust];
                        dust4.velocity.Y = dust4.velocity.Y * 1.2f;
                        Main.dust[_dust].scale *= scale;
                        if (spawnDust) Main.dust[_dust].velocity += Projectile.velocity;
                    }
                }
            }
            else Projectile.ai[0]++;
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPvp(Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);
    }
}