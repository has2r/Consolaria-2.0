using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class TurkorKnife : ModProjectile {
		public override void SetStaticDefaults () {

			ProjectileID.Sets.TrailCacheLength [Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
		}

		public override void SetDefaults () {
			int width = 20; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.aiStyle = 2;
			Projectile.tileCollide = false;
		}

		private int acolor = 1;

		public override bool PreDraw (ref Color lightColor) {
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
				color = Color.Lerp(color, Color.Crimson, k / 12f);
				color.A = (byte) 2f;
				color.R = (byte) (color.R * (10 - k) / acolor);
				color.G = (byte) (color.G * (10 - k) / acolor);
				color.B = (byte) (color.B * (10 - k) / acolor);
				color.A = (byte) (color.A * (10 - k) / acolor);
				spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
			}
			return false;
		}
	}
}