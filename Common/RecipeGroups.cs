using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Vanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Weapons.Melee;

namespace Consolaria
{
	public class RecipeGroups : ModSystem
	{
		public static RecipeGroup Titanium; 

		public override void Unload()
			=> Titanium = null;

		public override void AddRecipeGroups() 
		{
			Titanium = new RecipeGroup(() => "Adamantite or Titanium Bar", ItemID.AdamantiteBar, ItemID.TitaniumBar);
			RecipeGroup.RegisterGroup("Consolaria:TitaniumRecipeGroup", Titanium);
		}

		public override void AddRecipes() 
		{
			Recipe.Create(ItemID.RainbowBrick, 10)
				.AddIngredient(ItemID.StoneBlock, 10)
				.AddIngredient<RainbowPiece>(1)
				.AddTile(TileID.Furnaces)
				.Register();
		}

		public override void PostAddRecipes()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];

				if (recipe.HasResult(ItemID.Zenith))
				{
					recipe.AddIngredient(ModContent.ItemType<Tizona>());
				}
			}
		}
	}
}