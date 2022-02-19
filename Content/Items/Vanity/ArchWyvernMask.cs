using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class ArchWyvernMask : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Arch Wyvern Mask");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 40; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Pink;
			Item.sellPrice(gold: 1, silver: 60);

			Item.vanity = true;
		}
	}
}
