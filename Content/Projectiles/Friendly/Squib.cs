using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
	public class Squib : ModProjectile {
		private bool extraRotation;

		public override void SetDefaults () {
			int width = 16; int height = 24;
			Projectile.Size = new Vector2(width, height);

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.aiStyle = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 180;
		}

		public override void AI () {
			if (extraRotation) {
				Projectile.rotation += 0.25f * Projectile.direction;
				if (Projectile.ai [1] % 15 == 0)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y + 10, 0.0f, Projectile.velocity.Y * 0.5f, ProjectileID.MolotovFire, Projectile.damage / 3, 2f, Projectile.owner);
			}
		}

		public override bool OnTileCollide (Vector2 oldVelocity)
			=> extraRotation = true;

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
			=> target.AddBuff(BuffID.OnFire, 180);

		public override void OnHitPvp (Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.OnFire, 180);

		public override void Kill (int timeLeft) {
			Player player = Main.player [Projectile.owner];
			if (Projectile.owner == Main.myPlayer) {
				SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

				int width = Projectile.width;
				int height = Projectile.height;

				Projectile.Resize(60, 60);

				Projectile.maxPenetrate = -1;
				Projectile.penetrate = -1;
				Projectile.Damage();
				Projectile.Resize(width, height);

				Vector2 target5 = Projectile.Center;
				for (int num23 = 0; num23 < Projectile.oldPos.Length; num23++) {
					Vector2 vector5 = Projectile.oldPos [num23];
					if (vector5 == Vector2.Zero) break;
					int num24 = Main.rand.Next(10, 20);
					float num25 = MathHelper.Lerp(0.8f, 1f, Utils.GetLerpValue(Projectile.oldPos.Length, 0f, num23, clamped: true));
					if ((float) num23 >= (float) Projectile.oldPos.Length * 0.25f) num24--;
					if ((float) num23 >= (float) Projectile.oldPos.Length * 0.5f) num24 -= 2;
					Vector2 value4 = vector5.DirectionTo(target5).SafeNormalize(Vector2.Zero);
					target5 = vector5;
					for (float num26 = 0f; num26 < (float) num24; num26++) {
						if (Main.rand.NextBool(2)) {
							int num27 = Dust.NewDust(vector5, Projectile.width, Projectile.height, DustID.Firework_Red, 0f, 0f, 50, default);
							Dust dust2 = Main.dust [num27];
							dust2.velocity *= Main.rand.NextFloat() * 0.8f;
							Main.dust [num27].noGravity = true;
							Main.dust [num27].scale = Main.rand.NextFloat() * 1f;
							Main.dust [num27].fadeIn = Main.rand.NextFloat() * 2f;
							dust2 = Main.dust [num27];
							dust2.velocity += value4 * 8f;
							dust2 = Main.dust [num27];
							dust2.scale *= num25;
							if (num27 != 6000) {
								Dust dust9 = Dust.CloneDust(num27);
								dust2 = dust9;
								dust2.scale /= 2f;
								dust2 = dust9;
								dust2.fadeIn /= 2f;
								dust9.color = new Color(255, 255, 255);
							}
						}
						else {
							Dust dust10 = Dust.NewDustDirect(vector5, Projectile.width, Projectile.height, 6, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 50);
							Dust dust2;
							if (Main.rand.NextBool(2)) {
								dust10.noGravity = true;
								dust2 = dust10;
								dust2.scale *= 2.5f;
							}
							dust2 = dust10;
							dust2.velocity *= 2.5f;
							dust2 = dust10;
							dust2.velocity += value4 * 6f;
							dust2 = dust10;
							dust2.scale *= num25;
							dust10.noLightEmittence = (dust10.noLight = true);
						}
					}
				}
				for (int num28 = 0; num28 < 30; num28++) {
					Dust dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 50);
					dust11.noGravity = true;
					dust11.velocity = Main.rand.NextVector2Circular(1f, 1f) * 6f;
					dust11.scale = 1.6f;
					dust11.fadeIn = 1.3f + Main.rand.NextFloat() * 1f;
					dust11.noLightEmittence = (dust11.noLight = true);
					Dust dust2 = dust11;
					dust2.velocity += Projectile.velocity * 0.15f;
					dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 0);
					dust2 = dust11;
					dust2.velocity *= 2f;
					dust11.noLightEmittence = (dust11.noLight = true);
					dust2 = dust11;
					dust2.velocity += Projectile.velocity * 0.1f;
				}
			}
		}
	}
}
