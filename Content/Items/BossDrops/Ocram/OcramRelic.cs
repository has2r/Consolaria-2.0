using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Ocram
{
	public class OcramRelic : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ocram Relic");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics>());
			Item.placeStyle = 2;

			Item.maxStack = 99;

			Item.rare = ItemRarityID.Master;
			Item.value = Item.buyPrice(gold: 5);

			Item.master = true; 
		}
	}
}
