using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.Banners
{
	public class DragonHornetBanner : ModItem
	{
		public override void SetDefaults() {
			int width = 10; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.maxStack = 99;

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners>());
			Item.placeStyle = 10;

			Item.consumable = true;

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 10);
		}
	}
}
