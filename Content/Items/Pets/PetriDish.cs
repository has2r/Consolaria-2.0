using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets
{
	public class PetriDish : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Summons a pet Slime");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.Pets.Slime>();
			Item.buffType = ModContent.BuffType<Buffs.Slime>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600);
		}
	}
}
