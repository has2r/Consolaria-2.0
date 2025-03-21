using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Body)]

	public class MythicalRobe : ModItem {
		public override void Load () {
			string robeTexture = "Consolaria/Content/Items/Vanity/MythicalRobe_Extension";
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, robeTexture, EquipType.Legs, this);
		}

		public override void SetStaticDefaults ()
			=> Item.ResearchUnlockCount = 1;

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 90);
			Item.vanity = true;
		}

		public override void SetMatch (bool male, ref int equipSlot, ref bool robes) {
			var robeSlot = ModContent.GetInstance<MythicalRobe>();
			equipSlot = EquipLoader.GetEquipSlot(Mod, robeSlot.Name, EquipType.Legs);
			robes = true;
		}
	}
}