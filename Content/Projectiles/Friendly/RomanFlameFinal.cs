using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class RomanFlameFinal : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";
        private readonly ushort dustType = (ushort)(ModContent.DustType<Dusts.RomanFlame>());
        private readonly Color colorType = Main.DiscoColor;

        public override void SetDefaults () {
            int width = 4; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 45;

            Projectile.tileCollide = true;
        }

        public override void AI () {
            if (Main.netMode != NetmodeID.Server) {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 100, colorType, 1.5f);
                Main.dust [dust].noGravity = true;
            }
        }

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                Vector2 position = new Vector2(12f, 12f);
                for (int i = 0; i < 20; i++) {
                    int _dust = Dust.NewDust(Projectile.Center - position / 2f, (int) position.X, (int) position.Y, dustType, 0f, 0f, 100, colorType, 0.8f);
                    Main.dust [_dust].noGravity = true;
                    Main.dust [_dust].velocity *= 2.5f;
                }
            }
        }
    }
}
