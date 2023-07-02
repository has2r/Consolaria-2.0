using Consolaria.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class Tizona : ModProjectile {
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
        }

        public override void AI () 
            => SwingAI();
        

        private void SwingAI () {
            Player player = Main.player [Projectile.owner];
            Projectile.localAI [0] += 1f;
            float num = Projectile.localAI [0] / Projectile.ai [1];
            // can be changed for funny
            float num2 = 0.8f;
            float num3 = 1f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num3 + num * num2;

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
        }

        public override void ModifyHitNPC (NPC target, ref NPC.HitModifiers modifiers) {
            ParticleOrchestraSettings particleOrchestraSettings;
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            if (info.PvP)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }

        private void DrawLikeExcalibur (SpriteBatch spriteBatch) {
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Rectangle rectangle = texture.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = Projectile.scale * 1.1f;
            SpriteEffects effects = (!(Projectile.ai [0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float num2 = Projectile.localAI [0] / Projectile.ai [1];
            float num3 = Utils.Remap(num2, 0f, 0.5f, 0f, 1f) * Utils.Remap(num2, 0.5f, 1f, 1f, 0f);
            float num4 = 0.975f;
            float amount = num3;
            float fromValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float) Math.Sqrt(3.0);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Color value = new Microsoft.Xna.Framework.Color(142, 30, 255, 200);
            Color value2 = new Microsoft.Xna.Framework.Color(75, 15, 255, 70);
            Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Lerp(value2, value, 0.25f);
            Microsoft.Xna.Framework.Color color4 = new Microsoft.Xna.Framework.Color(65, 26, 84, 150);
            spriteBatch.Draw(texture, vector, rectangle, value * fromValue * num3, Projectile.rotation + Projectile.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num, effects, 0f);
            Color value3 = Color.White * num3 * 0.5f;
            value3.A = (byte) (value3.A * (1f - fromValue));
            Color value4 = value3 * fromValue * 0.5f;
            value4.G = (byte) (value4.G * fromValue);
            value4.B = (byte) (value4.R * (0.25f + fromValue * 0.75f));
            spriteBatch.Draw(texture, vector, rectangle, value4 * 0.15f, Projectile.rotation + Projectile.ai [0] * 0.01f, origin, num, effects, 0f);
            spriteBatch.Draw(texture, vector, rectangle, color * fromValue * num3 * 0.3f, Projectile.rotation, origin, num, effects, 0f);
            spriteBatch.Draw(texture, vector, rectangle, value2 * fromValue * num3 * 0.5f, Projectile.rotation, origin, num * num4, effects, 0f);
            spriteBatch.Draw(texture, vector, texture.Frame(1, 4, 0, 3), value * 0.6f * num3, Projectile.rotation + Projectile.ai [0] * 0.01f, origin, num, effects, 0f);
            spriteBatch.Draw(texture, vector, texture.Frame(1, 4, 0, 3), value2 * 0.5f * num3, Projectile.rotation + Projectile.ai [0] * -0.05f, origin, num * 0.8f, effects, 0f);
            spriteBatch.Draw(texture, vector, texture.Frame(1, 4, 0, 3), color4 * 0.4f * num3, Projectile.rotation + Projectile.ai [0] * -0.1f, origin, num * 0.6f, effects, 0f);
            float scaleFactor = num * 0.75f;
            for (float num5 = 0f; num5 < 12f; num5 += 1f) {
                float num6 = Projectile.rotation + Projectile.ai [0] * num5 * ((float) Math.PI * -2f) * 0.025f + Utils.Remap(num2, 0f, 0.6f, 0f, 0.9f) * Projectile.ai [0];
                Vector2 drawpos = vector + num6.ToRotationVector2() * (texture.Width * 0.5f - 6f) * num;
                float scale = num5 / 12f;
                DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos, Color.Lerp(value, color4, num2) * num3 * 2f * scale, color, num2, 0f, 0.5f, 0.5f, 1f, num6, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * scaleFactor, Vector2.One * scaleFactor);
            }
            Vector2 drawpos2 = vector + (Projectile.rotation + Utils.Remap(num2, 0f, 0.6f, 0f, 0.9f) * Projectile.ai [0]).ToRotationVector2() * (texture.Width * 0.5f - 4f) * num;
            DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos2, Color.Lerp(value, color4, num2) * num3 * 1.5f, color, num2, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f)) * scaleFactor, Vector2.One * scaleFactor);
        }
        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            DrawLikeExcalibur(spriteBatch);
            return false;
        }
    }
}