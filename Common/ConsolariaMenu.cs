using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    internal class ConsolariaMenu : ModMenu {
        public override string DisplayName => "Consolaria";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Logo");

        public override int Music => MusicID.ConsoleMenu; 

        public override bool PreDrawLogo (SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) {
            logoDrawCenter += new Vector2(0, 14);
            logoScale *= 0.65f;
            return true;
        }
    }
}
