using Consolaria.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria
{
	public class RecipeGroups : ModSystem
	{
		public static RecipeGroup Titanium; 

		public override void Unload()
			=> Titanium = null;

		public override void AddRecipeGroups() {
			Titanium = new RecipeGroup(() => "Adamantite or Titanium Bar", ItemID.AdamantiteBar, ItemID.TitaniumBar);
			RecipeGroup.RegisterGroup("Consolaria:TitaniumRecipeGroup", Titanium);
		}

		public override void AddRecipes() {
			Mod.CreateRecipe(ItemID.RainbowBrick)
			.AddIngredient(ItemID.StoneBlock, 25)
			.AddIngredient<RainbowPiece>(1)
			.AddTile(TileID.Furnaces)
			.Register();
		}
    }
}