using Consolaria.Common;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Body)]
	public class AncientHerosShirt : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			if (!ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
				ItemID.Sets.ShimmerTransformToItem [Type] = ItemID.HerosShirt;
				ItemID.Sets.ShimmerTransformToItem [ItemID.HerosShirt] = Type;
			}
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 10);
			Item.vanity = true;
		}
	}
}