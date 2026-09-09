using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
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
        DrawItself(spriteBatch, screenPos, drawColor);

        return false;
    }

    private void DrawItself(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
        drawColor = Color.Lerp(drawColor, Color.White, 0.5f);
        Texture2D texture = NPC.GetTexture();
        SpriteEffects flip = (-NPC.spriteDirection).ToSpriteEffects();
        NPC.QuickDraw(spriteBatch, screenPos, drawColor, NPC.frame, texture: texture, effect: flip);
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { 

    }
}
