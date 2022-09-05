using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class DreadSkull : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 5;
            ProjectileID.Sets.CultistIsResistantTo [Projectile.type] = true;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(585);

            int width = 22; int height = 24;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;

            AIType = 14;
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.scale = 1f;
            Projectile.light = 0.1f;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];

            if (++Projectile.frameCounter >= 8) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames [Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.velocity.Y += Projectile.ai [0];
            var vector = Projectile.velocity * 1.015f;
            Projectile.velocity = vector;
            Projectile.spriteDirection = player.direction;

            if (Main.netMode != NetmodeID.Server) {
                for (int _dustCount = 0; _dustCount < 10; _dustCount++) {
                    int _dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
                    Main.dust [_dust].noGravity = true;
                    Main.dust [_dust].velocity = Projectile.Center - Main.dust [_dust].position;
                    Main.dust [_dust].velocity.Normalize();
                    Main.dust [_dust].velocity *= -5f;
                    Main.dust [_dust].velocity += Projectile.velocity / 2f;
                }
            }

            float posX = Projectile.Center.X;
            float posY = Projectile.Center.Y;
            float maxDetectRadius = 600f;
            bool flag = false;
            for (int _npc = 0; _npc < Main.maxNPCs; ++_npc) {
                if (Main.npc [_npc].CanBeChasedBy(Projectile, false) && Projectile.Distance(Main.npc [_npc].Center) < maxDetectRadius && Collision.CanHit(Projectile.Center, 1, 1, Main.npc [_npc].Center, 1, 1)) {
                    float npsPosX = Main.npc [_npc].position.X + (Main.npc [_npc].width / 2);
                    float npsPosY = Main.npc [_npc].position.Y + (Main.npc [_npc].height / 2);
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
            Vector2 vector2 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
            float posX2 = posX - vector2.X;
            float posY2 = posY - vector2.Y;
            float vel = (float) Math.Sqrt(posX2 * (double) posX2 + posY2 * posY2);
            float vel2 = homingSpeed / vel;
            float velX = posX2 * vel2;
            float velY = posY2 * vel2;
            Projectile.velocity.X = (float) ((Projectile.velocity.X * 20.0 + velX) / 21.0);
            Projectile.velocity.Y = (float) ((Projectile.velocity.Y * 20.0 + velY) / 21.0);

            Lighting.AddLight(Projectile.Center, 0.5f, 0.4f, 0.9f);
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPvp (Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f, 100, default, 1.1f);
            }
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
        }

        public override Color? GetAlpha (Color lightColor)
           => new Color(255, 255, 255, 200);
    }
}