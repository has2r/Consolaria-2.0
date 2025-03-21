using Consolaria.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class TizonaProjectile : ModProjectile {
        public override void SetStaticDefaults ()
            => Main.projFrames [Type] = 4;

        public override void SetDefaults () {
            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 190;

            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.noEnchantmentVisuals = true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            Projectile.localAI [0]++;
            float percentageOfLife = Projectile.localAI [0] / Projectile.ai [1];
            float scaleMulti = 0.8f;
            float scaleAdder = 1f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = scaleAdder + percentageOfLife * scaleMulti;

            float offset = Projectile.rotation + Main.rand.NextFloatDirection() * ((float) Math.PI / 2f) * 0.7f;
            Vector2 position = Projectile.Center + offset.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 velocity = (offset + Projectile.ai [0] * ((float) Math.PI / 2f)).ToRotationVector2();
            if (Main.rand.NextFloat() < Projectile.Opacity) {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 27, velocity * 1f, 50, new Color(200, 191, 231), 0.4f);
                dust.fadeIn = 0.3f + Main.rand.NextFloat() * 0.15f;
                dust.noGravity = true;
            }

            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity) {
                Dust dust2 = Dust.NewDustPerfect(position, 27, velocity * 1.5f, 100, new Color(200, 191, 231) * Projectile.Opacity, Projectile.Opacity);
                dust2.noGravity = true;
            }

            if (Projectile.localAI [0] >= Projectile.ai [1])
                Projectile.Kill();

            Projectile.scale *= Projectile.ai [2];

            for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2) {
                Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
            }
        }

        public override void OnHitNPC (NPC target, NPC.HitInfo hit, int damageDone) {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge,
            new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
            hit.HitDirection = (Main.player [Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
            target.AddBuff(BuffID.ShadowFlame, 180);
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo info) {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge,
            new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) }, Projectile.owner);
            info.HitDirection = (Main.player [Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
            if (info.PvP) target.AddBuff(BuffID.ShadowFlame, 180);
        }

        private void DrawLikeExcalibur (SpriteBatch spriteBatch) {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Rectangle sourceRectangle = texture.Frame(1, Main.projFrames [Type]);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float projectileScale = Projectile.scale * 1.1f;
            SpriteEffects effects = (!(Projectile.ai [0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float percentageOfLife = Projectile.localAI [0] / Projectile.ai [1];
            float lerpTime = Utils.Remap(percentageOfLife, 0f, 0.5f, 0f, 1f) * Utils.Remap(percentageOfLife, 0.5f, 1f, 1f, 0f);
            float lightningValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float) Math.Sqrt(3.0);
            lightningValue = Utils.Remap(lightningValue, 0.2f, 1f, 0f, 1f);
            Color value = Color.Lerp(new Color(80, 70, 210, 220), new Color(180, 60, 140, 220), lerpTime); //last part
            spriteBatch.Draw(texture, position, sourceRectangle, value * lightningValue * lerpTime, Projectile.rotation + Projectile.ai [0] * ((float) Math.PI / 4f) * -1f * (1f - percentageOfLife), origin, projectileScale, effects, 0f);
            Color value2 = Color.Lerp(new Color(210, 60, 80, 220), new Color(70, 30, 240, 220), lerpTime) * 1.25f; //central part
            Color color = Color.Lerp(new Color(80, 70, 210, 220), new Color(181, 106, 255, 220), lerpTime) * 1.25f; //main color
            Color value3 = Color.White * lerpTime * 0.5f;
            value3.A = (byte) (value3.A * (1f - lightningValue));
            Color value4 = value3 * lightningValue * 0.5f;
            value4.G = (byte) (value4.G * lightningValue);
            value4.B = (byte) (value4.R * (0.25f + lightningValue * 0.75f));
            spriteBatch.Draw(texture, position, sourceRectangle, value4 * 0.15f, Projectile.rotation + Projectile.ai [0] * 0.01f, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, sourceRectangle, color * lightningValue * lerpTime * 0.3f, Projectile.rotation, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, sourceRectangle, value2 * lightningValue * lerpTime * 0.5f, Projectile.rotation, origin, projectileScale * 0.975f, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * lerpTime, Projectile.rotation + Projectile.ai [0] * 0.01f, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * lerpTime, Projectile.rotation + Projectile.ai [0] * -0.05f, origin, projectileScale * 0.8f, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * lerpTime, Projectile.rotation + Projectile.ai [0] * -0.1f, origin, projectileScale * 0.6f, effects, 0f);
            float scaleFactor = projectileScale * 0.75f;
            for (float i = 0f; i < 12f; i += 1f) {
                float edgeRotation = Projectile.rotation + Projectile.ai [0] * i * ((float) Math.PI * -2f) * 0.025f + Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 0.9f) * Projectile.ai [0];
                Vector2 drawPos = position + edgeRotation.ToRotationVector2() * (texture.Width * 0.5f - 6f) * projectileScale;
                float scale = i / 12f;
                DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos, Color.Lerp(new Color(255, 190, 190, 0), new Color(190, 180, 255, 0), percentageOfLife) * lerpTime * 2f * scale, color, percentageOfLife, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(0f, Utils.Remap(percentageOfLife, 0f, 1f, 3f, 0f)) * scaleFactor, Vector2.One * scaleFactor);
            }
            Vector2 drawpos2 = position + (Projectile.rotation + Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 0.9f) * Projectile.ai [0]).ToRotationVector2() * (texture.Width * 0.5f - 4f) * projectileScale;
            DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos2, Color.Lerp(new Color(255, 190, 190, 0), new Color(190, 180, 255, 0), percentageOfLife) * lerpTime * 1.5f, color, percentageOfLife, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(percentageOfLife, 0f, 1f, 4f, 1f)) * scaleFactor, Vector2.One * scaleFactor);
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            DrawLikeExcalibur(spriteBatch);
            return false;
        }
    }
}