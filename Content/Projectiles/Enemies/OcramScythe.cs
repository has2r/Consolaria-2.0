using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Consolaria.Content.Projectiles.Enemies {
	public class OcramScythe : ModProjectile {
		private float rotationTimer = (float) Math.PI;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Supreme Demon Scythe");

			ProjectileID.Sets.TrailCacheLength [Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode [Projectile.type] = 0;
		}

		public override void SetDefaults () {
			int width = 60; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.penetrate = 1;
			Projectile.tileCollide = false;

			Projectile.light = 0.25f;
			Projectile.scale = 0.6f;
		}

		public override void AI () {
			while (Projectile.scale < 1)
				Projectile.scale += 0.1f;

			if (Projectile.timeLeft > 200)
				Projectile.velocity *= 1.05f;

			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
			Main.dust [dust2].noGravity = true;

			Projectile.rotation += 2 / rotationTimer;
			rotationTimer += 0.01f;
		}

		public override void Kill (int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 30; i++) {
				int num506 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
				Main.dust [num506].noGravity = true;
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
			}
		}

		public override bool PreDraw (ref Color lightColor) {
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos [k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.MediumVioletRed * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
				//float rotation;
				//if (k + 1 >= Projectile.oldPos.Length) { rotation = (Projectile.position - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
				//else { rotation = (Projectile.oldPos [k + 1] - Projectile.oldPos [k]).ToRotation() + MathHelper.PiOver2; }
				spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length, effects, 0f);
			}
			return true;
		}

		public override void OnHitPlayer (Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.ShadowFlame, 180);

		public override Color? GetAlpha (Color lightColor)
			=> new Color(255, 255, 255, 200);
	}
}