using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed class EternalHorrorLaser1 : ModProjectile {
    public ref float ReflectedValue => ref Projectile.ai[2];

    public bool Reflected {
        get => ReflectedValue != 0f;
        set => ReflectedValue = value.ToInt();
    }

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

        ReflectFromEternalHorrorClones();
    }

    private void ReflectFromEternalHorrorClones() {
        if (Reflected) {
            return;
        }

        foreach (NPC npc in Main.ActiveNPCs) {
            if (npc.type != EternalHorror.SelfType) {
                return;
            }

            EternalHorror boss = npc.As<EternalHorror>();
            HashSet<EternalHorror.CloneInfo> cloneData = boss.GetActiveCloneData();
            float bossRotation = npc.rotation;
            Player bossTarget = npc.GetTargetPlayer();
            Vector2 bossTargetCenter = bossTarget.Center + bossTarget.velocity * Projectile.velocity.Length() / 2f;
            foreach (EternalHorror.CloneInfo cloneInfo in cloneData) {
                if (cloneInfo.Opacity < 0.5f) {
                    continue;
                }
                Rectangle hitbox = Projectile.Hitbox;
                Vector2 clonePosition = cloneInfo.VisualPosition;
                float cloneRotation = cloneInfo.Rotation;
                Vector2 cloneDirection = Vector2.UnitY.RotatedBy(cloneRotation);
                Vector2 clonePosition_Start = clonePosition + cloneDirection * npc.height / 2f,
                        clonePosition_End = clonePosition + -cloneDirection * npc.height / 2f;
                float collisionPoint = 0f;
                if (Collision.CheckAABBvLineCollision(hitbox.Location.ToVector2(), hitbox.Size(), clonePosition_Start, clonePosition_End, npc.width, ref collisionPoint)) {
                    Projectile.velocity = Projectile.Center.DirectionTo(bossTargetCenter) * Projectile.velocity.Length();

                    Reflected = true;
                    return;
                }
            }
        }
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
            EternalHorror.DrawUnderGlowEffect(drawContext, (newPosition, newColor) => {
                Color color = Reflected ? new Color(60 - k * 5, 10, 60 + k * 4, 40 + k * 4) : new Color(60 + k * 4, 20 - k, 10 + k * 4, 60 + k * 4);
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
