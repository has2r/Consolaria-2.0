using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]

	public class MonomiBody : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Monomi Body");
			ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 20)
				.AddIngredient<WhiteThread>(3)
				.AddIngredient(ItemID.PinkThread, 3)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}
