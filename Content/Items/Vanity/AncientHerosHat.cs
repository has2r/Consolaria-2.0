using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity 
{
	[AutoloadEquip(EquipType.Head)]
	public class AncientHerosHat : ModItem {
		public override void SetStaticDefaults () 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () 
		{
			int width = 38; int height = 34;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 20);
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