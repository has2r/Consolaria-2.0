using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets
{
	public class Beeswax : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Summons a pet Tiphia");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.Carrot);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 3);

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.Pets.Tiphia>();
			Item.buffType = ModContent.BuffType<Buffs.Tiphia>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600);
		}
	}
}
