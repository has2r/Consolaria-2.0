using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace Consolaria
{
	public class Consolaria : Mod
	{
		public static ModKeybind OcramJumpKeybind;

        public override void Load() {
            OcramJumpKeybind = KeybindLoader.RegisterKeybind(this, "Use Exta Jump", Keys.LeftAlt);
        }

        public override void Unload() {
            OcramJumpKeybind = null;
        }
    }
}