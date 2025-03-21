using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpectralArrow : ModProjectile {

        private int hitCounter;

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 5;

            Projectile.alpha = byte.MaxValue;
            Projectile.light = 0.1f;

            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.timeLeft = 120;
        }

        public override void AI ()
            => Projectile.velocity *= 0.975f;

        public override void OnHitNPC (NPC target, NPC.HitInfo hit, int damageDone)
            => hitCounter++;

        public override void ModifyHitNPC (NPC target, ref NPC.HitModifiers modifiers)
            => modifiers.FinalDamage -= (int) (modifiers.FinalDamage.Flat * hitCounter * 0.1f);

        public override void OnKill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < 7; k++) {
                    int dust1 = Dust.NewDust(Projectile.position, 8, 16, DustID.DungeonSpirit, 0, 0, 100, default, 1.4f);
                    Main.dust [dust1].noGravity = true;
                    Main.dust [dust1].fadeIn = 1f;
                    Main.dust [dust1].velocity = Main.dust [dust1].velocity * 0.6f + Projectile.oldVelocity * 1.5f;
                }
            }
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Pitch = 0.2f, Volume = 0.5f }, Projectile.Center);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override Color? GetAlpha (Color lightColor)
            => new Color?(Color.White * 0.4f);

        public override bool? CanCutTiles ()
            => false;
    }
}