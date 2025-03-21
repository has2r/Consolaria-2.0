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
		public bool vanillaBossMusicEnabled;

		[DefaultValue(true)]
        [ReloadRequired]
        public bool heartbeatariaIntegrationEnabled;

		[DefaultValue(false)]
        [ReloadRequired]
        public bool genderRestrictShopEnabled;

		[DefaultValue(false)]
        [ReloadRequired]
        public bool oktoberLocksEnabled;

		[DefaultValue(false)]
        [ReloadRequired]
        public bool originalAncientHeroSetRecipeEnabled;

		[DefaultValue(false)]
        [ReloadRequired]
        public bool mythicalWyvernKiteVanillaDropruleEnabled;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool tizonaZenithIntegrationEnabled;

        [Header("WorldGeneration")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool pyramidEnabled;
    }
}