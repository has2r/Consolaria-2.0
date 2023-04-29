using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]

	public class AncientPlumbersShirt : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() 
		{
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.buyPrice(gold: 25);
			Item.vanity = true;
		}

		public override void AddRecipes() 
		{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<AncientPlumbersShirt>())
			.AddCustomShimmerResult(ItemID.PlumbersShirt)
			.Register();
		}
	}
}
