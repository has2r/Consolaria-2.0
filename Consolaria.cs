using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria
{
	public class Consolaria : Mod
	{
		public static ModKeybind OcramJumpKeybind;

        public override void Load() {
            OcramJumpKeybind = KeybindLoader.RegisterKeybind(this, "Use  Rocketjump", Keys.LeftAlt);
			if (Main.netMode != NetmodeID.Server) {
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(new Ref<Effect>(Assets.Request<Effect>("Assets/Effects/Shockwave", AssetRequestMode.ImmediateLoad).Value), "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}
		}

        public override void Unload() {
            OcramJumpKeybind = null;
        }
    }
}