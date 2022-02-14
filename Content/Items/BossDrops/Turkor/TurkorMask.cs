using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Turkor
{
	[AutoloadEquip(EquipType.Head)]
	public class TurkorMask : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Turkor the Ungrateful Mask");
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
