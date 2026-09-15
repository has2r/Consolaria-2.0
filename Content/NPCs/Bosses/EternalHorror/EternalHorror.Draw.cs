using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    private float _glowOpacity,
                  _shadowProgress,
                  _shadowTime,
                  _dashOpacity;

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
                  glowTexture = _glowTexture.Value,
                  shadowTexture = _shadowTexture.Value;
        SpriteEffects flip = (-NPC.spriteDirection).ToSpriteEffects();
        Vector2 position = NPC.Center;
        float rotation = NPC.rotation;
        Rectangle clip = NPC.frame,
                  glowClip = glowTexture.Bounds;

        DrawContext drawContext = new(spriteBatch, position, texture, clip, drawColor, rotation, flip, screenPos);

        void drawShadows() {
            drawContext = drawContext with { Texture = shadowTexture };
            DrawUnderShadowEffect(drawContext, draw: (newPosition, newColor) => {
                ShaderLoader.DistortShader.SetDefault(shadowTexture.Width * 2, shadowTexture.Height * 2);
                ShaderLoader.DistortShader.Anxiety = 0.5f + 0.5f * _shadowProgress;
                ShaderLoader.ApplyEffect(ShaderLoader.DistortShader.Effect, spriteBatch, () => {
                    Draw_Inner(drawContext with {
                        Position = newPosition,
                        DrawColor = newColor,
                    });
                });
            }, sinWaveOffset: WaveOffset,
               progress: _shadowProgress,
               opacity: 0.375f,
               sinStep: _shadowTime);
        }
        void drawSelf() {
            drawContext = drawContext with { Texture = texture };
            Draw_Inner(drawContext);
        }
        void drawGlowingEyes() {
            Texture2D eyesTexture = _eyeTexture.Value;
            drawContext = drawContext with { Texture = eyesTexture };
            DrawUnderGlowEffect(drawContext, draw: (newPosition, newColor) => {
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
            DrawUnderGlowEffect(drawContext, draw: (newPosition, newColor) => {
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
            DrawUnderGlowEffect(drawContext, draw: (newPosition, newColor) => {
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
        void drawClones() {
            if (!Init) {
                return;
            }
            foreach (CloneInfo cloneInfo in _cloneData) {
                if (!cloneInfo.Active) {
                    continue;
                }

                Player target = NPC.GetTargetPlayer();
                Vector2 clonePosition = cloneInfo.VisualPosition;
                Color cloneColor = drawColor;
                float timeLeftProgress = cloneInfo.TimeLeftProgress;
                cloneColor *= cloneInfo.Opacity;
                DrawContext cloneDrawContext = drawContext with {
                    Texture = shadowTexture,
                    Position = clonePosition,
                    DrawColor = cloneColor
                };
                DrawUnderShadowEffect(cloneDrawContext, draw: (newPosition, newColor) => {
                    Vector2 position = newPosition;
                    Color color = newColor;
                    float rotation = cloneInfo.Rotation;

                    ShaderLoader.DistortShader.SetDefault(shadowTexture.Width * 2, shadowTexture.Height * 2);
                    ShaderLoader.ApplyEffect(ShaderLoader.DistortShader.Effect, spriteBatch, () => {
                        Draw_Inner(cloneDrawContext with {
                            Position = position,
                            DrawColor = color,
                            Rotation = rotation
                        });
                    });
                }, sinWaveOffset: WaveOffset,
                   progress: timeLeftProgress * 0.25f,
                   opacity: 0.375f,
                   sinStep: _shadowTime);
            }
        }
        void drawTrails() {
            int length = NPC.oldPos.Length - 1;
            for (int num173 = 1; num173 < length; num173 += 1) {
                _ = ref NPC.oldPos[num173];
                Color color39 = drawColor;
                color39 = color39.MultiplyRGBA(MainPurpleColor_Dynamic);
                color39.R = (byte)(1f * (double)(int)color39.R * (double)(length - num173) / length);
                color39.G = (byte)(1f * (double)(int)color39.G * (double)(length - num173) / length);
                color39.B = (byte)(1f * (double)(int)color39.B * (double)(length - num173) / length);
                color39.A = (byte)(1f * (double)(int)color39.A * (double)(length - num173) / length);
                //color39 *= MathHelper.Clamp(NPC.velocity.Length(), 0f, 9f) / 9f;
                color39 *= 1f - num173 / length;
                //color39 *= _trailOpacity;
                //color39 *= 0.8f;
                color39 *= 1f;
                color39 *= _dashOpacity;
                Rectangle frame7 = NPC.frame;
                Vector2 origin = NPC.frame.Centered();
                Vector2 pos = NPC.oldPos[num173];
                pos += NPC.Size / 2f;

                pos = Vector2.Lerp(pos, NPC.Center, Ease.SineIn(1f - _dashOpacity));

                pos -= screenPos;

                ShaderLoader.DistortShader.SetDefault(shadowTexture.Width * 2, shadowTexture.Height * 2);
                ShaderLoader.ApplyEffect(ShaderLoader.DistortShader.Effect, spriteBatch, () => {
                    spriteBatch.Draw(shadowTexture,
                    pos,
                    frame7, color39 * NPC.Opacity, NPC.rotation, origin, NPC.scale, flip, 0f);
                });
            }
        }

        drawTrails();
        drawShadows();
        drawClones();
        drawSelf();
        drawGlowingEyes();
        drawLaserGlow();
        //drawLaserLine();
    }

    private void UpdateVisuals() {
        float lerpValue = 0.1f;
        float glowOpacity = 0f;
        if (HasActiveState<Phase1LaserAttack>()) {
            glowOpacity = Phase1LaserAttackProgress;
        }
        _glowOpacity = Helper.Approach(_glowOpacity, glowOpacity, lerpValue);
        float shadowOpacity = 0f;
        if (HasActiveState<Phase1ShadowSpawn>()) {
            shadowOpacity = Phase1ShadowSpawnProgress;
        }
        else {
            lerpValue = 1f;
        }
        _shadowProgress = Helper.Approach(_shadowProgress, shadowOpacity, lerpValue);
        _shadowTime += 1 / 60f;
        if (_shadowProgress <= 0f) {
            _shadowTime = 0;
        }
        _dashOpacity = Helper.Approach(_dashOpacity, 0f, 1 / 60f);
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
                                                                                                   float progress = 0f,
                                                                                                   float countStep = MathHelper.PiOver2,
                                                                                                   float opacity = 1f,
                                                                                                   float sinStep = 0f) {
        Vector2 position = drawContext.Position;
        float rotation = drawContext.Rotation;
        Color color = drawContext.DrawColor;
        for (int i = 0; i < 1; i++) {
            for (float k = -MathHelper.Pi; k <= MathHelper.Pi; k += countStep) {
                Vector2 shadowPosition = position;
                float alpha = (progress >= 0.5f) ? (1f - (progress - 0.5f) / 0.5f) : (progress / 0.5f);
                float offset = alpha * 32f;
                float time = sinStep == 0f ? Main.GlobalTimeWrappedHourly : sinStep;
                shadowPosition += Vector2.UnitX.RotatedBy(k + time + rotation) * (float)(offset + offset * 0.5f
                    * MathF.Sin(time * 4f));
                //shadowPosition.X += Helper.Wave(-1f, -1f, 5f, k + sinWaveOffset) * 10f * progress;
                //shadowPosition.Y += Helper.Wave(-1f, -1f, 5f, k + MathHelper.Pi + sinWaveOffset) * 10f * progress;
                Color shadowColor = color;
                shadowColor = shadowColor.MultiplyRGBA(MainPurpleColor);
                shadowColor = Color.Lerp(shadowColor, shadowColor.MultiplyRGBA(MainPurpleColor_Dynamic), 0.5f);
                shadowColor = shadowColor.MultiplyAlpha(alpha);
                shadowColor.A /= 1;
                shadowColor *= opacity;
                draw(shadowPosition, shadowColor);
            }
        }
    }

    public static void DrawUnderGlowEffect(DrawContext drawContext, Action<Vector2, Color> draw, float sinWaveOffset = 0f, 
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
