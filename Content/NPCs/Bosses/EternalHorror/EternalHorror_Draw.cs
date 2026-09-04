using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public override void FindFrame(int frameHeight) {
        
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
        drawColor = Color.Lerp(drawColor, Color.White, 0.5f);
        Texture2D texture = NPC.GetTexture();
        SpriteEffects flip = (-NPC.spriteDirection).ToSpriteEffects();
        NPC.QuickDraw(spriteBatch, screenPos, drawColor, NPC.frame, texture: texture, effect: flip);

        return false;
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { 
    }
}
