using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Legs)]

	public class DirndlSkirt : ModItem {
		public override void Load () {
			string pantsTextureFemale = "Consolaria/Content/Items/Vanity/DirndlSkirt_Legs_Female";
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, pantsTextureFemale, EquipType.Legs, name: "DirndlSkirtFemale");
		}

		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Dirndl Skirt");
			Tooltip.SetDefault("");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 10);
			Item.vanity = true;
		}

		public override void SetMatch (bool male, ref int equipSlot, ref bool robes) {
			if (!male)
				equipSlot = EquipLoader.GetEquipSlot(Mod, "DirndlSkirtFemale", EquipType.Legs);
		}
	}
}