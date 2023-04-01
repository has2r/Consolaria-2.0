using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class VulcanBolt : ModProjectile {
        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 2;
        }

        public override void SetDefaults () {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
        }

        public override void OnHitNPC (NPC target, NPC.HitInfo hit, int damageDone)
            => target.AddBuff(BuffID.OnFire3, 300);

        public override void OnHitPlayer (Player target, Player.HurtInfo info) {
            if (info.PvP)
                target.AddBuff(BuffID.OnFire3, 300);
        }

        public override void AI () {
            if (Main.netMode != NetmodeID.Server) {
                if (Main.rand.NextBool(3)) {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 0, default, Main.rand.NextFloat(1f, 1.6f));
                    Main.dust [dust].fadeIn = Main.rand.NextFloat(0.5f, 1f);
                }
            }
        }

        public override void Kill (int timeLeft) {
            Player player = Main.player [Projectile.owner];
            if (Main.netMode != NetmodeID.Server) {
                int radius = 5;
                for (int x = -radius; x <= radius; x++) {
                    for (int y = -radius; y <= radius; y++) {
                        if (Math.Sqrt(x * x + y * y) <= radius + 0.5) {
                            int dust = Dust.NewDust(Projectile.position, 20, 20, DustID.Smoke, 0.0f, 0.0f, 120, default, Main.rand.NextFloat(0.5f, 1.5f));
                            int dust2 = Dust.NewDust(Projectile.position, 20, 20, DustID.Torch, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 100, default, Main.rand.NextFloat(1f, 2f));
                            Main.dust [dust2].noGravity = true;
                            Main.dust [dust].fadeIn = Main.rand.NextFloat(0.4f, 1f);
                            Main.dust [dust2].fadeIn = Main.rand.NextFloat(0.4f, 1f);
                        }
                    }
                }
                Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);
                Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);
                Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default, Main.rand.Next(61, 64), 1f);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VulcanBlast>(), (int) (Projectile.damage * 0.5f), Projectile.knockBack, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.75f, MaxInstances = 3 }, Projectile.Center);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 drawPos = Projectile.oldPos [k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(200 + k * 5, 150 - k * 10, 0, 50);
                spriteBatch.Draw(texture, drawPos, null, color * 0.45f, Projectile.oldRot [k] + (float) Math.PI / 2, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
                spriteBatch.Draw(texture, drawPos - Projectile.oldPos [k] * 0.5f + Projectile.oldPos [k + 1] * 0.5f, null, color * 0.45f, Projectile.oldRot [k] * 0.5f + Projectile.oldRot [k + 1] * 0.5f + (float) Math.PI / 2, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
            }
            return true;
        }
    }
}