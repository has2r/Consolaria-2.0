using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private readonly record struct DrawContext(SpriteBatch SpriteBatch, Vector2 Position, Texture2D Texture, Color DrawColor, float Rotation, SpriteEffects Flip, Vector2 ScreenPosition);

    private static Asset<Texture2D> _eyeTexture;

    private float WaveOffset => NPC.whoAmI;

    private partial void Load_Textures() {
        _eyeTexture = ModContent.Request<Texture2D>(Texture + "_Eyes");
    }

    private static Color MainPurpleColor => new(175, 85, 255);
    private static Color MainPurpleColor_Dynamic => Color.Lerp(new(175, 85, 255), Color.Lerp(new(198, 123, 173), new(131, 186, 64), 0.5f), Helper.Wave(0f, 1f, 1f, 0f));

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
        drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
        drawColor = Color.Lerp(drawColor, Color.White, 0.5f);
        Texture2D texture = NPC.GetTexture();
        SpriteEffects flip = (-NPC.spriteDirection).ToSpriteEffects();
        Vector2 position = NPC.Center;
        float rotation = NPC.rotation;

        DrawContext drawContext = new(spriteBatch, position, texture, drawColor, rotation, flip, screenPos);

        Draw_Inner(drawContext);

        Texture2D eyesTexture = _eyeTexture.Value;
        drawContext = drawContext with { Texture = eyesTexture };
        DrawUnderShadowEffect(drawContext, (newPosition, newColor) => {
            Draw_Inner(drawContext with {
                Position = newPosition,
                DrawColor = newColor,
            });
        });
    }

    private void Draw_Inner(DrawContext drawContext) {
        NPC.QuickDraw(drawContext.SpriteBatch, drawContext.ScreenPosition, drawContext.DrawColor, rotation: drawContext.Rotation, position: drawContext.Position, texture: drawContext.Texture, effect: drawContext.Flip);
    }

    private void DrawUnderShadowEffect(DrawContext drawContext, Action<Vector2, Color> draw) {
        Vector2 position = drawContext.Position;
        float rotation = drawContext.Rotation;
        int shadowCount = 20;
        for (float k = 0f; k < MathHelper.TwoPi; k += MathHelper.TwoPi / 4f) {
            for (int i = shadowCount; i > 0; i--) {
                float shadowProgress = i / (float)shadowCount;
                Vector2 eyesPosition = position;
                eyesPosition += -Vector2.UnitY.RotatedBy(rotation + k) * shadowCount * 2 * shadowProgress;
                Color eyesColor = Color.White;
                eyesColor.A = 0;
                eyesColor *= 1f - shadowProgress;
                float getWaveFactor(float waveOffset = 0f) => Helper.Wave(0.25f, 1f, 10f, waveOffset + WaveOffset);
                eyesColor *= getWaveFactor(0f);
                eyesColor *= getWaveFactor(2f);
                eyesColor *= getWaveFactor(4f);
                eyesColor *= getWaveFactor(6f);
                float kWaveOffset = (k == MathHelper.PiOver2 || k == MathHelper.Pi + MathHelper.PiOver2).ToInt() * MathHelper.TwoPi * 0.5f;
                eyesColor *= Helper.Wave(0.5f, 1f, 10f, kWaveOffset + WaveOffset);

                eyesColor *= 0.5f;

                draw(eyesPosition, eyesColor);
            }
        }
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { }
}
