using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets
{
	public class GoldenLantern : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Golden Lantern");
			Tooltip.SetDefault("Summons a mythical wyvernling to provide light");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.MythicalWyvernling>(), ModContent.BuffType<Buffs.MythicalWyvernling>());

			int width = 28; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 10);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2); 
			return false;
		}
	}
}
