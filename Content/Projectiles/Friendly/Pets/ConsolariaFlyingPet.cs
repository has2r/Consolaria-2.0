using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public abstract class ConsolariaFlyingPet : ModProjectile {
		public virtual int maxFrames => 0;
		public virtual int PreviewOffsetX => 0;
		public virtual int PreviewOffsetY => 0;
		public virtual int PreviewSpriteDirection => 1;
		public virtual bool isLightPet => false;

		public override void SetStaticDefaults () {
			Main.projPet [Projectile.type] = true;

			Main.projFrames [Projectile.type] = maxFrames;
			ProjectileID.Sets.LightPet [Projectile.type] = isLightPet;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, maxFrames)
				.WithOffset(PreviewOffsetX, PreviewOffsetY)
				.WithSpriteDirection(PreviewSpriteDirection)
                .WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults () {
			Projectile.netImportant = true;
			Projectile.friendly = true;

			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
		}

		public override void AI () => CheckPlayer();

		public void FloatingAI (float tilt) {
			Player player = Main.player [Projectile.owner];
			float movement = 0.05f;
			for (int i = 0; i < 1000; i++) {
				Projectile checkProjectile = Main.projectile [i];
				if (checkProjectile.active && checkProjectile.owner == Projectile.owner && Main.projPet [checkProjectile.type] && i != Projectile.whoAmI) {
					bool flag = Main.projPet [checkProjectile.type];
					float taxicabDist = Math.Abs(Projectile.position.X - checkProjectile.position.X) + Math.Abs(Projectile.position.Y - checkProjectile.position.Y);
					if (flag && taxicabDist < Projectile.width) {
						if (Projectile.position.X < checkProjectile.position.X) {
							Projectile.velocity.X = Projectile.velocity.X - movement;
						}
						else {
							Projectile.velocity.X = Projectile.velocity.X + movement;
						}
						if (Projectile.position.Y < checkProjectile.position.Y) {
							Projectile.velocity.Y = Projectile.velocity.Y - movement;
						}
						else {
							Projectile.velocity.Y = Projectile.velocity.Y + movement;
						}
					}
				}
			}
			float flySpeedBoost = 0.5f;
			Projectile.tileCollide = false;
			float range = 100f;
			Vector2 projPos = Projectile.Center;
			float xDist = player.Center.X - projPos.X;
			float yDist = player.Center.Y - projPos.Y;
			yDist += Utils.NextFloat(Main.rand, -10f, 20f);
			xDist += Utils.NextFloat(Main.rand, -10f, 20f);
			xDist += 60f * (isLightPet ? player.direction : -player.direction);
			yDist -= 60f;
			Vector2 playerVector = new Vector2(xDist, yDist);
			float playerDist = playerVector.Length();
			float returnSpeed = 18f;
			if (playerDist < range && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) && Projectile.velocity.Y < -6f) {
				Projectile.velocity.Y = -6f;
			}
			if (playerDist > 2000f) {
				Projectile.position.X = player.Center.X - Projectile.width / 2;
				Projectile.position.Y = player.Center.Y - Projectile.height / 2;
				Projectile.netUpdate = true;
			}
			if (playerDist < 50f) {
				if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f) {
					Projectile.velocity *= 0.99f;
				}
				flySpeedBoost = 0.01f;
			}
			else {
				if (playerDist < 100f) {
					flySpeedBoost = 0.1f;
				}
				if (playerDist > 300f)
					flySpeedBoost = 1f;

				playerDist = returnSpeed / playerDist;
				playerVector.X *= playerDist;
				playerVector.Y *= playerDist;
			}
			float speedCheck = 0.05f;
			if (Projectile.velocity.X < playerVector.X) {
				Projectile.velocity.X = Projectile.velocity.X + flySpeedBoost;
				if (flySpeedBoost > speedCheck && Projectile.velocity.X < 0f) {
					Projectile.velocity.X = Projectile.velocity.X + flySpeedBoost;
				}
			}
			if (Projectile.velocity.X > playerVector.X) {
				Projectile.velocity.X = Projectile.velocity.X - flySpeedBoost;
				if (flySpeedBoost > speedCheck && Projectile.velocity.X > 0f) {
					Projectile.velocity.X = Projectile.velocity.X - flySpeedBoost;
				}
			}
			if (Projectile.velocity.Y < playerVector.Y) {
				Projectile.velocity.Y = Projectile.velocity.Y + flySpeedBoost;
				if (flySpeedBoost > speedCheck && Projectile.velocity.Y < 0f) {
					Projectile.velocity.Y = Projectile.velocity.Y + flySpeedBoost * 2f;
				}
			}
			if (Projectile.velocity.Y > playerVector.Y) {
				Projectile.velocity.Y = Projectile.velocity.Y - flySpeedBoost;
				if (flySpeedBoost > speedCheck && Projectile.velocity.Y > 0f) {
					Projectile.velocity.Y = Projectile.velocity.Y - flySpeedBoost * 2f;
				}
			}
			if (player.velocity.X != 0) {
				if (Projectile.velocity.X > 0.25f) {
					Projectile.spriteDirection = -1;
				}
				if (Projectile.velocity.X < -0.25f) {
					Projectile.spriteDirection = 1;
				}
			}
			Projectile.rotation = Projectile.velocity.X * tilt;
		}

		public void Animation (int animationSpeed) {
            Projectile.frameCounter++;
			if (Projectile.frameCounter > animationSpeed) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
			}
			if (Projectile.frame > Main.projFrames [Projectile.type] - 1) {
                Projectile.frame = 0;
			}
		}

		public void LightColor (Color color, float brightness) {
            if (!Main.dedServ)
				Lighting.AddLight(Projectile.Center, color.ToVector3() * brightness);
		}

        private void CheckPlayer () {
			Player player = Main.player [Projectile.owner];
			if (!player.active) {
				Projectile.active = false;
				return;
			}
		}
	}
}