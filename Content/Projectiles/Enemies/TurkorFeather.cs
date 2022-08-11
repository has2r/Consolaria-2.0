using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.Projectiles.Enemies
{
	public class TurkorFeather : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Turkor's Feather");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults() {
			int width = 20; int height = 40;
			Projectile.Size = new Vector2(width, height);

			Projectile.friendly = false;
			Projectile.hostile = true;

			Projectile.aiStyle = 2;
			Projectile.timeLeft = 180;
			Projectile.tileCollide = false;
		}

		public override void AI() {
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1.57f;
			if (Projectile.aiStyle == -1) {
				float scaleFactor3 = 18f;
				int num203 = Player.FindClosest(Projectile.Center, 1, 1);

				Vector2 vector20 = Main.player[num203].Center - Projectile.Center;
				vector20.Normalize();
				vector20 *= scaleFactor3;
				int num204 = 70;
				Projectile.velocity = (Projectile.velocity * (num204 - 1) + vector20) / num204;
				if (Projectile.velocity.Length() < 14f) {
					Projectile.velocity.Normalize();
					Projectile.velocity *= 18f;
				}
			}
		}

		public override void Kill(int timeLeft) {
			if (Projectile.owner == Main.myPlayer) {
				for (int k = 0; k < 5; k++)
					Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 1, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				color = Color.BlueViolet * 0.12f;
				float rotation;
				if (k + 1 >= Projectile.oldPos.Length) { rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2; }
				else { rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2; }
				spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, effects, 0f);
			}
			return true;
		}
	}
}