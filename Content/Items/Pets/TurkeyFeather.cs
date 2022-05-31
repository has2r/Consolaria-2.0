using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.PetTurkey>(), ModContent.BuffType<Buffs.PetTurkey>());
			
			int width = 46; int height = 30;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 10);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile
			return false;
		}
	}
}
