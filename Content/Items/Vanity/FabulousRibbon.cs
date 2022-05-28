using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]

	public class FabulousRibbon : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Fabulous Ribbon");
			Tooltip.SetDefault("'Allows flight and slow fall'");

			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(gold: 15);
			Item.vanity = true;
		}
    }
}

