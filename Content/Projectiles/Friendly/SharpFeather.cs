using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SharpFeather : ModProjectile {
        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.HarpyFeather);

            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            AIType = ProjectileID.HarpyFeather;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1f;
        }

        public override void OnKill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < 5; k++)
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Demonite, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                color = Color.BlueViolet * 0.12f;
                float rotation;
                if (k + 1 >= Projectile.oldPos.Length) { rotation = (Projectile.position - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                else { rotation = (Projectile.oldPos [k + 1] - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
            }
            return true;
        }
    }
}