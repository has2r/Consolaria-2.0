using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.Projectiles.Friendly {
    public class OcramEyeLaser : ModProjectile {

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Ocram Eye Laser");

            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.EyeLaser;

            Projectile.width = Projectile.height = 4;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;

            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.timeLeft = 900;
            Projectile.penetrate = -1;

            Projectile.light = 0.1f;
            Projectile.extraUpdates = 3;
        }

        public override void AI () {
            if (Main.rand.NextBool(8)) {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, 0, 0, 100, default, 1.1f);
                Main.dust [dust].velocity = -Projectile.velocity * 0.4f;
            }
            if (Projectile.timeLeft <= 890) Projectile.alpha = 50;
            Lighting.AddLight(Projectile.Center, 0.4f, 0.1f, 0.5f);
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                if (Projectile.timeLeft < 880) {
                    Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Color.MediumVioletRed * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                    color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                    float rotation;
                    if (k + 1 >= Projectile.oldPos.Length) { rotation = (Projectile.position - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                    else { rotation = (Projectile.oldPos [k + 1] - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                    spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);

                }
            }
            return true;
        }

        public override void Kill (int timeLeft) {
            for (int i = 0; i < 4; i++) {
                int num506 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
                Main.dust [num506].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
            }
        }

        public override Color? GetAlpha (Color lightColor)
            => new Color(255, 255, 255, 200);
    }
}