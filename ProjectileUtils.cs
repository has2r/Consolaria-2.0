using Consolaria;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace RoA.Core.Utility.Vanilla;

static class ProjectileUtils {
    public static void QuickDraw(this Projectile projectile, Color lightColor, float exRot = 0f, Texture2D? texture = null, SpriteEffects? spriteEffects = null, Rectangle? sourceRectangle = null, Vector2? origin = null, Vector2? scale = null) {
        Texture2D mainTex = texture ?? projectile.GetTexture();

        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();

        if (scale != null) {
            Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition/* + Vector2.UnitY * projectile.gfxOffY*/, sourceRectangle, lightColor, projectile.rotation + exRot,
            origin ?? mainTex.Size() / 2, projectile.scale * scale.Value, effects, 0);
            return;
        }
        Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition/* + Vector2.UnitY * projectile.gfxOffY*/, sourceRectangle, lightColor, projectile.rotation + exRot,
            origin ?? mainTex.Size() / 2, projectile.scale, effects, 0);
    }

    public static void QuickDraw(this Projectile projectile, Color lightColor, float overrideRot, int EmmmItIsAStupidValue) {
        Texture2D mainTex = projectile.GetTexture();

        Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition/* + Vector2.UnitY * projectile.gfxOffY*/, null, lightColor, overrideRot,
            mainTex.Size() / 2, projectile.scale, 0, 0);
    }

    public static void QuickDraw(this Projectile projectile, Rectangle frameBox, Color lightColor, float exRot) {
        Texture2D mainTex = projectile.GetTexture();

        Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition /*+ Vector2.UnitY * projectile.gfxOffY*/, frameBox, lightColor, projectile.rotation + exRot,
            frameBox.Size() / 2, projectile.scale, 0, 0);
    }

    public static void QuickDrawAnimated(this Projectile projectile, Color lightColor, float exRot = 0f, Texture2D? texture = null, byte maxFrames = 0, Vector2? scale = null, float? scale_float = null, Vector2? origin = null, Vector2? originScale = null, SpriteEffects? spriteEffects = null, Rectangle? frameBox = null) {
        Texture2D mainTex = texture ?? projectile.GetTexture();

        int frameSize = mainTex.Height / (maxFrames != 0 ? maxFrames : Main.projFrames[projectile.type]);
        frameBox ??= new(0, frameSize * projectile.frame, mainTex.Width, frameSize);
        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();
        origin ??= frameBox.Value.Size() / 2;
        scale_float ??= projectile.scale;
        if (originScale != null) {
            origin *= originScale;
        }
        if (scale != null) {
            Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition/* + Vector2.UnitY * projectile.gfxOffY*/, frameBox, lightColor, projectile.rotation + exRot,
                origin.Value, scale.Value, effects, 0);
        }
        else {
            Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition /*+ Vector2.UnitY * projectile.gfxOffY*/, frameBox, lightColor, projectile.rotation + exRot,
                origin.Value, scale_float.Value, effects, 0);
        }
    }

    public static void QuickDrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, int start, float extraRot = 0, float scale = -1, Texture2D? texture = null, byte maxFrames = 0, SpriteEffects? spriteEffects = null, Vector2? origin = null, Rectangle? clip = null) {
        int howMany = projectile.oldPos.Length;
        projectile.DrawShadowTrails(drawColor, maxAlpha, maxAlpha / howMany, start, howMany, 1, extraRot, scale, texture, maxFrames, spriteEffects, origin, clip);
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float extraRot = 0, float scale = -1, Texture2D? texture = null, byte maxFrames = 0, SpriteEffects? spriteEffects = null, Vector2? origin = null, Rectangle? clip = null) {
        Texture2D mainTex = texture ?? TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        int frameSize = mainTex.Height / (maxFrames != 0 ? maxFrames : Main.projFrames[projectile.type]);
        Rectangle frameBox = new(0, frameSize * projectile.frame, mainTex.Width, frameSize);
        if (clip != null) {
            frameBox = clip.Value;
        }
        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();
        origin ??= frameBox.Size() / 2;

        for (int i = start; i < howMany; i += step) {
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin.Value, scale == -1 ? projectile.scale : scale, effects, 0);
        }
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float scaleStep, float extraRot = 0, float scale = -1, byte maxFrames = 0, SpriteEffects? spriteEffects = null, Vector2? origin = null) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        int frameSize = mainTex.Height / (maxFrames != 0 ? maxFrames : Main.projFrames[projectile.type]);
        Rectangle frameBox = new(0, frameSize * projectile.frame, mainTex.Width, frameSize);
        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();
        origin ??= frameBox.Size() / 2;

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin.Value, (scale == -1 ? projectile.scale : scale) * (1 - (i * scaleStep)), effects, 0);
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float scaleStep, Rectangle frameBox, float extraRot = 0, float scale = -1) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);
        var origin = frameBox.Size() / 2;

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin, (scale == -1 ? projectile.scale : scale) * (1 - (i * scaleStep)), 0, 0);
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, Vector2 scale, float extraRot = 0, byte maxFrames = 0, SpriteEffects? spriteEffects = null, Vector2? origin = null) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        int frameSize = mainTex.Height / (maxFrames != 0 ? maxFrames : Main.projFrames[projectile.type]);
        Rectangle frameBox = new(0, frameSize * projectile.frame, mainTex.Width, frameSize);
        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();
        origin ??= frameBox.Size() / 2;

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin.Value, scale, effects, 0);
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, Vector2 scale, float scaleStep, float extraRot = 0, byte maxFrames = 0, SpriteEffects? spriteEffects = null, Vector2? origin = null) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        int frameSize = mainTex.Height / (maxFrames != 0 ? maxFrames : Main.projFrames[projectile.type]);
        Rectangle frameBox = new(0, frameSize * projectile.frame, mainTex.Width, frameSize);
        SpriteEffects effects = spriteEffects ?? projectile.spriteDirection.ToSpriteEffects();
        origin ??= frameBox.Size() / 2;

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin.Value, scale * (1 - (i * scaleStep)), effects, 0);
    }

    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float scale, Rectangle frameBox, float extraRot) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, frameBox.Size() / 2, scale, 0, 0);
    }

    public static void DrawShadowTrailsSacleStep(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float scaleStep, Rectangle frameBox, float extraRot = 0, float scale = -1) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, frameBox.Size() / 2, (scale == -1 ? projectile.scale : scale) * (1 - (i * scaleStep)), 0, 0);
    }

    public static void DrawShadowTrailsSacleStep(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, float scaleStep, Rectangle? frameBox, SpriteEffects effect, float extraRot = 0, float scale = -1) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);
        Vector2 origin = frameBox.HasValue ? frameBox.Value.Size() / 2 : mainTex.Size() / 2;

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, origin, (scale == -1 ? projectile.scale : scale) * (1 - (i * scaleStep)), effect, 0);
    }


    public static void DrawShadowTrails(this Projectile projectile, Color drawColor, float maxAlpha, float alphaStep, int start, int howMany, int step, Vector2 scale, Rectangle frameBox, float extraRot = 0) {
        Texture2D mainTex = TextureAssets.Projectile[projectile.type].Value;
        Vector2 toCenter = new(projectile.width / 2, projectile.height / 2);

        for (int i = start; i < howMany; i += step)
            Main.spriteBatch.Draw(mainTex, projectile.oldPos[i] + toCenter - Main.screenPosition, frameBox,
                drawColor * (maxAlpha - (i * alphaStep)), projectile.oldRot[i] + extraRot, frameBox.Size() / 2, scale, 0, 0);
    }

    public static bool CanProjectileCutTiles(Projectile checkProjectile) {
        if (ProjectileLoader.CanCutTiles(checkProjectile) is bool modResult)
            return modResult;

        if (checkProjectile.aiStyle != 45 && checkProjectile.aiStyle != 137 && checkProjectile.aiStyle != 92 && checkProjectile.aiStyle != 105 && checkProjectile.aiStyle != 106 && !ProjectileID.Sets.IsAGolfBall[checkProjectile.type] && checkProjectile.type != 463 && checkProjectile.type != 69 && checkProjectile.type != 70 && checkProjectile.type != 621 && checkProjectile.type != 10 && checkProjectile.type != 11 && checkProjectile.type != 379 && checkProjectile.type != 407 && checkProjectile.type != 476 && checkProjectile.type != 623 && (checkProjectile.type < 625 || checkProjectile.type > 628) && checkProjectile.type != 833 && checkProjectile.type != 834 && checkProjectile.type != 835 && checkProjectile.type != 818 && checkProjectile.type != 831 && checkProjectile.type != 820 && checkProjectile.type != 864 && checkProjectile.type != 970 && checkProjectile.type != 995 && checkProjectile.type != 908)
            return checkProjectile.type != 1020;

        return false;
    }

    public static void CutTiles(Projectile projectileThatCuts) {
        if (CanProjectileCutTiles(projectileThatCuts))
            return;

        int owner = projectileThatCuts.owner;
        AchievementsHelper.CurrentlyMining = true;
        bool[] tileCutIgnorance = Main.player[owner].GetTileCutIgnorance(allowRegrowth: false, projectileThatCuts.trap);
        DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
        DelegateMethods.tileCutIgnore = tileCutIgnorance;

        CutTilesAt(projectileThatCuts, projectileThatCuts.position, projectileThatCuts.width, projectileThatCuts.height);

        AchievementsHelper.CurrentlyMining = false;
    }

    public static void CutTilesAt(Projectile projectileThatCuts, Vector2 checkBoxPosition, int checkBoxWidth, int checkBoxHeight) {
        int num = (int)(checkBoxPosition.X / 16f);
        int num2 = (int)((checkBoxPosition.X + (float)checkBoxWidth) / 16f) + 1;
        int num3 = (int)(checkBoxPosition.Y / 16f);
        int num4 = (int)((checkBoxPosition.Y + (float)checkBoxHeight) / 16f) + 1;
        if (num < 0)
            num = 0;

        if (num2 > Main.maxTilesX)
            num2 = Main.maxTilesX;

        if (num3 < 0)
            num3 = 0;

        if (num4 > Main.maxTilesY)
            num4 = Main.maxTilesY;

        bool[] tileCutIgnorance = Main.player[projectileThatCuts.owner].GetTileCutIgnorance(allowRegrowth: false, projectileThatCuts.trap);
        for (int i = num; i < num2; i++) {
            for (int j = num3; j < num4; j++) {
                if (Main.tile[i, j] != null && Main.tileCut[Main.tile[i, j].TileType] && !tileCutIgnorance[Main.tile[i, j].TileType] && WorldGen.CanCutTile(i, j, TileCuttingContext.AttackProjectile)) {
                    WorldGen.KillTile(i, j);
                    if (Main.netMode != 0)
                        NetMessage.SendData(17, -1, -1, null, 0, i, j);
                    // Extra patch context.
                }
            }
        }

        ProjectileLoader.CutTiles(projectileThatCuts);
    }

    public static void DrawSpearProjectile(Projectile projectile, Texture2D? texture = null, Texture2D? glowMaskTexture = null) {
        Projectile proj = projectile;
        texture ??= TextureAssets.Projectile[projectile.type].Value;
        SpriteEffects dir = SpriteEffects.None;
        float num = (float)Math.Atan2(proj.velocity.Y, proj.velocity.X) + 2.355f;
        Player player = Main.player[proj.owner];
        Microsoft.Xna.Framework.Rectangle value = texture.Frame();
        Microsoft.Xna.Framework.Rectangle rect = proj.getRect();
        Vector2 vector = Vector2.Zero;
        if (player.direction > 0) {
            dir = SpriteEffects.FlipHorizontally;
            vector.X = texture.Width;
            num -= (float)Math.PI / 2f;
        }

        if (player.gravDir == -1f) {
            if (proj.direction == 1) {
                dir = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                vector = new Vector2(texture.Width, texture.Height);
                num -= (float)Math.PI / 2f;
            }
            else if (proj.direction == -1) {
                dir = SpriteEffects.FlipVertically;
                vector = new Vector2(0f, texture.Height);
                num += (float)Math.PI / 2f;
            }
        }

        Vector2.Lerp(vector, value.Center.ToVector2(), 0.25f);
        float num2 = 0f;
        Vector2 vector2 = proj.Center;
        Color color = Lighting.GetColor((int)proj.Center.X / 16, (int)proj.Center.Y / 16);
        Main.EntitySpriteDraw(texture, vector2 - Main.screenPosition, value, color, num, vector, proj.scale, dir);
        color = Color.White * 0.9f * (1f - proj.alpha / 255f);

        if (projectile.type == ProjectileID.MushroomSpear) {
            DelegateMethods.v3_1 = new Vector3(0.1f, 0.4f, 1f);
            Utils.PlotTileLine(vector2, vector2 + Vector2.UnitY.RotatedBy(num) * value.Width, 4, DelegateMethods.CastLightOpen);
        }

        if (glowMaskTexture != null) {
            Main.EntitySpriteDraw(glowMaskTexture, vector2 - Main.screenPosition, value, color, num, vector, proj.scale, dir);
        }
    }
}
