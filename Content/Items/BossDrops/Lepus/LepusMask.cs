using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Lepus
{
	[AutoloadEquip(EquipType.Head)]
	public class LepusMask : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Lepus Mask");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
