using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Turkor
{
	public class TurkorRelic : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Turkor the Ungrateful Relic");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TurkorRelic>());

			Item.maxStack = 99;

			Item.rare = ItemRarityID.Master;
			Item.value = Item.buyPrice(gold: 1);

			Item.master = true; 
		}
	}
}
