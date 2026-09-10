using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed class EternalHorrorLaser1 : ModProjectile {
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
    }

    public override void SetDefaults() {
        Projectile.CloneDefaults(ProjectileID.EyeLaser);
        AIType = ProjectileID.EyeLaser;

        Projectile.hostile = true;
        Projectile.tileCollide = false;

        Projectile.scale = 1f;
        Projectile.alpha = 255;

        Projectile.width = 6;

        Projectile.timeLeft = 900;
        Projectile.penetrate = -1;

        Projectile.light = 0.1f;
    }

    public override void AI() {
        if (Projectile.timeLeft <= 895) Projectile.alpha = 50;
        Lighting.AddLight(Projectile.Center, 0.6f, 0.1f, 0.1f);
    }

    public override bool PreDraw(ref Color lightColor) {
        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
        Vector2 position = Projectile.Center;
        Rectangle clip = texture.Bounds;
        Color drawColor = Color.Lerp(lightColor, Color.White, 0.5f);
        float rotation = Projectile.rotation;
        SpriteEffects flip = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Vector2 screenPos = Main.screenPosition;

        SpriteBatch spriteBatch = Main.spriteBatch;

        Vector2 drawOrigin = clip.Centered();

        for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
            Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            position = drawPos;
            rotation = (float)Math.Atan2(Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y, Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X);
            EternalHorror.DrawContext drawContext = new(spriteBatch, position, texture, clip, drawColor, rotation, flip, screenPos);
            EternalHorror.DrawUnderShadowEffect(drawContext, (newPosition, newColor) => {
                Color color = new Color(60 + k * 4, 20 - k, 10 + k * 4, 60 + k * 4);
                color = color.MultiplyRGBA(newColor);
                spriteBatch.Draw(drawContext.Texture, newPosition, null, color, drawContext.Rotation, drawOrigin, (Projectile.scale - k / (float)Projectile.oldPos.Length) * 0.75f, drawContext.Flip, 0f);
                spriteBatch.Draw(drawContext.Texture, newPosition - Projectile.oldPos[k] * 0.5f + Projectile.oldPos[k + 1] * 0.5f, null, color, drawContext.Rotation, drawOrigin, (Projectile.scale - k / (float)Projectile.oldPos.Length) * 0.75f, drawContext.Flip, 0f);
            }, sinWaveOffset: Projectile.identity + MathHelper.Pi,
                applyInnerOpacity: false,
                forcedOpacity: MathHelper.Lerp(0.125f, 0.25f, 0f),
                sinWaveOffset_BasedOnEffectIndex: MathHelper.TwoPi * 0.25f);
        }

        return false;
    }

    public override Color? GetAlpha(Color lightColor)
        => new Color(255, 255, 255, 200);
}
