using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class FestiveTopHat : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Festive Top Hat");
			Tooltip.SetDefault("'Because nothing tops the festive season!'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 40; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Green;
			Item.buyPrice(gold: 5);

			Item.vanity = true;
		}
	}
}
