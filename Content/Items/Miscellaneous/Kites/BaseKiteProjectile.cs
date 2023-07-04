using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous.Kites.Custom;

public record KiteInfo(float MinLength = 4f,
                       float MaxLength = 500f,
                       int SegmentsCount = 15,
                       int SegmentsCountToDraw = 10,
                       float LengthBetweenBodySegments = 10f,
                       int BodyXPositionOffset = -14,
                       int HeadYPositionOffset = -2,
                       int LengthBetweenTailSegments = -1,
                       int TailLength = -1,
                       int WindResistance = 8);

public abstract class BaseKiteProjectile : ModProjectile {
    #region Properties
    protected Player Owner
        => Main.player[Projectile.owner];

    protected ref float KiteLength
        => ref Projectile.ai[0];

	protected abstract KiteInfo KiteInfo();
    #endregion

    #region TML Hooks
    public sealed override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 80;
		ProjectileID.Sets.TrailingMode[Type] = 2;
    }

    public sealed override void SetDefaults() {
        int width = 4, height = width;
        Projectile.Size = new Vector2(width, height);

        Projectile.penetrate = -1;

        Projectile.aiStyle = -1;
        Projectile.extraUpdates = 60;
    }

    public sealed override bool PreAI()
        => true;

    public sealed override void AI()
		=> UpdateKite();

    public override void PostAI() {
    }

    public sealed override bool PreDraw(ref Color lightColor)
		=> false;

    public override void PostDraw(Color lightColor)
        => DrawKite();

    public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity *= 0.75f;

        return false;
    }
	#endregion

	#region Custom
	protected virtual float SetSegmentRotation()
		=> 0f;

    protected virtual float SetHeadRotation()
		=> 8f;

    private void UpdateKite() {
		Projectile.timeLeft = 60;

        if (Despawn()) {
            return;
        }

        float halfKiteLength = KiteInfo().MaxLength / 2f;
        if (Projectile.owner == Main.myPlayer && Projectile.extraUpdates == 0) {
            float netUpdateValue1 = KiteLength;
            if (KiteLength == 0f) {
                KiteLength = halfKiteLength;
            }

            float netUpdateValue2 = KiteLength;
            if (Main.mouseRight) {
                netUpdateValue2 -= 5f;
            }

            if (Main.mouseLeft) {
                netUpdateValue2 += 5f;
            }

            KiteLength = MathHelper.Clamp(netUpdateValue2, KiteInfo().MinLength, KiteInfo().MaxLength);
            if (netUpdateValue1 != netUpdateValue2) {
                Projectile.netUpdate = true;
            }
        }

        if (Projectile.numUpdates == 1) {
            Projectile.extraUpdates = 0;
        }

        if (!MoveOnWind()) {
            return;
        }

        Projectile.spriteDirection = Owner.direction;
	}

    private bool Despawn() {
        bool whenToDespawn = false;
        if (Owner.CCed || Owner.noItems) {
            whenToDespawn = true;
        }
        else if (Owner.inventory[Owner.selectedItem].shoot != Type) { 
            whenToDespawn = true;
        }
        else if (Owner.pulley) {
            whenToDespawn = true;
        }
        else if (Owner.dead) {
            whenToDespawn = true;
        }
        float maxProjectileDistance = 2000f;
        if (!whenToDespawn) {
            whenToDespawn = ((Owner.Center - Projectile.Center).Length() > maxProjectileDistance);
        }
        if (whenToDespawn) {
            Projectile.Kill();

            return true;
        }

        return false;
    }

    #region Vanilla
    private bool MoveOnWind() {
        Vector2 vector = Owner.RotatedRelativePoint(Owner.MountedCenter);

        int width = Projectile.width, height = Projectile.height;

        float cloudAlpha = Main.cloudAlpha;
        float windSpeed = 0f;
        if (WorldGen.InAPlaceWithWind(Projectile.position, width, height)) {
            windSpeed = Main.WindForVisuals;
        }

        float num8 = Utils.GetLerpValue(0.2f, 0.5f, Math.Abs(windSpeed), clamped: true) * 0.5f;
        Vector2 mouseWorld = Projectile.Center;
        mouseWorld += new Vector2(windSpeed, (float)Math.Sin(Main.GlobalTimeWrappedHourly) + cloudAlpha * 5f) * 25f;
        Vector2 v = mouseWorld - Projectile.Center;
        v = v.SafeNormalize(Vector2.Zero) * (3f + cloudAlpha * 7f);
        if (num8 == 0f)
            v = Projectile.velocity;

        float num9 = Projectile.Distance(mouseWorld);
        float lerpValue = Utils.GetLerpValue(5f, 10f, num9, clamped: true);
        float y = Projectile.velocity.Y;
        if (num9 > 10f)
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, v, 0.075f * lerpValue);

        Projectile.velocity.Y = y;
        Projectile.velocity.Y -= num8;
        Projectile.velocity.Y += 0.02f + num8 * 0.25f;
        Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y, -2f, 2f);
        if (Projectile.Center.Y + Projectile.velocity.Y < mouseWorld.Y)
            Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, Projectile.velocity.Y + num8 + 0.01f, 0.75f);

        Projectile.velocity.X *= 0.98f;
        float num10 = Projectile.Distance(vector);
        float num11 = KiteLength;
        if (num10 > num11) {
            Vector2 vector2 = Projectile.DirectionTo(vector);
            float scaleFactor = num10 - num11;
            Projectile.Center += vector2 * scaleFactor;
            bool num12 = Vector2.Dot(vector2, Vector2.UnitY) < 0.8f || num8 > 0f;
            Projectile.velocity.Y += vector2.Y * 0.05f;
            if (num12)
                Projectile.velocity.Y -= 0.15f;

            Projectile.velocity.X += vector2.X * 0.2f;
            if (num11 == KiteInfo().MinLength && Projectile.owner == Main.myPlayer)
            {
                Projectile.Kill();

                return false;
            }
        }

        Projectile.timeLeft = 2;
        Vector2 vector3 = Projectile.Center - vector;
        int dir = (vector3.X > 0f) ? 1 : (-1);

        Owner.ChangeDir(dir);

        Vector2 value2 = Projectile.DirectionTo(vector).SafeNormalize(Vector2.Zero);
        if (num8 == 0f && Projectile.velocity.Y > -0.02f)
        {
            Projectile.rotation *= 0.95f;
        }
        else
        {
            float num13 = (-value2).ToRotation() + (float)Math.PI / 4f;
            if (Projectile.spriteDirection == -1)
                num13 -= (float)Math.PI / 2f * (float)Owner.direction;

            Projectile.rotation = num13 + Projectile.velocity.X * 0.05f;
        }

        return true;
    }

    private void DrawKite() {
        Texture2D value = (Texture2D)ModContent.Request<Texture2D>(Texture);
        Texture2D value2 = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Body");

		int num4 = 5;
		int num13 = 1;
		int num14 = 0;
		int num15 = 0;
		bool flag = true;
		bool flag2 = false;

        int num = KiteInfo().SegmentsCount;
        int num12 = 500; //something
        float num2 = SetSegmentRotation();
        int num3 = KiteInfo().SegmentsCountToDraw;
        float num5 = KiteInfo().LengthBetweenBodySegments;
		float num6 = SetHeadRotation();
        int num7 = KiteInfo().BodyXPositionOffset;
        int num8 = KiteInfo().HeadYPositionOffset;
        int num9 = KiteInfo().LengthBetweenTailSegments;
        int num10 = KiteInfo().TailLength;
        int num11 = KiteInfo().WindResistance;

		SpriteEffects effects = (Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		Rectangle rectangle = value.Frame(Main.projFrames[Type], 1, Projectile.frame);
		Vector2 origin = rectangle.Size() / 2f;
		Vector2 position = Projectile.Center - Main.screenPosition;
		Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		Color alpha = Projectile.GetAlpha(color);
		Texture2D value3 = TextureAssets.FishingLine.Value;
		Rectangle value4 = value3.Frame();
		Vector2 origin2 = new Vector2(value4.Width / 2, 2f);
		Rectangle rectangle2 = value2.Frame(num);
		int width = rectangle2.Width;
		rectangle2.Width -= 2;
		Vector2 origin3 = rectangle2.Size() / 2f;
		rectangle2.X = width * (num - 1);
		Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
		Vector2 center = Projectile.Center;
		Vector2.Distance(center, playerArmPosition);
		float scaleFactor = 12f;
		_ = (playerArmPosition - center).SafeNormalize(Vector2.Zero) * scaleFactor;
		if (Owner.itemAnimation <= 0)
			Owner.bodyFrame = new Rectangle(0, 56 * 3, 40, 56);
        Vector2 vector = Owner.Center;
		Vector2 vector2 = center - vector;
		Vector2 velocity = Projectile.velocity;
		if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
			Utils.Swap(ref velocity.X, ref velocity.Y);

		float num16 = vector2.Length();
		float num17 = 16f;
		float num18 = 80f;
		bool flag3 = true;
		if (num16 == 0f) {
			flag3 = false;
		}
		else {
			vector2 *= 12f / num16;
			vector -= vector2;
			vector2 = center - vector;
		}

		while (flag3) {
			float num19 = 12f;
			float num20 = vector2.Length();
			float num21 = num20;
			if (float.IsNaN(num20) || num20 == 0f) {
				flag3 = false;
				continue;
			}

			if (num20 < 20f) {
				num19 = num20 - 8f;
				flag3 = false;
			}

			num20 = 12f / num20;
			vector2 *= num20;
			vector += vector2;
			vector2 = center - vector;
			if (num21 > 12f) {
				float num22 = 0.3f;
				float num23 = Math.Abs(velocity.X) + Math.Abs(velocity.Y);
				if (num23 > num17)
					num23 = num17;

				num23 = 1f - num23 / num17;
				num22 *= num23;
				num23 = num21 / num18;
				if (num23 > 1f)
					num23 = 1f;

				num22 *= num23;
				if (num22 < 0f)
					num22 = 0f;

				num23 = 1f;
				num22 *= num23;
				if (vector2.Y > 0f) {
					vector2.Y *= 1f + num22;
					vector2.X *= 1f - num22;
				}
				else {
					num23 = Math.Abs(velocity.X) / 3f;
					if (num23 > 1f)
						num23 = 1f;

					num23 -= 0.5f;
					num22 *= num23;
					if (num22 > 0f)
						num22 *= 2f;

					vector2.Y *= 1f + num22;
					vector2.X *= 1f - num22;
				}
			}

			float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
			if (!flag3)
				value4.Height = (int)num19;

			Color color2 = Lighting.GetColor(center.ToTileCoordinates());
            Main.EntitySpriteDraw(value3, vector + new Vector2(Owner.direction * 6f, 2f) - Main.screenPosition, value4, color2, rotation, origin2, 1f, SpriteEffects.None);
		}

		Vector2 value5 = Projectile.Size / 2f;
		float num24 = Math.Abs(Main.WindForVisuals);
		float num25 = MathHelper.Lerp(0.5f, 1f, num24);
		float num26 = num24;
		if (vector2.Y >= -0.02f && vector2.Y < 1f)
			num26 = Utils.GetLerpValue(0.2f, 0.5f, num24, clamped: true);

		int num27 = num4;
		int num28 = num3 + 1;
		for (int i = 0; i < num13; i++) {
			rectangle2.X = width * (num - 1);
			List<Vector2> list = new List<Vector2>();
			Vector2 value6 = new Vector2(num25 * (float)num11 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num26) * 2f;
			float num29 = num7 + num14;
			float num30 = num8 + num15;
			switch (i) {
				case 1:
					value6 = new Vector2(num25 * (float)num11 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num26 + 0.5f) * 2f;
					num29 -= 8f;
					num30 -= 8f;
					break;
				case 2:
					value6 = new Vector2(num25 * (float)num11 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num26 + 1f) * 2f;
					num29 -= 4f;
					num30 -= 4f;
					break;
				case 3:
					value6 = new Vector2(num25 * (float)num11 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num26 + 1.5f) * 2f;
					num29 -= 12f;
					num30 -= 12f;
					break;
			}

			Vector2 value7 = Projectile.Center + new Vector2(((float)rectangle.Width * 0.5f + num29) * (float)Projectile.spriteDirection, num30).RotatedBy(Projectile.rotation + num6);
			list.Add(value7);
			int num31 = num27;
			int num32 = 1;
			while (num31 < num28 * num27) {
				if (num9 != -1 && num9 == num32)
					num5 = num10;

				Vector2 value8 = Projectile.oldPos[num31];
				if (value8.X == 0f && value8.Y == 0f) {
					list.Add(value7);
				}
				else {
					value8 += value5 + new Vector2(((float)rectangle.Width * 0.5f + num29) * (float)Projectile.oldSpriteDirection[num31], num30).RotatedBy(Projectile.oldRot[num31] + num6);
					value8 += value6 * (num32 + 1);
					Vector2 value9 = value7 - value8;
					float num33 = value9.Length();
					if (num33 > num5)
						value9 *= num5 / num33;

					value8 = value7 - value9;
					list.Add(value8);
					value7 = value8;
				}

				num31 += num27;
				num32++;
			}

			if (flag) {
				Rectangle value10 = value3.Frame();
				for (int num34 = list.Count - 2; num34 >= 0; num34--) {
					Vector2 vector3 = list[num34];
					Vector2 v = list[num34 + 1] - vector3;
					float num35 = v.Length();
					if (!(num35 < 2f)) {
						float rotation2 = v.ToRotation() - (float)Math.PI / 2f;
                        Main.EntitySpriteDraw(value3, vector3 - Main.screenPosition, value10, alpha, rotation2, origin2, new Vector2(1f, num35 / (float)value10.Height), SpriteEffects.None);
					}
				}
			}

			for (int num36 = list.Count - 2; num36 >= 0; num36--) {
				Vector2 value11 = list[num36];
				Vector2 value12 = list[num36 + 1];
				Vector2 v2 = value12 - value11;
				v2.Length();
				float rotation3 = v2.ToRotation() - (float)Math.PI / 2f + num2;
                Main.EntitySpriteDraw(value2, value12 - Main.screenPosition, rectangle2, alpha, rotation3, origin3, Projectile.scale, effects);
				rectangle2.X -= width;
				if (rectangle2.X < 0) {
					int num37 = num12;
					if (flag2)
						num37--;

					rectangle2.X = num37 * width;
				}
			}
		}

        Main.EntitySpriteDraw(value, position, rectangle, alpha, Projectile.rotation + num6, origin, Projectile.scale, effects);
    }
    #endregion
    #endregion
}
