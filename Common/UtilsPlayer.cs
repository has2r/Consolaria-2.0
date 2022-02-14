using Consolaria.Content.Items.Weapons.Magic;
using Consolaria.Content.Projectiles.Friendly;
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
            if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<RomanCandle>() || Player.ownedProjectileCounts[ModContent.ProjectileType<RomanFlame>()] >= 1 || Player.ownedProjectileCounts[ModContent.ProjectileType<RomanFlameMid>()] >= 1 || Player.ownedProjectileCounts[ModContent.ProjectileType<RomanFlameFinal>()] >= 1) {
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
