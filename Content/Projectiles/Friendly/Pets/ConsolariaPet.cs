using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public abstract class ConsolariaPet : ModProjectile {
		public virtual int maxFrames => 0;
		public virtual int PreviewFirstFrame => 0;
		public virtual int PreviewLastFrame => 0;
		public virtual int PreviewOffsetX => 0;
		public virtual int PreviewOffsetY => 0;
		public virtual int PreviewSpriteDirection => 1;

		public override void SetStaticDefaults () {
			Main.projPet [Projectile.type] = true;
			Main.projFrames [Projectile.type] = maxFrames;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(PreviewFirstFrame, PreviewLastFrame)
				.WithOffset(PreviewOffsetX, PreviewOffsetY)
				.WithSpriteDirection(PreviewSpriteDirection)
                .WhenNotSelected(0, 0);
		}

		public override void SetDefaults () {
			Projectile.netImportant = true;
			Projectile.friendly = true;

			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;

			Projectile.tileCollide = true;
		}

		public override bool TileCollideStyle (ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			Player player = Main.player [Projectile.owner];
			float playerDistance = (player.Center - Projectile.Center).Length();
			fallThrough = (playerDistance > 200f);
			return true;
		}

		public override void AI () => CheckPlayer();

		private int playerStill;
		public bool isFlying;

		public void WalkerAI () {
			Player player = Main.player [Projectile.owner];
			if (Projectile.ai [0] == 0f) {
				Projectile.ai [0] = 1f;
				Projectile.Center = player.Center - new Vector2(0f, 25f);
			}
			if (!isFlying) {
                Projectile.rotation = 0f;
				float playerDistance = (player.Center - Projectile.Center).Length();
				if (Projectile.velocity.Y == 0f && (CheckHole() || (playerDistance > 110f && Projectile.position.X == Projectile.oldPosition.X))) {
                    Projectile.velocity.Y = -5f;
				}
				Projectile checkProjectile1 = Projectile;
				checkProjectile1.velocity.Y += 0.35f; // falling speed
				if (Projectile.velocity.Y > 7.5f) {
                    Projectile.velocity.Y = 7.5f;
				}
				if (playerDistance > 600f) {
					isFlying = true;
                    Projectile.velocity.X = 0f;
                    Projectile.velocity.Y = 0f;
                    Projectile.tileCollide = false;
				}
				if (playerDistance > 100f) {
					if (player.position.X - Projectile.position.X > 0f) {
						Projectile checkProjectile2 = Projectile;
						checkProjectile2.velocity.X += 0.1f;
						if (Projectile.velocity.X > 7f) {
                            Projectile.velocity.X = 7f;
						}
					}
					else {
						Projectile checkProjectile3 = Projectile;
						checkProjectile3.velocity.X -= 0.1f;
						if (Projectile.velocity.X < -7f) {
                            Projectile.velocity.X = -7f;
						}
					}
				}
				if (playerDistance < 100f && Projectile.velocity.X != 0f) {
					if (Projectile.velocity.X > 0.5f) {
						Projectile checkProjectile4 = Projectile;
						checkProjectile4.velocity.X -= 0.15f;
					}
					else if (Projectile.velocity.X < -0.5f) {
						Projectile checkProjectile5 = Projectile;
						checkProjectile5.velocity.X += 0.15f;
					}
					else if (Projectile.velocity.X < 0.5f && Projectile.velocity.X > -0.5f) {
                        Projectile.velocity.X = 0f;
					}
				}
				if (Projectile.position.X == Projectile.oldPosition.X && Projectile.position.Y == Projectile.oldPosition.Y && Projectile.velocity.X == 0f) {
                    Projectile.frame = idleFrame;
				}
				else if (Projectile.velocity.Y > 0.3f && Projectile.position.Y != Projectile.oldPosition.Y) {
					Projectile.frame = jumpFrame;
					Projectile.frameCounter = 0;
				}
				else {
					Projectile.frameCounter++;
					if (Projectile.frameCounter > walkingAnimationSpeed) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > walkingLastFrame) {
						Projectile.frame = walkingFirstFrame;
					}
				}
			}
			else if (isFlying) {
				float flySpeedBoost = 0.3f;
                Projectile.tileCollide = false;
				Vector2 position =  new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y +  Projectile.height * 0.5f);
				float posX = Main.player [Projectile.owner].position.X + Main.player [Projectile.owner].width / 2 - position.X;
				float posY = Main.player [Projectile.owner].position.Y + Main.player [Projectile.owner].height / 2 - position.Y;
				posY += Main.rand.Next(-10, 21);
				posX += Main.rand.Next(-10, 21);
				posX += 60f * -(float) Main.player [Projectile.owner].direction;
				posY -= 60f;
				float newPlayerDistance = (float) Math.Sqrt((double) (posX * posX + posY * posY));
				float num17 = 18f;
				Math.Sqrt((double) (posX * posX + posY * posY));
				if (newPlayerDistance > 1200f) {
                    Projectile.position.X = Main.player [Projectile.owner].Center.X - Projectile.width / 2;
                    Projectile.position.Y = Main.player [Projectile.owner].Center.Y - Projectile.height / 2;
                    Projectile.netUpdate = true;
				}
				if (newPlayerDistance < 100f) {
					flySpeedBoost = 0.1f;
					if (player.velocity.Y == 0f) {
						playerStill++;
					}
					else {
						playerStill = 0;
					}
					if (playerStill > 30 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
						isFlying = false;
                        Projectile.tileCollide = true;
					}
				}
				if (newPlayerDistance < 50f) {
					if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f) {
                        Projectile.velocity *= 0.9f;
					}
					flySpeedBoost = 0.01f;
				}
				else {
					if (newPlayerDistance < 100f) {
						flySpeedBoost = 0.1f;
					}
					if (newPlayerDistance > 300f) {
						flySpeedBoost = 1f;
					}
					newPlayerDistance = num17 / newPlayerDistance;
					posX *= newPlayerDistance;
					posY *= newPlayerDistance;
				}
				if (Projectile.velocity.X <= posX) {
                    Projectile.velocity.X = Projectile.velocity.X + flySpeedBoost;
					if (flySpeedBoost > 0.05f && Projectile.velocity.X < 0f) {
                        Projectile.velocity.X = Projectile.velocity.X + flySpeedBoost;
					}
				}
				if (Projectile.velocity.X > posX) {
                    Projectile.velocity.X = Projectile.velocity.X - flySpeedBoost;
					if (flySpeedBoost > 0.05f && Projectile.velocity.X > 0f) {
                        Projectile.velocity.X = Projectile.velocity.X - flySpeedBoost;
					}
				}
				if (Projectile.velocity.Y <= posY) {
                    Projectile.velocity.Y = Projectile.velocity.Y + flySpeedBoost;
					if (flySpeedBoost > 0.05f && Projectile.velocity.Y < 0f) {
                        Projectile.velocity.Y = Projectile.velocity.Y + flySpeedBoost * 2f;
					}
				}
				if (Projectile.velocity.Y > posY) {
                    Projectile.velocity.Y = Projectile.velocity.Y - flySpeedBoost;
					if (flySpeedBoost > 0.05f && Projectile.velocity.Y > 0f) {
                        Projectile.velocity.Y = Projectile.velocity.Y - flySpeedBoost * 2f;
					}
				}
				if (oneFrame == true) {
					Projectile.frame = maxFrames - 1;
				}
				else {
					Projectile.frameCounter++;
					if (Projectile.frameCounter > flyingAnimationSpeed) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > flyingLastFrame) {
						Projectile.frame = flyingFirstFrame;
					}
				}
			}
			if (Projectile.velocity.X > 0.25f) {
                Projectile.spriteDirection = -1;
				return;
			}
			if (Projectile.velocity.X < -0.25f) {
                Projectile.spriteDirection = 1;
			}
		}

		private int idleFrame, jumpFrame;

		public void PassiveAnimation (int idleFrame, int jumpFrame) {
			this.idleFrame = idleFrame;
			this.jumpFrame = jumpFrame;
		}

		private int walkingAnimationSpeed,  walkingFirstFrame,  walkingLastFrame;

		public void WalkingAnimation (int walkingAnimationSpeed, int walkingFirstFrame, int walkingLastFrame) {
			this.walkingAnimationSpeed = walkingAnimationSpeed;
			this.walkingFirstFrame = walkingFirstFrame;
			this.walkingLastFrame = walkingLastFrame;
		}

		private int flyingAnimationSpeed, flyingFirstFrame, flyingLastFrame;
		private bool oneFrame;

		public void FlyingAnimation (int flyingAnimationSpeed, int flyingFirstFrame, int flyingLastFrame) {
			this.flyingAnimationSpeed = flyingAnimationSpeed;
			this.flyingFirstFrame = flyingFirstFrame;
			this.flyingLastFrame = flyingLastFrame;		
		}

		public void FlyingAnimation (bool oneFrame) {
			this.oneFrame = oneFrame;
		}

		private int texFrameCounter;
		private int texCurrentFrame;

		public void OverrideVanillaAnimation (Color lightColor, int animationSpeed, int walkingLastFrame, int flyingFirstFrame) {
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
			bool onGround = Projectile.velocity.Y == 0f;
			texFrameCounter++;
			if (texFrameCounter > animationSpeed) {
				texFrameCounter = 0;
				texCurrentFrame++;
				if (texCurrentFrame >= (onGround ? walkingLastFrame : Main.projFrames [Projectile.type]))
					texCurrentFrame = onGround ? 0 : flyingFirstFrame;
			}
			if (onGround && Projectile.velocity.X == 0f) {
				texCurrentFrame = 0;
				texFrameCounter = 0;
			}

			Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			int frameHeight = texture.Height / Main.projFrames [Projectile.type];
			Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
			Main.EntitySpriteDraw(texture, position, frameRect, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
		}

		private bool CheckHole () {
			int tileWidth = 4;
			int tileX = (int) (Projectile.Center.X / 16f) - tileWidth;
			if (Projectile.velocity.X > 0f) {
				tileX += tileWidth;
			}
			int tileY = (int) ((Projectile.position.Y + Projectile.height) / 16f);
			for (int y = tileY; y < tileY + 2; y++) {
				for (int x = tileX; x < tileX + tileWidth; x++) {
					if (Main.tile [x, y].HasTile) {
						return false;
					}
				}
			}
			return true;
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