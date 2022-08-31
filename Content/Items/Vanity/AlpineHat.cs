using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Head)]

	public class AlpineHat : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Alpine Hat");
			Tooltip.SetDefault("");

			ArmorIDs.Head.Sets.DrawHatHair [Item.headSlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
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