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

		[Label("Enable Easter")]
		[DefaultValue(false)]
		public bool easterEnabled;

		[Label("Enable Thanksgiving Day")]
		[DefaultValue(false)]
		public bool thanksgivingEnabled;
	}
}
