using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Head)]
	public class AncientPlumbersHat : ModItem {
		public override void SetStaticDefaults () {
			ArmorIDs.Head.Sets.DrawHatHair [Item.headSlot] = true;
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem [Type] = ItemID.PlumbersHat;
			ItemID.Sets.ShimmerTransformToItem [ItemID.PlumbersHat] = Type;
		}

		public override void SetDefaults () {
			int width = 38; int height = 34;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 20);
			Item.vanity = true;
		}
	}
}