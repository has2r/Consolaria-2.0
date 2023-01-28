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

			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.OcramRelic>());

			Item.maxStack = 9999;

			Item.rare = ItemRarityID.Master;
			Item.value = Item.buyPrice(gold: 1);

			Item.master = true; 
		}
	}
}
