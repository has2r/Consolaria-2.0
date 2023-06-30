using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Body)]

	public class TangYuanShirt : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;

			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			int equipSlotBodyAlt = EquipLoader.GetEquipSlot(Mod, "TangYuanShirt", EquipType.Body);

			ArmorIDs.Body.Sets.HidesArms [equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms [equipSlotBodyAlt] = true;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 90);
			Item.vanity = true;
		}
	}
}