using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
    [AutoloadEquip(EquipType.Head)]
	public class OcramMask : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 20; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Blue;

			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
