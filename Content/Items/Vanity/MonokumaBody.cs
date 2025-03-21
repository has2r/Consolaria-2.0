using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Body)]

	public class MonokumaBody : ModItem {
		public override void SetStaticDefaults () {
			ArmorIDs.Body.Sets.HidesTopSkin [Item.bodySlot] = true;

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}
	}
}