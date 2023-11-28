using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Consolaria.Common {
	public class ConsolariaConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("SeasonalEvents")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool easterEnabled;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool thanksgivingEnabled;

		[DefaultValue(false)]
		[ReloadRequired]
		public bool smallEventsEnabled;

		[Header("Miscellaneous")]
		[DefaultValue(false)]
		public bool vanillaBossMusic;

		[DefaultValue(false)]
        [ReloadRequired]
        public bool oktoberLocksEnabled;

        [DefaultValue(false)]
        [ReloadRequired]
        public bool dontTouchZenith;
    }
}