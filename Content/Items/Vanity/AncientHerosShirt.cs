using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Consolaria.Content.Items.Materials;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity 
{
	[AutoloadEquip(EquipType.Body)]
	public class AncientHerosShirt : ModItem 
	{
		public override void SetStaticDefaults () 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () 
		{
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 10);
			Item.vanity = true;
		}

		public override void AddRecipes () 
		{
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 20)
				.AddIngredient(ModContent.ItemType<PurpleThread>(), 3)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}