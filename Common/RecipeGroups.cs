using Consolaria.Common;
using Consolaria.Content.Items.Weapons.Melee;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria {
    public class RecipeGroups : ModSystem {
		public static RecipeGroup Titanium;

		public override void Unload ()
			=> Titanium = null;

		public override void AddRecipeGroups () {
			Titanium = new RecipeGroup(() => "Adamantite or Titanium Bar", ItemID.AdamantiteBar, ItemID.TitaniumBar);
			RecipeGroup.RegisterGroup("Consolaria:TitaniumRecipeGroup", Titanium);
		}

		public override void PostAddRecipes () {
			if (ModContent.GetInstance<ConsolariaConfig>().tizonaZenithIntegrationEnabled) {
				for (int i = 0; i < Recipe.numRecipes; i++) {
					Recipe recipe = Main.recipe [i];

					if (recipe.HasResult(ItemID.Zenith)) {
						recipe.AddIngredient(ModContent.ItemType<Tizona>());
					}
				}
			}
		}
	}
}