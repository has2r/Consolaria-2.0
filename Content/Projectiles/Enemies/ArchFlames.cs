using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class ArchFlames : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults () {
            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.extraUpdates = 2;
        }

        public override void AI () {
            if (Projectile.timeLeft > 45)
                Projectile.timeLeft = 45;
            if (Projectile.ai [0] > 7.0) {
                float num = 1f;
                if (Projectile.ai [0] == 8.0)
                    num = 0.25f;
                else if (Projectile.ai [0] == 9.0)
                    num = 0.5f;
                else if (Projectile.ai [0] == 10.0)
                    num = 0.75f;
                ++Projectile.ai [0];
                int Type = 66;
                if (Main.rand.NextBool(1)) {
                    for (int index1 = 0; index1 < 2; ++index1) {
                        int num3 = 4;
                        int index2 = Dust.NewDust(new Vector2(Projectile.position.X + num3, Projectile.position.Y + num3), Projectile.width - num3 * 3, Projectile.height - num3 * 3, DustID.FlameBurst, 0.0f, 0.0f, 100, new Color(), 1.2f);
                        if (Type == 66 && Main.rand.NextBool(3)) {
                            Main.dust [index2].noGravity = true;
                            Main.dust [index2].scale *= 2f;
                            Dust dust1 = Main.dust [index2];
                            dust1.velocity.X = dust1.velocity.X * 2f;
                            Dust dust2 = Main.dust [index2];
                            dust2.velocity.Y = dust2.velocity.Y * 2f;
                        } else {
                            Main.dust [index2].noGravity = true;
                            Main.dust [index2].scale *= 1;
                        }
                        Dust dust3 = Main.dust [index2];
                        dust3.velocity.X = dust3.velocity.X * 1.2f;
                        Dust dust4 = Main.dust [index2];
                        dust4.velocity.Y = dust4.velocity.Y * 1.2f;
                        Main.dust [index2].scale *= num;
                        if (Type == 66)
                            Main.dust [index2].velocity += Projectile.velocity;
                    }
                }
            } else
                ++Projectile.ai [0];
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo info)
            => target.AddBuff(BuffID.OnFire, 180);
    }
}