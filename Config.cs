using Newtonsoft.Json;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace Consolaria
{
	[Label("$Consolaria Events Config")]

	class ServerConfig : ModConfig
	{
		[JsonIgnore]
		public const string ConfigName = "Events";

		public override bool Autoload(ref string name)
		{
			name = ConfigName;
			return base.Autoload(ref name);
		}

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public static ServerConfig Instance;

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			message = Language.GetTextValue("Mods.Consolaria.Config.ServerBlocked");
			return false;
		}
        [Label("Enable seasonal event limits")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool SeasonsEnabled;

    }
}
