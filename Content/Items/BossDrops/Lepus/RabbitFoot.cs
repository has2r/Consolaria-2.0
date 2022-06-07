using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Lepus
{
	public class RabbitFoot : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rabbit Foot");
			Tooltip.SetDefault("Summons a Baby Lepus");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.SmallLepus>(), ModContent.BuffType<Buffs.SmallLepus>());

			int width = 20; int height = 32;
			Item.Size = new Vector2(width, height);

			Item.master = true;
			Item.value = Item.sellPrice(gold: 5);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2); 
			return false;
		}
	}
}
