using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.Projectiles.Enemies {
    public class OcramLaser2 : ModProjectile {

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Ocram Laser");

            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.EyeLaser);
            AIType = ProjectileID.EyeLaser;

            Projectile.hostile = true;
            Projectile.tileCollide = false;

            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.timeLeft = 900;
            Projectile.penetrate = -1;

            Projectile.light = 0.1f;
        }

        public override void AI () {
            if (Projectile.timeLeft <= 895) Projectile.alpha = 50;
            Lighting.AddLight(Projectile.Center, 0.4f, 0.1f, 0.5f);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                if (Projectile.timeLeft < 890) {
                    Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Color.MediumVioletRed * ((float) (Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                    color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                    float rotation;
                    if (k + 1 >= Projectile.oldPos.Length) { rotation = (Projectile.position - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                    else { rotation = (Projectile.oldPos [k + 1] - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                    spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);

                }
            }
            return true;
        }

        public override void OnHitPlayer (Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override Color? GetAlpha (Color lightColor)
            => new Color(255, 255, 255, 200);
    }
}