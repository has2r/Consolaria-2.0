using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Materials;
using Consolaria.Common;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Body)]
	public class AncientHerosShirt : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			if (!ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
				ItemID.Sets.ShimmerTransformToItem [Type] = ItemID.HerosPants;
				ItemID.Sets.ShimmerTransformToItem [ItemID.HerosPants] = Type;
			}
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 10);
			Item.vanity = true;
		}
		
		public override void AddRecipes () {
			if (ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
				CreateRecipe()
					.AddIngredient(ItemID.Silk, 20)
					.AddIngredient<PurpleThread>(3)
					.AddTile(TileID.Loom)
//					.AddCustomShimmerResult(ItemID.Silk, 20)
//					.AddCustomShimmerResult<PurpleThread>(3)
					.Register();
			}
		}
	}
}