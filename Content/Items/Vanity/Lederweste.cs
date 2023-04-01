using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Body)]

	public class Lederweste : ModItem {
		public override void SetStaticDefaults () {
			// DisplayName.SetDefault("Lederweste");
			// Tooltip.SetDefault("");

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 10);
			Item.vanity = true;
		}
	}
}