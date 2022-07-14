using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.Banners {
	public class MythicalWyvernBanner : ModItem {
		public override void SetStaticDefaults () {
			SacrificeTotal = 1;
			Tooltip.SetDefault("{$CommonItemTooltip.BannerBonus}Mythical Wyvern");
		}

		public override void SetDefaults () {
			int width = 10; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.maxStack = 99;

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners>());
			Item.placeStyle = 14;

			Item.consumable = true;

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 10);
		}
	}
}