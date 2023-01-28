using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Body)]
	public class AncientHerosShirt : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Ancient Hero's Shirt");
			SacrificeTotal = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 10);
			Item.vanity = true;
		}
	}
}