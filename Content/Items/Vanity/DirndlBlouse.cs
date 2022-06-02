using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]

	public class DirndlBlouse : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Dirndl Blouse");
			Tooltip.SetDefault("");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 15);
			Item.vanity = true;
		}
	}
}
