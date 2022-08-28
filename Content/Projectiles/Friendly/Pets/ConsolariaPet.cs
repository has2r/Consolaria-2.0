using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
	public abstract class ConsolariaPet : ModProjectile {
		public virtual int maxFrames => 0;

		public override void SetStaticDefaults () {
			Main.projPet [Projectile.type] = true;
			Main.projFrames [Projectile.type] = maxFrames;
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
			Vector2 center2 = Projectile.Center;
			float playerDistance = (player.Center - center2).Length();
			fallThrough = (playerDistance > 200f);
			return true;
		}

		public override void AI () => CheckPlayer();

		private int playerStill;
		private bool fly;

		public void WalkerAI () {
			Player player = Main.player [Projectile.owner];
			if (!fly) {
                Projectile.rotation = 0f;
				Vector2 center2 = Projectile.Center;
				float playerDistance = (player.Center - center2).Length();
				if (Projectile.velocity.Y == 0f && (CheckHole() || (playerDistance > 110f && Projectile.position.X == Projectile.oldPosition.X))) {
                    Projectile.velocity.Y = -5f;
				}
				Projectile projectile = Projectile;
				projectile.velocity.Y = projectile.velocity.Y + 0.2f;
				if (Projectile.velocity.Y > 7f) {
                    Projectile.velocity.Y = 7f;
				}
				if (playerDistance > 600f) {
					fly = true;
                    Projectile.velocity.X = 0f;
                    Projectile.velocity.Y = 0f;
                    Projectile.tileCollide = false;
				}
				if (playerDistance > 100f) {
					if (player.position.X - Projectile.position.X > 0f) {
						Projectile projectile2 = Projectile;
						projectile2.velocity.X = projectile2.velocity.X + 0.1f;
						if (Projectile.velocity.X > 7f) {
                            Projectile.velocity.X = 7f;
						}
					}
					else {
						Projectile projectile3 = Projectile;
						projectile3.velocity.X = projectile3.velocity.X - 0.1f;
						if (Projectile.velocity.X < -7f) {
                            Projectile.velocity.X = -7f;
						}
					}
				}
				if (playerDistance < 100f && Projectile.velocity.X != 0f) {
					if (Projectile.velocity.X > 0.5f) {
						Projectile projectile4 = Projectile;
						projectile4.velocity.X = projectile4.velocity.X - 0.15f;
					}
					else if (Projectile.velocity.X < -0.5f) {
						Projectile projectile5 = Projectile;
						projectile5.velocity.X = projectile5.velocity.X + 0.15f;
					}
					else if (Projectile.velocity.X < 0.5f && Projectile.velocity.X > -0.5f) {
                        Projectile.velocity.X = 0f;
					}
				}
				if (Projectile.position.X == Projectile.oldPosition.X && Projectile.position.Y == Projectile.oldPosition.Y && Projectile.velocity.X == 0f) {
                    Projectile.frame = 0;
				}
				else if (Projectile.velocity.Y > 0.3f && Projectile.position.Y != Projectile.oldPosition.Y) {
                    Projectile.frame = 1;
                    Projectile.frameCounter = 0;
				}
				else {
					/*  Projectile.frameCounter++;
					  if (Projectile.frameCounter > 5) {
						  Projectile.frame++;
						  Projectile.frameCounter = 0;
					  }
					  if (Projectile.frame > 6) {
						  Projectile.frame = 2;
					  }
					  if (Projectile.frame < 2) {
						  Projectile.frame = 2;
					  }*/
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
			else if (fly) {
				float num16 = 0.3f;
                Projectile.tileCollide = false;
				Vector2 vector3 =  new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y +  Projectile.height * 0.5f);
				float horiPos = Main.player [Projectile.owner].position.X + Main.player [Projectile.owner].width / 2 - vector3.X;
				float vertiPos = Main.player [Projectile.owner].position.Y + Main.player [Projectile.owner].height / 2 - vector3.Y;
				vertiPos += Main.rand.Next(-10, 21);
				horiPos += Main.rand.Next(-10, 21);
				horiPos += 60f * -(float) Main.player [Projectile.owner].direction;
				vertiPos -= 60f;
				float playerDistance2 = (float) Math.Sqrt((double) (horiPos * horiPos + vertiPos * vertiPos));
				float num17 = 18f;
				Math.Sqrt((double) (horiPos * horiPos + vertiPos * vertiPos));
				if (playerDistance2 > 1200f) {
                    Projectile.position.X = Main.player [Projectile.owner].Center.X - Projectile.width / 2;
                    Projectile.position.Y = Main.player [Projectile.owner].Center.Y - Projectile.height / 2;
                    Projectile.netUpdate = true;
				}
				if (playerDistance2 < 100f) {
					num16 = 0.1f;
					if (player.velocity.Y == 0f) {
						playerStill++;
					}
					else {
						playerStill = 0;
					}
					if (playerStill > 60 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
						fly = false;
                        Projectile.tileCollide = true;
					}
				}
				if (playerDistance2 < 50f) {
					if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f) {
                        Projectile.velocity *= 0.9f;
					}
					num16 = 0.01f;
				}
				else {
					if (playerDistance2 < 100f) {
						num16 = 0.1f;
					}
					if (playerDistance2 > 300f) {
						num16 = 1f;
					}
					playerDistance2 = num17 / playerDistance2;
					horiPos *= playerDistance2;
					vertiPos *= playerDistance2;
				}
				if (Projectile.velocity.X <= horiPos) {
                    Projectile.velocity.X = Projectile.velocity.X + num16;
					if (num16 > 0.05f && Projectile.velocity.X < 0f) {
                        Projectile.velocity.X = Projectile.velocity.X + num16;
					}
				}
				if (Projectile.velocity.X > horiPos) {
                    Projectile.velocity.X = Projectile.velocity.X - num16;
					if (num16 > 0.05f && Projectile.velocity.X > 0f) {
                        Projectile.velocity.X = Projectile.velocity.X - num16;
					}
				}
				if (Projectile.velocity.Y <= vertiPos) {
                    Projectile.velocity.Y = Projectile.velocity.Y + num16;
					if (num16 > 0.05f && Projectile.velocity.Y < 0f) {
                        Projectile.velocity.Y = Projectile.velocity.Y + num16 * 2f;
					}
				}
				if (Projectile.velocity.Y > vertiPos) {
                    Projectile.velocity.Y = Projectile.velocity.Y - num16;
					if (num16 > 0.05f && Projectile.velocity.Y > 0f) {
                        Projectile.velocity.Y = Projectile.velocity.Y - num16 * 2f;
					}
				}
                Projectile.rotation = Projectile.velocity.X * 0.03f;
				//Projectile.frame = 7;
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