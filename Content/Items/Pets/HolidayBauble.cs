using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets
{
	public class HolidayBauble : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Holiday Bauble");
			Tooltip.SetDefault("Summons a pet Elfa");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.Elfa>(), ModContent.BuffType<Buffs.Elfa>());

			int width = 22; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 1);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			return false;
		}
	}
}
