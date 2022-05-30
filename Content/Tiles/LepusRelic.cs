using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Consolaria.Content.Tiles
{
	public class LepusRelic : Relics
	{
		public override string RelicTextureName => "Consolaria/Content/Tiles/LepusRelic";

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
			=> Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.BossDrops.Lepus.LepusRelic>());	
	}
}
