using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
	public class Squib : ModProjectile {
		private bool extraRotation;

		public override void SetDefaults () {
			int width = 15; int height = 25;
			Projectile.Size = new Vector2(width, height);

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.friendly = true;

			Projectile.penetrate = -1;
			Projectile.timeLeft = 240;

			Projectile.tileCollide = true;
		}

		public override void AI () {
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 5) {
				Explode();
				Projectile.Kill();
			}

			Projectile.ai [0] += 1f;
			if (Projectile.ai [0] >= 20f && Projectile.tileCollide)
				Projectile.velocity.Y = Projectile.velocity.Y + 0.15f; // 0.1f for arrow gravity, 0.4f for knife gravity
			if (Projectile.velocity.Y > 16f)
				Projectile.velocity.Y = 16f;
			if (Projectile.velocity.X != 0) {
				Projectile.rotation += Projectile.velocity.X * 0.1f;

				if (extraRotation) {
					Projectile.rotation += 0.15f * Projectile.direction;
					if (Projectile.timeLeft % 20 == 0)
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y + 10, 0.0f, Projectile.velocity.Y * 0.5f, ProjectileID.MolotovFire, Projectile.damage / 4, 2f, Projectile.owner);
				}
			}
			return;
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
			if (Projectile.timeLeft > 4)
				Projectile.timeLeft = 4;
			target.AddBuff(BuffID.OnFire, 180);
		}

		public override void ModifyHitNPC (NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			if (Main.expertMode) {
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) damage /= 5;
			}
		}

		public override void ModifyHitPlayer (Player target, ref int damage, ref bool crit)
			=> damage /= 4;

		public override bool OnTileCollide (Vector2 oldVelocity) {
			extraRotation = true;
			return false;
		}

		private void Explode () {
			Projectile.alpha = 255;
			Projectile.position = Projectile.Center;
			Projectile.Center = Projectile.position;
			Projectile.hostile = true;
			Projectile.maxPenetrate = -1;
			Projectile.penetrate = -1;
			Projectile.Resize(120, 120);
			Projectile.knockBack = 8;
			Projectile.Damage();
		}

		public override void Kill (int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			if (Main.netMode != NetmodeID.Server) {
				Vector2 target5 = Projectile.Center;
				for (int num23 = 0; num23 < Projectile.oldPos.Length; num23++) {
					Vector2 vector5 = Projectile.oldPos [num23];
					if (vector5 == Vector2.Zero) break;
					int num24 = Main.rand.Next(10, 20);
					float num25 = MathHelper.Lerp(0.8f, 1f, Utils.GetLerpValue(Projectile.oldPos.Length, 0f, num23, clamped: true));
					if (num23 >= Projectile.oldPos.Length * 0.25f) num24--;
					if (num23 >= Projectile.oldPos.Length * 0.5f) num24 -= 2;
					Vector2 value4 = vector5.DirectionTo(target5).SafeNormalize(Vector2.Zero);
					target5 = vector5;
					for (float num26 = 0f; num26 < num24; num26++) {
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
							Dust dust10 = Dust.NewDustDirect(vector5, Projectile.width, Projectile.height, DustID.Torch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 50);
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
					Dust dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 50);
					dust11.noGravity = true;
					dust11.velocity = Main.rand.NextVector2Circular(1f, 1f) * 6f;
					dust11.scale = 1.6f;
					dust11.fadeIn = 1.3f + Main.rand.NextFloat() * 1f;
					dust11.noLightEmittence = (dust11.noLight = true);
					Dust dust2 = dust11;
					dust2.velocity += Projectile.velocity * 0.15f;
					dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 0);
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