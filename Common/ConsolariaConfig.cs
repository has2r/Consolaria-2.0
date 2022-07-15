using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Consolaria.Common
{
	[Label("Configuration")]
	public class ConsolariaConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public static ConsolariaConfig Instance;

		[Header("Seasonal Events")]

		[Label("Restrict seasonal content by system date (Easter)")]
		[Tooltip("If enabled the relevant items and enemies will only appear during Easter\nApril 1 - April 30")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool easterEnabled;

		[Label("Restrict seasonal content by system date (Thanksgiving)")]
		[Tooltip("If enabled the relevant items and enemies will only appear during Thanksgiving\nNovember 1 - November 30")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool thanksgivingEnabled;

		[Label("Restrict seasonal content by system date (Other events)")]
		[Tooltip("If enabled the relevant items and enemies will only appear during the corresponding holidays or when Wishbone is used")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool smallEventsEnabled;
	}
}
