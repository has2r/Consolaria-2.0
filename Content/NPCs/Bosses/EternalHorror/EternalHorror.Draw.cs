using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private readonly record struct DrawContext(SpriteBatch SpriteBatch, Vector2 Position, Texture2D Texture, Rectangle Clip, Color DrawColor, float Rotation, SpriteEffects Flip, Vector2 ScreenPosition);

    private static Asset<Texture2D> _eyeTexture = null!,
                                    _glowTexture = null!;

    private float _glowOpacity;

    private float WaveOffset => NPC.whoAmI;

    private partial void Load_Textures() {
        _eyeTexture = ModContent.Request<Texture2D>(Texture + "_Eyes");
        _glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    private static Color MainPurpleColor => new(175, 85, 255);
    private static Color MainPurpleColor_Dynamic => Color.Lerp(new(175, 85, 255), Color.Lerp(new(198, 123, 173), new(131, 186, 64), 0.5f), Helper.Wave(0f, 1f, 1f, 0f));

    private static Color MainRedColor_Dynamic => Color.Lerp(new(255, 10, 25), MainPurpleColor_Dynamic, Helper.Wave(0f, 1f, 25f, 0f) * 0.25f);

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
            });
        }
        void drawLaserGlow() {
            Color getLaserGlowColor(Color drawColor) => drawColor.MultiplyRGBA(MainRedColor_Dynamic) * _glowOpacity;
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
            }, sinWaveOffset: MathHelper.Pi,
               applyInnerOpacity: false,
               forcedOpacity: MathHelper.Lerp(0.125f, 0.25f, 1f),
               sinWaveOffset_BasedOnEffectIndex: MathHelper.TwoPi * 0.25f,
               sinStep: AICounter);
        }

        drawSelf();
        drawGlowingEyes();
        drawLaserGlow();
    }

    private void Draw_Inner(DrawContext drawContext) {
        NPC.QuickDraw(drawContext.SpriteBatch, drawContext.ScreenPosition, drawContext.DrawColor, frameBox: drawContext.Clip, 
                                                                                                  rotation: drawContext.Rotation, 
                                                                                                  position: drawContext.Position, 
                                                                                                  texture: drawContext.Texture, 
                                                                                                  effect: drawContext.Flip);
    }

    private void DrawUnderShadowEffect(DrawContext drawContext, Action<Vector2, Color> draw, float sinWaveOffset = 0f, 
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
        for (float k = 0f; k < MathHelper.TwoPi; k += MathHelper.PiOver4) {
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
                float getWaveFactor(float waveOffset = 0f) => Helper.Wave(sinStep_Value, 0.25f, 1f, 10f, sinWaveOffset + waveOffset + WaveOffset);
                if (applyInnerOpacity) {
                    eyesColor *= getWaveFactor(0f);
                    eyesColor *= getWaveFactor(2f);
                    eyesColor *= getWaveFactor(4f);
                    eyesColor *= getWaveFactor(6f);
                }
                float kWaveOffset = x.ToInt() * sinWaveOffset_BasedOnEffectIndex;
                eyesColor *= Helper.Wave(sinStep_Value, 0.5f, 1f, 10f, sinWaveOffset + kWaveOffset + WaveOffset);
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
