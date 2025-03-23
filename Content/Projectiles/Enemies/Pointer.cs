using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class Pointer : ModProjectile {
        float num2 = 3f;
        public override string Texture => "Consolaria/Assets/Textures/Empty";
        public override string Name => "";

        public override void SetDefaults() {
            int width = 10; int height = 20;
            Projectile.Size = new Vector2(width, height);

            Projectile.hostile = false;
            Projectile.friendly = false;

            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            if (Main.netMode != NetmodeID.Server) {
                int num;
                for (int num164 = 0; num164 < 1; num164 = num + 1) {
                    float x2 = Projectile.position.X - Projectile.velocity.X / 10f * num164;
                    float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * num164;
                    int num165 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.Smoke, 0f, 0f, 60, Color.White, 1f);
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)) * num2;
                    Main.dust[num165].position.X = x2 + offset.X;
                    Main.dust[num165].position.Y = y2 + offset.Y;
                    Dust dust3 = Main.dust[num165];
                    dust3.velocity = Projectile.velocity * 0.5f;
                    Main.dust[num165].noGravity = true;
                    Main.dust[num165].scale = num2;
                    num = num164;
                    num2 *= 0.96f;
                }
            }
        }
    }
}