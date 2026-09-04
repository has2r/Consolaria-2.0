using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public static float ScreenObstruction { get; private set; }
    public static Color FrontColor { get; private set; } = new Color(0, 0, 120);

    private float _purpleColorTime, _purpleColorTime2;
    private float _purpleColorStrength;

    private partial void Load_Background() {
        On_ScreenDarkness.Update += On_ScreenDarkness_Update;

        On_ScreenDarkness.DrawBack += On_ScreenDarkness_DrawBack;
        On_ScreenDarkness.DrawFront += On_ScreenDarkness_DrawFront;
    }

    private void On_ScreenDarkness_Update(On_ScreenDarkness.orig_Update orig) {
        orig();

        ApplyScreenDarkness();
    }

    private void On_ScreenDarkness_DrawBack(On_ScreenDarkness.orig_DrawBack orig, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
        orig(spriteBatch);

        DrawDarkness_Back(spriteBatch);
    }

    private void On_ScreenDarkness_DrawFront(On_ScreenDarkness.orig_DrawFront orig, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
        orig(spriteBatch);

        DrawDarkness_Front(spriteBatch);
    }

    private void ApplyScreenDarkness() {
        float value = 0f;
        float amount = 1f / 60f;
        Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
        for (int i = 0; i < 200; i++) {
            if (Main.npc[i].active && Main.npc[i].type == SelfType && Main.npc[i].Distance(mountedCenter) < 3000f) {
                value = 0.95f;
                FrontColor = new Color(22, 21, 18) * 0.3f;
                amount = 0.03f;
            }
        }

        amount *= 2f;

        ScreenObstruction = MathHelper.Lerp(ScreenObstruction, value, amount);

        if (_purpleColorTime == 0f) {
            _purpleColorTime = -Main.rand.NextFloat(Helper.SecondsToFrames(1f), Helper.SecondsToFrames(2.5f));
            _purpleColorTime2 = _purpleColorTime;
            _purpleColorTime *= 1.5f;
            _purpleColorStrength = Main.rand.NextFloat(0.75f);
        }
        _purpleColorTime = Helper.Approach(_purpleColorTime, 0f, 1f);
    }

    private void DrawDarkness_Back(SpriteBatch spriteBatch) {
        if (ScreenObstruction != 0f) {
            float purpleColorFactor = 0f;
            float purpleColorTime_Min = _purpleColorTime2 * 0.125f * 0.75f;
            if (_purpleColorTime > purpleColorTime_Min) {
                purpleColorFactor = Utils.GetLerpValue(purpleColorTime_Min, 0f, _purpleColorTime, true);
            }
            Color baseColor = Color.Black;
            baseColor = Color.Lerp(baseColor, MainPurpleColor_Dynamic, 0.125f * 0.5f);
            baseColor = Color.Lerp(baseColor, MainPurpleColor, purpleColorFactor * _purpleColorStrength * 0.5f);
            Color color = baseColor * ScreenObstruction;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
        }
    }

    private void DrawDarkness_Front(SpriteBatch spriteBatch) {
        if (ScreenObstruction != 0f) {
            Color baseColor = FrontColor;
            baseColor = Color.Lerp(baseColor, MainPurpleColor_Dynamic, 0.125f * 0.5f);
            Color color = baseColor * ScreenObstruction;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
        }
    }
}
