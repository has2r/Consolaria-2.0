using Microsoft.Xna.Framework;
using Terraria;
<<<<<<< Updated upstream
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
=======
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
>>>>>>> Stashed changes
    [AutoloadEquip(EquipType.Legs)]

	public class AncientPlumbersPants : ModItem
	{
		public override void SetStaticDefaults() 
		{
<<<<<<< Updated upstream
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
=======
			DisplayName.SetDefault("Ancient Plumber's Pants");
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PlumbersPants;
>>>>>>> Stashed changes
		}

		public override void SetDefaults() 
		{
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.buyPrice(gold: 25);
			Item.vanity = true;
		}
	}
}