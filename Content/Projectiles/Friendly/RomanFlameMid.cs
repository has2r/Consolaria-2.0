using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class RomanFlameMid : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";
        private readonly ushort dustType = (ushort) (ModContent.DustType<Dusts.RomanFlame>());
        private readonly Color colorType = Main.DiscoColor;

        public override void SetDefaults () {
            int width = 5; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;

            Projectile.tileCollide = true;
        }


        public override void AI () {
            if (Main.netMode != NetmodeID.Server) {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 100, colorType, 1.5f);
                Main.dust [dust].noGravity = true;
            }
        }

        public override void OnKill (int timeLeft) {
            Player player = Main.player [Projectile.owner];
            float projectilesCount = Main.rand.Next(3, 5);
            Vector2 velocity = Projectile.velocity;
            for (int i = 0; i < projectilesCount; i++) {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(180));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, perturbedSpeed, ModContent.ProjectileType<RomanFlameFinal>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            }
            if (Main.netMode != NetmodeID.Server) {
                Vector2 position = new Vector2(16f, 16f);
                for (int i = 0; i < 20; i++) {
                    int _dust = Dust.NewDust(Projectile.Center - position / 2f, (int) position.X, (int) position.Y, dustType, 0f, 0f, 100, colorType, 1f);
                    Main.dust [_dust].noGravity = true;
                    Main.dust [_dust].velocity *= 2.5f;
                    _dust = Dust.NewDust(Projectile.Center - position / 2f, (int) position.X, (int) position.Y, dustType, 0f, 0f, 100, colorType, 1.2f);
                    Main.dust [_dust].velocity *= 1.5f;
                    Main.dust [_dust].noGravity = true;
                }
            }
            if (Projectile.soundDelay == 0) {
                Projectile.soundDelay = 100;
                SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.8f }, Projectile.Center);
            }
        }
    }
}