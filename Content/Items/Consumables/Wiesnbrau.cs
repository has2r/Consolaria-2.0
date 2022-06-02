using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Buffs;

namespace Consolaria.Content.Items.Consumables
{
	public class Wiesnbrau : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Wiesnbräu");
			Tooltip.SetDefault("Numbs the user from damage taken but also reduces damage inflicted");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
		}

		public override void SetDefaults() {
			int width = 18; int height = 30;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 10);

			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;

			Item.maxStack = 20;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;

			Item.buffType = ModContent.BuffType<Drunk>();
			Item.buffTime = 60 * 30;
		}
	}
}