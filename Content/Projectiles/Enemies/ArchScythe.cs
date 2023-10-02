using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
	public class ArchScythe : ModProjectile {
		private float rotationTimer = (float)Math.PI;

		public override void SetDefaults () {
			int width = 48; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.penetrate = 1;
			Projectile.tileCollide = true;

			Projectile.light = 0.25f;
			Projectile.scale = 0.9f;
		}

		public override void AI () {
			if (Projectile.timeLeft > 200)
				Projectile.velocity *= 1.05f;

			if (Projectile.timeLeft == 299) {
				if (Main.netMode != NetmodeID.Server) {
					float dustCount = 32f;
					int vel = 0;
					while (vel < dustCount) {
						Vector2 vector = Vector2.UnitX * 0f;
						vector += -Vector2.UnitY.RotatedBy(vel * (8f / dustCount), default) * new Vector2(18f, 18f);
						vector = vector.RotatedBy(Projectile.velocity.ToRotation(), default);
						int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, 0f, 0f, 0, default, 1f);
						Main.dust [dust].noGravity = true;
						Main.dust [dust].position = new Vector2(Projectile.Center.X, Projectile.Center.Y) + vector;
						Main.dust [dust].velocity = Projectile.velocity * 0f + vector.SafeNormalize(Vector2.UnitY) * 0.8f;
						int vel_ = vel;
						vel = vel_ + 1;
					}
				}
			}

			if (Main.netMode != NetmodeID.Server) {
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
				Main.dust [dust2].noGravity = true;
			}

			Projectile.rotation += 2 / rotationTimer;
			rotationTimer += 0.01f;
		}

		public override void OnHitPlayer (Player target, Player.HurtInfo info)
			=> target.AddBuff(BuffID.OnFire, 180);

		public override void OnKill (int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			if (Main.netMode != NetmodeID.Server) {
				for (int i = 0; i < 30; i++) {
					int num506 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
					Main.dust [num506].noGravity = true;
					Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
				}
			}
		}

		public override Color? GetAlpha (Color lightColor)
			=> new Color(255, 255, 255, 200);
	}
}