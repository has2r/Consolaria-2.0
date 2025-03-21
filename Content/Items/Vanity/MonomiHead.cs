using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]

	public class MonomiHead : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 15);
			Item.vanity = true;
		}
	}
}

