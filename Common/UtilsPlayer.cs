using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Common
{
	public class UtilsPlayer : ModPlayer 
	{
        public static Color DiscoColor;

        private Color originalColor;
        private Color targetColor;
        private float blendCount;

        public override void PostUpdate() {
            if (Content.Dusts.RomanFlame.changeColor) { 
                DiscoColor = Helper.FadeToColor(originalColor, targetColor, blendCount, 100);
                blendCount += 0.025f;
                if (blendCount >= 1) {
                    originalColor = targetColor;
                    targetColor = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), 100);
                    blendCount = 0;
                }
            }
        }
    }
}
