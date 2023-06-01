using Consolaria.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class TizonaShoot : ModProjectile {
        public override void SetDefaults () {
            int width = 30; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            //Projectile.aiStyle = 191;

            Projectile.friendly = true;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
        }

        public override void AI () {
            SwingAI();
        }

        private void SwingAI () {
            Player player = Main.player [Projectile.owner];
            //Main.NewText(Projectile.ai [0], Color.Blue);
            //Main.NewText(Projectile.ai [1], Color.Red);
            //Main.NewText(Projectile.ai [2], Color.Green);
           // Projectile.ai [0] += 1F;
            float num = 50f;
            float num2 = 15f;
            float num3 = Projectile.ai [1] + num;
            float num4 = num3 + num2;
            float num5 = 77f;

            if (Projectile.localAI [0] == 0f)
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);

            Projectile.localAI [0] += 1f;
            if (Projectile.damage == 0 && Projectile.localAI [0] < MathHelper.Lerp(num3, num4, 0.5f))
                Projectile.localAI [0] += 6f;

            Projectile.Opacity = Utils.Remap(Projectile.localAI [0], 0f, Projectile.ai [1], 0f, 1f) * Utils.Remap(Projectile.localAI [0], num3, num4, 1f, 0f);
            if (Projectile.localAI [0] >= num4) {
                Projectile.localAI [1] = 1f;
                Projectile.Kill();
                return;
            }

            float fromValue = Projectile.localAI [0] / Projectile.ai [1];
            float num6 = Utils.Remap(Projectile.localAI [0], Projectile.ai [1] * 0.4f, num4, 0f, 1f);
            Projectile.direction = (Projectile.spriteDirection = (int) Projectile.ai [0]);
            int num7 = 3;
            if (Projectile.damage != 0 && Projectile.localAI [0] >= num5 + num7)
                Projectile.damage = 0;

            if (Projectile.damage != 0) {
                int num8 = 80;
                bool flag = false;
                float num9 = Projectile.velocity.ToRotation();
                for (float num10 = -1f; num10 <= 1f; num10 += 0.5f) {
                    Vector2 position = Projectile.Center + (num9 + num10 * ((float) Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * 0.5f * Projectile.scale;
                    Vector2 position2 = Projectile.Center + (num9 + num10 * ((float) Math.PI / 4f) * 0.25f).ToRotationVector2() * num8 * Projectile.scale;
                    if (!Collision.SolidTiles(Projectile.Center, 0, 0) && Collision.CanHit(position, 0, 0, position2, 0, 0)) {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    Projectile.damage = 0;
            }

            fromValue = Projectile.localAI [0] / Projectile.ai [1];
            Projectile.localAI [1] += 1f;
            num6 = Utils.Remap(Projectile.localAI [1], Projectile.ai [1] * 0.4f, num4, 0f, 1f);
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity + Projectile.velocity * num6 * num6 * num5;
            Projectile.rotation += Projectile.ai [0] * ((float) Math.PI * 2f) * (4f + Projectile.Opacity * 4f) / 90f;
            Projectile.scale = Utils.Remap(Projectile.localAI [0], Projectile.ai [1] + 2f, num4, 1.12f, 1f) * Projectile.ai [2];
            float f = Projectile.rotation + Main.rand.NextFloatDirection() * ((float) Math.PI / 2f) * 0.7f;
            Vector2 position3 = Projectile.Center + f.ToRotationVector2() * 84f * Projectile.scale;
            if (Main.rand.NextBool(5)) {
                Dust dust = Dust.NewDustPerfect(position3, 14, null, 150, default, 1.4f);
                dust.noLight = (dust.noLightEmittence = true);
            }

            for (int i = 0; i < 3f * Projectile.Opacity; i++) {
                Vector2 value = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                int num11 = (Main.rand.NextFloat() < Projectile.Opacity) ? 75 : 27;
                Dust dust2 = Dust.NewDustPerfect(position3, num11, Projectile.velocity * 0.2f + value * 3f, 100, default, 1.4f);
                dust2.noGravity = true;
                dust2.customData = Projectile.Opacity * 0.2f;
            }


            Projectile.ownerHitCheck = (Projectile.localAI [0] <= 6f);
            if (Projectile.localAI [0] >= MathHelper.Lerp(num3, num4, 0.65f))
                Projectile.damage = 0;

            float fromValue2 = 1f - (1f - num6) * (1f - num6);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = Utils.Remap(fromValue2, 0f, 1f, 1.5f, 1f) * Projectile.ai [2];
            num6 = Utils.Remap(Projectile.localAI [0], Projectile.ai [1] / 2f, num4, 0f, 1f);
            Projectile.Opacity = Utils.Remap(Projectile.localAI [0], 0f, Projectile.ai [1] * 0.5f, 0f, 1f) * Utils.Remap(Projectile.localAI [0], num4 - 12f, num4, 1f, 0f);
            if (Projectile.velocity.Length() > 8f) {
                Projectile.velocity *= 0.94f;
                new Vector2(32f, 32f);
                float scaleFactor = Utils.Remap(fromValue, 0.7f, 1f, 110f, 110f);
                if (Projectile.localAI [1] == 0f) {
                    bool flag2 = false;
                    for (float num12 = -1f; num12 <= 1f; num12 += 0.5f) {
                        Vector2 position4 = Projectile.Center + (Projectile.rotation + num12 * ((float) Math.PI / 4f) * 0.25f).ToRotationVector2() * scaleFactor * 0.5f * Projectile.scale;
                        Vector2 position5 = Projectile.Center + (Projectile.rotation + num12 * ((float) Math.PI / 4f) * 0.25f).ToRotationVector2() * scaleFactor * Projectile.scale;
                        if (Collision.CanHit(position4, 0, 0, position5, 0, 0)) {
                            flag2 = true;
                            break;
                        }
                    }

                    if (!flag2)
                        Projectile.localAI [1] = 1f;
                }

                if (Projectile.localAI [1] == 1f && Projectile.velocity.Length() > 8f)
                    Projectile.velocity *= 0.8f;

                if (Projectile.localAI [1] == 1f)
                    Projectile.velocity *= 0.88f;
            }

            float num13 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float) Math.PI / 2f) * 0.9f;
            Vector2 position6 = Projectile.Center + num13.ToRotationVector2() * 85f * Projectile.scale;
            (num13 + Projectile.ai [0] * ((float) Math.PI / 2f)).ToRotationVector2();
            Color value2 = new Color(64, 220, 96);
            Color value3 = new Color(15, 84, 125);
            Lighting.AddLight(Projectile.Center + Projectile.rotation.ToRotationVector2() * 85f * Projectile.scale, value2.ToVector3());
            for (int j = 0; j < 3; j++) {
                if (Main.rand.NextFloat() < Projectile.Opacity + 0.1f) {
                    Color.Lerp(Color.Lerp(Color.Lerp(value3, value2, Utils.Remap(fromValue, 0f, 0.6f, 0f, 1f)), Color.White, Utils.Remap(fromValue, 0.6f, 0.8f, 0f, 0.5f)), Color.White, Main.rand.NextFloat() * 0.3f);
                    Dust dust3 = Dust.NewDustPerfect(position6, 107, Projectile.velocity * 0.7f, 100, default(Color) * Projectile.Opacity, 0.8f * Projectile.Opacity);
                    dust3.scale *= 0.7f;
                    dust3.velocity += player.velocity * 0.1f;
                    dust3.position -= dust3.velocity * 6f;
                }
            }

            if (Projectile.damage == 0) {
                Projectile.localAI [0] += 3f;
                Projectile.velocity *= 0.76f;
            }

            if (Projectile.localAI [0] < 10f && (Projectile.localAI [1] == 1f || Projectile.damage == 0)) {
                Projectile.localAI [0] += 1f;
                Projectile.velocity *= 0.85f;
                for (int k = 0; k < 4; k++) {
                    float num14 = Main.rand.NextFloatDirection();
                    float num15 = 1f - Math.Abs(num14);
                    num13 = Projectile.rotation + num14 * ((float) Math.PI / 2f) * 0.9f;
                    position6 = Projectile.Center + num13.ToRotationVector2() * 85f * Projectile.scale;
                    Color.Lerp(Color.Lerp(Color.Lerp(value3, value2, Utils.Remap(fromValue, 0f, 0.6f, 0f, 1f)), Color.White, Utils.Remap(fromValue, 0.6f, 0.8f, 0f, 0.5f)), Color.White, Main.rand.NextFloat() * 0.3f);
                    Dust dust4 = Dust.NewDustPerfect(position6, 107, Projectile.velocity.RotatedBy(num14 * ((float) Math.PI / 4f)) * 0.2f * Main.rand.NextFloat(), 100, default(Color), 1.4f * num15);
                    dust4.velocity += player.velocity * 0.1f;
                    dust4.position -= dust4.velocity * Main.rand.NextFloat() * 3f;
                }
            }
        }

        public override bool? Colliding (Rectangle projHitbox, Rectangle targetHitbox) {
            Vector2 v = targetHitbox.ClosestPointInRect(Projectile.Center) - Projectile.Center;
            v.SafeNormalize(Vector2.UnitX);
            float num2 = 100f * Projectile.scale;
            if (v.Length() < num2 && Collision.CanHit(Projectile.Center, 0, 0, targetHitbox.Center.ToVector2(), 0, 0))
                return true;
            return false;
        }

        public override void ModifyHitNPC (NPC target, ref NPC.HitModifiers modifiers) {
            ParticleOrchestraSettings particleOrchestraSettings;
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
        }

        private void DrawLikeTrueNightsEdge (SpriteBatch spriteBatch) {
            Asset<Texture2D> asset = TextureAssets.Projectile [Projectile.type];
            Rectangle rectangle = asset.Frame(1, 2);
            Vector2 origin = rectangle.Size() / 2f;
            float num = Projectile.scale * 1.1f;
            SpriteEffects effects = (!(Projectile.ai [0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float num2 = 0.975f;
            float fromValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float) Math.Sqrt(3.0);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            float num3 = MathHelper.Min(0.15f + fromValue * 0.85f, Utils.Remap(Projectile.localAI [0], 30f, 96f, 1f, 0f));
            _ = Projectile.Size / 2f;
            float num4 = 2f;
            for (float num5 = num4; num5 >= 0f; num5 -= 1f) {
                if (!(Projectile.oldPos [(int) num5] == Vector2.Zero)) {
                    Vector2 value = Projectile.Center - Projectile.velocity * 0.5f * num5;
                    float num6 = Projectile.oldRot [(int) num5] + Projectile.ai [0] * ((float) Math.PI * 2f) * 0.1f * (0f - num5);
                    Vector2 position = value - Main.screenPosition;
                    float num7 = 1f - num5 / num4;
                    float scale = Projectile.Opacity * num7 * num7 * 0.85f;
                    float amount = Projectile.Opacity * Projectile.Opacity;
                    Color value2 = Color.Lerp(new Color(40, 20, 60, 120), new Color(80, 160, 50, 120), amount);
                    spriteBatch.Draw(asset.Value, position, rectangle, value2 * num3 * scale, num6 + Projectile.ai [0] * ((float) Math.PI / 4f) * -1f, origin, num * num2, effects, 0f);
                    Color value3 = Color.Lerp(new Color(80, 40, 180), new Color(155, 255, 100), amount);
                    Color value4 = Color.White * scale * 0.5f;
                    value4.A = (byte) (value4.A * (1f - num3));
                    Color value5 = value4 * num3 * 0.5f;
                    value5.G = (byte) (value5.G * num3);
                    value5.R = (byte) (value5.R * (0.25f + num3 * 0.75f));
                    float num8 = 3f;
                    for (float num9 = (float) Math.PI * -2f + (float) Math.PI * 2f / num8; num9 < 0f; num9 += (float) Math.PI * 2f / num8) {
                        float scale2 = Utils.Remap(num9, (float) Math.PI * -2f, 0f, 0f, 0.5f);
                        spriteBatch.Draw(asset.Value, position, rectangle, value5 * 0.15f * scale2, num6 + Projectile.ai [0] * 0.01f + num9, origin, num, effects, 0f);
                        spriteBatch.Draw(asset.Value, position, rectangle, Color.Lerp(new Color(80, 30, 160), new Color(200, 255, 0), amount) * fromValue * scale * scale2, num6 + num9, origin, num * 0.8f, effects, 0f);
                        spriteBatch.Draw(asset.Value, position, rectangle, value3 * fromValue * scale * MathHelper.Lerp(0.05f, 0.4f, fromValue) * scale2, num6 + num9, origin, num * num2, effects, 0f);
                        spriteBatch.Draw(asset.Value, position, asset.Frame(1, 2, 0, 1), Color.White * MathHelper.Lerp(0.05f, 0.5f, fromValue) * scale * scale2, num6 + num9, origin, num, effects, 0f);
                    }

                    spriteBatch.Draw(asset.Value, position, rectangle, value5 * 0.15f, num6 + Projectile.ai [0] * 0.01f, origin, num, effects, 0f);
                    spriteBatch.Draw(asset.Value, position, rectangle, Color.Lerp(new Color(80, 30, 160), new Color(200, 255, 0), amount) * num3 * scale, num6, origin, num * 0.8f, effects, 0f);
                    spriteBatch.Draw(asset.Value, position, rectangle, value3 * fromValue * scale * MathHelper.Lerp(0.05f, 0.4f, num3), num6, origin, num * num2, effects, 0f);
                    spriteBatch.Draw(asset.Value, position, asset.Frame(1, 2, 0, 1), Color.White * MathHelper.Lerp(0.05f, 0.5f, num3) * scale, num6, origin, num, effects, 0f);
                }
            }

            float num10 = 1f - Projectile.localAI [0] * 1f / 80f;
            if (num10 < 0.5f)
                num10 = 0.5f;

            Vector2 drawpos = Projectile.Center - Main.screenPosition + (Projectile.rotation + 213f / 452f * Projectile.ai [0]).ToRotationVector2() * (asset.Width() * 0.5f - 4f) * num * num10;
            float scale3 = MathHelper.Min(num3, MathHelper.Lerp(1f, fromValue, Utils.Remap(Projectile.localAI [0], 0f, 80f, 0f, 1f)));
            DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * Projectile.Opacity * 0.5f * scale3, new Color(150, 255, 100) * scale3, Projectile.Opacity, 0f, 1f, 1f, 2f, (float) Math.PI / 4f, new Vector2(2f, 2f), Vector2.One);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            DrawLikeTrueNightsEdge(spriteBatch);
            return false;
        }
    }
}