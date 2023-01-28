using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Head)]
	public class AncientHerosHat : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Ancient Hero's Hat");
			SacrificeTotal = 1;
		}

		public override void SetDefaults () {
			int width = 38; int height = 34;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 20);
			Item.vanity = true;
		}
	}
}