using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets
{
	public class TurkeyFeather : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Turkey Feather");
			Tooltip.SetDefault("Summons a Pet Turkey");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.Carrot);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 10);

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.Pets.PetTurkey>();
			Item.buffType = ModContent.BuffType<Buffs.PetTurkey>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600);
		}
	}
}
