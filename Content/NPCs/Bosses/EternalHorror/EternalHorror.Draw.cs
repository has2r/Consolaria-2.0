using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public readonly record struct DrawContext(SpriteBatch SpriteBatch, Vector2 Position, Texture2D Texture, Rectangle Clip, Color DrawColor, float Rotation, SpriteEffects Flip, Vector2 ScreenPosition);

    private static Asset<Texture2D> _eyeTexture = null!,
                                    _glowTexture = null!,
                                    _shadowTexture = null!,
                                    _backgroundTexture = null!;

    private float _glowOpacity;

    private float WaveOffset => NPC.whoAmI;

    private partial void Load_Textures() {
        _eyeTexture = Helper.RequestTexture(Texture + "_Eyes");
        _glowTexture = Helper.RequestTexture(Texture + "_Glow");
        _shadowTexture = Helper.RequestTexture(Texture + "_Shadow");
        _backgroundTexture = Helper.RequestTexture(Texture + "_Background");
    }

    public static Color MainPurpleColor => new(175, 85, 255);
    public static Color MainPurpleColor_Dynamic => Color.Lerp(new(175, 85, 255), Color.Lerp(new(198, 123, 173), new(131, 186, 64), 0.5f), Helper.Wave(0f, 1f, 1f, 0f));

    public static Color MainRedColor_Dynamic => Color.Lerp(new(255, 10, 25), MainPurpleColor_Dynamic, Helper.Wave(0f, 1f, 25f, 0f) * 0.25f);

    public override void FindFrame(int frameHeight) {
        int phase1LastFrame = 3;
        void playPhase1IdleAnimation() {
            int frame = NPC.GetCurrentFrame(frameHeight);
            ref double frameCounter = ref NPC.frameCounter;
            int frameTime = 6;
            if (++frameCounter >= frameTime) {
                frameCounter = 0;
                frame++;
                if (frame >= phase1LastFrame) {
                    frame = 0;
                }
            }
            NPC.SetCurrentFrame(frame, frameHeight);
        }

        playPhase1IdleAnimation();
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        Draw(spriteBatch, screenPos, drawColor);

        return false;
    }

    private void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        UpdateVisuals();

        drawColor = NPC.GetNPCColorTintedByBuffs(npcColor: drawColor);
        drawColor = Color.Lerp(drawColor, Color.White, 0.5f);
        Texture2D texture = NPC.GetTexture(),
                  glowTexture = _glowTexture.Value;
        SpriteEffects flip = (-NPC.spriteDirection).ToSpriteEffects();
        Vector2 position = NPC.Center;
        float rotation = NPC.rotation;
        Rectangle clip = NPC.frame,
                  glowClip = glowTexture.Bounds;

        DrawContext drawContext = new(spriteBatch, position, texture, clip, drawColor, rotation, flip, screenPos);

        void drawSelf() {
            Draw_Inner(drawContext);
        }
        void drawGlowingEyes() {
            Texture2D eyesTexture = _eyeTexture.Value;
            drawContext = drawContext with { Texture = eyesTexture };
            DrawUnderShadowEffect(drawContext, draw: (newPosition, newColor) => {
                Draw_Inner(drawContext with {
                    Position = newPosition,
                    DrawColor = newColor,
                });
            }, sinWaveOffset: WaveOffset);
        }
        Color getLaserGlowColor(Color color) => GetLaserGlowColor(color) * _glowOpacity;
        void drawLaserLine() {
            float rotation = Phase1LaserRotation;

            Vector2 vector = NPC.Center - Main.screenPosition;
            int num = 40;
            int num2 = 180 * num;
            num2 /= 2;
            Microsoft.Xna.Framework.Color color = MainRedColor_Dynamic;
            Microsoft.Xna.Framework.Color color2 = color;
            color.A = 0;
            color2.A /= 2;
            Texture2D value = TextureAssets.Extra[ExtrasID.FairyQueenLance].Value;
            Vector2 origin = value.Frame().Size() * new Vector2(0f, 0.5f);
            Vector2 scale = new Vector2(num2 / value.Width, 2f);
            Vector2 scale2 = new Vector2((float)(num2 / value.Width) * 0.5f, 2f);
            Color color3 = color;
            //spriteBatch.Draw(value, vector, null, color3, rotation, origin, scale2, SpriteEffects.None, 0f);
            //spriteBatch.Draw(value, vector, null, color3 * 0.3f, rotation, origin, scale, SpriteEffects.None, 0f);
            //Microsoft.Xna.Framework.Color color3 = color * Utils.GetLerpValue(60f, 55f, proj.localAI[0], clamped: true) * Utils.GetLerpValue(0f, 10f, proj.localAI[0], clamped: true);
            drawContext = new DrawContext(spriteBatch, vector, value, value.Bounds, color3, rotation, drawContext.Flip, Main.screenPosition);
            DrawUnderShadowEffect(drawContext, draw: (newPosition, newColor) => {
                newColor = getLaserGlowColor(newColor);
                newColor *= Phase1LaserAttackProgress;
                spriteBatch.Draw(drawContext.Texture, newPosition, null, newColor, drawContext.Rotation, origin, scale2, drawContext.Flip, 0f);
                spriteBatch.Draw(drawContext.Texture, newPosition, null, newColor * 0.3f, drawContext.Rotation, origin, scale, drawContext.Flip, 0f);
            }, sinWaveOffset: WaveOffset + MathHelper.Pi,
               applyInnerOpacity: false,
               forcedOpacity: MathHelper.Lerp(0.125f, 0.25f, 1f),
               sinWaveOffset_BasedOnEffectIndex: MathHelper.TwoPi * 0.25f,
               sinStep: AICounter);
            //Texture2D value2 = TextureAssets.Projectile[proj.type].Value;
            //Vector2 origin2 = value2.Frame().Size() / 2f;
            //Microsoft.Xna.Framework.Color color4 = Microsoft.Xna.Framework.Color.White * Utils.GetLerpValue(0f, 20f, proj.localAI[0], clamped: true);
            //color4.A /= 2;
            //float num3 = MathHelper.Lerp(0.7f, 1f, Utils.GetLerpValue(55f, 60f, proj.localAI[0], clamped: true));
            //float lerpValue = Utils.GetLerpValue(10f, 60f, proj.localAI[0]);
            //if (lerpValue > 0f) {
            //    float lerpValue2 = Utils.GetLerpValue(0f, 1f, proj.velocity.Length(), clamped: true);
            //    for (float num4 = 1f; num4 > 0f; num4 -= 1f / 6f) {
            //        Vector2 vector2 = rotation.ToRotationVector2() * -120f * num4 * lerpValue2;
            //        spriteBatch.Draw(value2, vector + vector2, null, color * lerpValue * (1f - num4), rotation, origin2, num3, SpriteEffects.None, 0f);
            //        spriteBatch.Draw(value2, vector + vector2, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.15f * lerpValue * (1f - num4), rotation, origin2, num3 * 0.85f, SpriteEffects.None, 0f);
            //    }

            //    for (float num5 = 0f; num5 < 1f; num5 += 0.25f) {
            //        Vector2 vector3 = (num5 * ((float)Math.PI * 2f) + rotation).ToRotationVector2() * 2f * num3;
            //        spriteBatch.Draw(value2, vector + vector3, null, color2 * lerpValue, rotation, origin2, num3, SpriteEffects.None, 0f);
            //    }

            //    spriteBatch.Draw(value2, vector, null, color2 * lerpValue, rotation, origin2, num3 * 1.1f, SpriteEffects.None, 0f);
            //}

            //spriteBatch.Draw(value2, vector, null, color4, rotation, origin2, num3, SpriteEffects.None, 0f);
        }
        void drawLaserGlow() {
            drawContext = drawContext with { 
                Texture = glowTexture,
                Clip = glowClip
            };
            Draw_Inner(drawContext with { DrawColor = getLaserGlowColor(drawColor) });
            DrawUnderShadowEffect(drawContext, draw: (newPosition, newColor) => {
                newColor = getLaserGlowColor(newColor);
                Draw_Inner(drawContext with {
                    Position = newPosition,
                    DrawColor = newColor,
                });
            }, sinWaveOffset: WaveOffset + MathHelper.Pi,
               applyInnerOpacity: false,
               forcedOpacity: MathHelper.Lerp(0.125f, 0.25f, 1f),
               sinWaveOffset_BasedOnEffectIndex: MathHelper.TwoPi * 0.25f,
               sinStep: AICounter);
        }

        drawSelf();
        drawGlowingEyes();
        drawLaserGlow();
        //drawLaserLine();
    }

    private void UpdateVisuals() {
        ref float glowOpacity = ref _glowOpacity;
        glowOpacity = Helper.Approach(glowOpacity, Phase1LaserAttackProgress, 0.1f);
    }

    public static Color GetLaserGlowColor(Color drawColor) => drawColor.MultiplyRGBA(MainRedColor_Dynamic);

    private void Draw_Inner(DrawContext drawContext) {
        NPC.QuickDraw(drawContext.SpriteBatch, drawContext.ScreenPosition, drawContext.DrawColor, frameBox: drawContext.Clip, 
                                                                                                  rotation: drawContext.Rotation, 
                                                                                                  position: drawContext.Position, 
                                                                                                  texture: drawContext.Texture, 
                                                                                                  effect: drawContext.Flip);
    }

    public static void DrawUnderShadowEffect(DrawContext drawContext, Action<Vector2, Color> draw, float sinWaveOffset = 0f, 
                                                                                                   bool applyInnerOpacity = true, 
                                                                                                   float forcedOpacity = 1f,
                                                                                                   bool drawXEffect = true,
                                                                                                   bool drawYEffect = true,
                                                                                                   float sinWaveOffset_BasedOnEffectIndex = MathHelper.TwoPi * 0.5f,
                                                                                                   float? sinStep = null) {
        sinStep ??= Main.GlobalTimeWrappedHourly;
        float sinStep_Value = sinStep.Value;
        Vector2 position = drawContext.Position;
        float rotation = drawContext.Rotation;
        int shadowCount = 20;
        for (float k = 0f; k < MathHelper.TwoPi; k += MathHelper.TwoPi / 4f) {
            for (int i = shadowCount; i > 0; i--) {
                float shadowProgress = i / (float)shadowCount;
                Vector2 eyesPosition = position;
                Vector2 rotationDirection = -Vector2.UnitY.RotatedBy(rotation + k);
                float rotationOffsetValue = shadowCount * 2 * shadowProgress;
                eyesPosition += rotationDirection * rotationOffsetValue;
                Color eyesColor = Color.White;
                eyesColor.A = 0;
                eyesColor *= 1f - shadowProgress;
                bool x = k is MathHelper.PiOver2 or (MathHelper.Pi + MathHelper.PiOver2);
                if (!drawXEffect && x) {
                    continue;
                }
                if (!drawYEffect && !x) {
                    continue;
                }
                float getWaveFactor(float waveOffset = 0f) => Helper.Wave(sinStep_Value, 0.25f, 1f, 10f, sinWaveOffset + waveOffset);
                if (applyInnerOpacity) {
                    eyesColor *= getWaveFactor(0f);
                    eyesColor *= getWaveFactor(2f);
                    eyesColor *= getWaveFactor(4f);
                    eyesColor *= getWaveFactor(6f);
                }
                float kWaveOffset = x.ToInt() * sinWaveOffset_BasedOnEffectIndex;
                eyesColor *= Helper.Wave(sinStep_Value, 0.5f, 1f, 10f, sinWaveOffset + kWaveOffset);
                if (applyInnerOpacity) {
                    eyesColor *= 0.5f;
                }
                eyesColor *= forcedOpacity;

                draw(eyesPosition, eyesColor);
            }
        }
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { }
}
