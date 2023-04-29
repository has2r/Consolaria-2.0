using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]

	public class GeorgesHat : ModItem
	{
		public override void SetStaticDefaults() {

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 15);
			Item.vanity = true;
		}
    }
}

