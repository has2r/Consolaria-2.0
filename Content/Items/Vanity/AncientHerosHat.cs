using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;
using Consolaria.Common;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Head)]
	public class AncientHerosHat : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			if (!ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
				ItemID.Sets.ShimmerTransformToItem [Type] = ItemID.HerosHat;
				ItemID.Sets.ShimmerTransformToItem [ItemID.HerosHat] = Type;
			}
        }

		public override void SetDefaults () {
			int width = 38; int height = 34;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 20);
			Item.vanity = true;
		}
	}
}