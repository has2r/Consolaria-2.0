using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class ShadowflameBreath : ModProjectile {
        public override string Texture
			=> $"Terraria/Images/Projectile_85";

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults () {
            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.penetrate = 4;
            Projectile.timeLeft = 110;

            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

		public override bool? CanDamage()
			=> Projectile.timeLeft > 27;

        public override void AI () {
           	Projectile.localAI[0] += 1f;
			int num = 90;
			int num2 = 12;
			int num3 = num + num2;
			if (Projectile.localAI[0] >= (float)num3)
				Projectile.Kill();

			if (Projectile.localAI[0] >= (float)num)
				Projectile.velocity *= 0.95f;

			int num4 = 80;
			int num5 = num4;

			if (Projectile.localAI[0] < (float)num5 && Main.rand.NextFloat() < 0.15f) {
				short num6 = DustID.Shadowflame;
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(30f, 30f) * Utils.Remap(Projectile.localAI[0], 0f, 72f, 0.5f, 1f), 4, 4, num6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 150);
				if (Main.rand.NextBool(4)) {
					dust.noGravity = true;
					dust.scale *= 1.6f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
				}
				else {
					dust.scale *= 0.8f;
				}

				dust.scale *= 1.25f;
				dust.velocity *= 1.2f;
				dust.velocity += Projectile.velocity * 1f * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.75f, 1f, 0.1f) * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.1f, 0.1f, 1f);
				dust.customData = 1;
			}

			if (num4 > 0 && Projectile.localAI[0] >= (float)num4 && Main.rand.NextFloat() < 0.25f) {
				Vector2 center = Main.player[Projectile.owner].Center;
				Vector2 vector = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
				short num7 = 14;
				Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, num7, 0f, 0f, 150);
				dust2.noGravity = true;
				dust2.velocity = vector;
				dust2.scale *= 0.9f + Main.rand.NextFloat() * 0.2f;
				dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
			}
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            int num = (int)Utils.Remap(Projectile.localAI[0], 0f, 60f, 10f, 40f);
            hitbox.Inflate(num, num);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            if (!projHitbox.Intersects(targetHitbox))
                return false;

            return Collision.CanHit(Projectile.Center, 0, 0, targetHitbox.Center.ToVector2(), 0, 0);
        }

        public override bool PreDraw(ref Color lightColor) {
			DrawProj_Flamethrower();
			return false;
        }

        private void DrawProj_Flamethrower() {
			Projectile proj = Projectile;
			float num = 114f;
			float num2 = 12f;
			float fromMax = num + num2;
			Texture2D value = TextureAssets.Projectile[proj.type].Value;
			Color transparent = Color.Transparent;
            Color color = new Color(82, 80, 195, 200);
			Color color2 = new Color(160, 115, 190, 80);
			Color color3 = Color.Lerp(color2, color, 0.25f);
			Color color4 = new Color(56, 56, 164, 150);
			float num3 = 0.35f;
			float num4 = 0.7f;
			float num5 = 0.85f;
			float num6 = (proj.localAI[0] > num - 10f) ? 0.175f : 0.2f;

			int num7 = 3;
			int num8 = 2;
			int verticalFrames = 7;
			float scale = Utils.Remap(proj.localAI[0], num, fromMax, 1f, 0f);
			float num9 = Math.Min(proj.localAI[0], 20f);
			float num10 = Utils.Remap(proj.localAI[0], 0f, fromMax, 0f, 1f);
			float num11 = Utils.Remap(num10, 0.2f, 0.5f, 0.25f, 1f);
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, (int)Utils.Remap(num10, 0.5f, 1f, 3f, 5f));
			if (!(num10 < 1f))
				return;

			for (int i = 0; i < 2; i++) {
				for (float num12 = 1f; num12 >= 0f; num12 -= num6) {
					transparent = ((num10 < 0.1f) ? Color.Lerp(Color.Transparent, color, Utils.GetLerpValue(0f, 0.1f, num10, clamped: true)) : ((num10 < 0.2f) ? Color.Lerp(color, color2, Utils.GetLerpValue(0.1f, 0.2f, num10, clamped: true)) : ((num10 < num3) ? color2 : ((num10 < num4) ? Color.Lerp(color2, color3, Utils.GetLerpValue(num3, num4, num10, clamped: true)) : ((num10 < num5) ? Color.Lerp(color3, color4, Utils.GetLerpValue(num4, num5, num10, clamped: true)) : ((!(num10 < 1f)) ? Color.Transparent : Color.Lerp(color4, Color.Transparent, Utils.GetLerpValue(num5, 1f, num10, clamped: true))))))));
					float num13 = (1f - num12) * Utils.Remap(num10, 0f, 0.2f, 0f, 1f);
					Vector2 vector = proj.Center - Main.screenPosition + proj.velocity * (0f - num9) * num12;
					Color color5 = transparent * num13;
					Color value2 = color5;
					//value2.G /= 2;
					//value2.B /= 2;
					value2.A = (byte)Math.Min((float)(int)color5.A + 80f * num13, 255f);
					Utils.Remap(proj.localAI[0], 20f, fromMax, 0f, 1f);

					float num14 = 1f / num6 * (num12 + 1f);
					float num15 = proj.rotation + num12 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num14 * 2f;
					float num16 = proj.rotation - num12 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num14 * 2f;

					float opacity = Projectile.timeLeft < 40f ? (Projectile.timeLeft - 10f) / 30f : 1f;

                    switch (i) {
						case 0:
                            Main.EntitySpriteDraw(value, vector + proj.velocity * (0f - num9) * num6 * 0.5f, rectangle, value2 * scale * 0.25f * opacity, num15 + (float)Math.PI / 4f, rectangle.Size() / 2f, num11, SpriteEffects.None);
                            Main.EntitySpriteDraw(value, vector, rectangle, value2 * scale * opacity, num16, rectangle.Size() / 2f, num11, SpriteEffects.None);
							break;
						case 1:
                            Main.EntitySpriteDraw(value, vector + proj.velocity * (0f - num9) * num6 * 0.2f, rectangle, color5 * scale * 0.25f * opacity, num15 + (float)Math.PI / 2f, rectangle.Size() / 2f, num11 * 0.75f, SpriteEffects.None);
                            Main.EntitySpriteDraw(value, vector, rectangle, color5 * scale * opacity, num16 + (float)Math.PI / 2f, rectangle.Size() / 2f, num11 * 0.75f, SpriteEffects.None);
							break;
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.ShadowFlame, 180);
			Projectile.damage = (int)(Projectile.damage * 0.8f);
		}

        public override void OnHitPlayer (Player target, Player.HurtInfo info) {
			if (info.PvP) {
				target.AddBuff(BuffID.ShadowFlame, 180);
				Projectile.damage = (int)(Projectile.damage * 0.8f);
			}
        }
    }
}