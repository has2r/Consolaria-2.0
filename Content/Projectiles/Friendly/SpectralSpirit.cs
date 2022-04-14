using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class SpectralSpirit : ModProjectile
    {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults() {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI() {
            for (int dustCount = 0; dustCount < 3; ++dustCount) {
                float dustVelX = Projectile.velocity.X / 4f * dustCount;
                float dustVelY = Projectile.velocity.Y / 4f * dustCount;

                int dust_ = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 0, default, 1.25f);
                Main.dust[dust_].position.X = Projectile.Center.X - dustVelX;
                Main.dust[dust_].position.Y = Projectile.Center.Y - dustVelY;
                Main.dust[dust_].velocity *= 0.0f;
                Main.dust[dust_].noGravity = true;
                Main.dust[dust_].noLight = true;

                if (Main.rand.Next(2) == 0) {
                    int dust2_ = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, new Color(255, 255, 255, 200), 1.1f);
                    Main.dust[dust2_].position.X = Projectile.Center.X - dustVelX;
                    Main.dust[dust2_].position.Y = Projectile.Center.Y - dustVelY;
                    Main.dust[dust2_].velocity *= 0.0f;
                    Main.dust[dust2_].noGravity = true;
                }
            }

            float posX = Projectile.Center.X;
            float posY = Projectile.Center.Y;
            float maxDetectRadius = 800f;
            bool flag = false;
            for (int _npc = 0; _npc < Main.maxNPCs; ++_npc) {
                if (Main.npc[_npc].CanBeChasedBy(Projectile, false) && Projectile.Distance(Main.npc[_npc].Center) < maxDetectRadius && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[_npc].Center, 1, 1)) {
                    float npsPosX = Main.npc[_npc].position.X + (Main.npc[_npc].width / 2);
                    float npsPosY = Main.npc[_npc].position.Y + (Main.npc[_npc].height / 2);
                    float dist = Math.Abs(Projectile.position.X + (Projectile.width / 2) - npsPosX) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - npsPosY);
                    if (dist < maxDetectRadius) {
                        maxDetectRadius = dist;
                        posX = npsPosX;
                        posY = npsPosY;
                        flag = true;
                    }
                }
            }
            if (!flag) return;
            float homingSpeed = 12f;
            Vector2 vector2 = new(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
            float posX2 = posX - vector2.X;
            float posY2 = posY - vector2.Y;
            float vel = (float)Math.Sqrt(posX2 * (double)posX2 + posY2 * posY2);
            float vel2 = homingSpeed / vel;
            float velX = posX2 * vel2;
            float velY = posY2 * vel2;
            Projectile.velocity.X = (float)((Projectile.velocity.X * 20.0 + velX) / 21.0);
            Projectile.velocity.Y = (float)((Projectile.velocity.Y * 20.0 + velY) / 21.0);

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1f;
        }

        public override void Kill(int timeLeft) {
            for (int dustCount = 12; dustCount > 0; --dustCount) {
                Vector2 velocity = Projectile.velocity;
                int dust = Dust.NewDust(Projectile.position, 2, 2, DustID.Shadowflame, 0.0f, 0.0f, 100, new Color(255, 255, 255, 200), 1.5f);
                Main.dust[dust].velocity = velocity.RotatedBy(15 * (dustCount + 2), new Vector2());
                Main.dust[dust].noGravity = true;
                if (Main.rand.Next(2) == 0) {
                    int dust2 = Dust.NewDust(Projectile.position, 2, 2, DustID.GoldFlame, 0.0f, 0.0f, 0, default, 2f);
                    Main.dust[dust2].velocity = velocity.RotatedBy(15 * (dustCount + 2), new Vector2());
                    Main.dust[dust2].noGravity = true;
                }
            }
        }
    }
}
