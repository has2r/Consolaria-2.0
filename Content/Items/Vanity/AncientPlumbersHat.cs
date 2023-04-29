using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
	public class AncientPlumbersHat : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			DisplayName.SetDefault("Ancient Plumber's Hat");
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PlumbersHat;
		}

		public override void SetDefaults() 
		{
			int width = 38; int height = 34;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 20);
			Item.vanity = true;
		}
	}
}
