using Consolaria.Backgrounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    internal class ValentinesDayMenu : ModMenu {
        public override string DisplayName => "Valentine's Day";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Consolaria/Assets/Textures/ValentinesDayLogo");

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<ValentinesDayBackgroundStyle>();

        public override bool PreDrawLogo (SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) {
            logoDrawCenter += new Vector2(0, 14);
            logoScale *= 1f;
            return true;
        }
    }
}
