using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Consolaria.Content.Items.Weapons.Summon {
	public class EternityStaff : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Eternity Staff");
			Tooltip.SetDefault("Summons an eye of eternity to fight for you");

			ItemID.Sets.GamepadWholeScreenUseRange [Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision [Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			int width = 36; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;

			Item.mana = 25;
			Item.noMelee = true;

			Item.DamageType = DamageClass.Summon;
			Item.damage = 36;
			Item.knockBack = 4f;

			Item.value = Item.sellPrice(gold: 5, silver: 75);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item44;

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.EyeOfEternity>();
			Item.buffType = ModContent.BuffType<Buffs.EyeOfEternity>();
		}

		public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
			=> position = Main.MouseWorld;

		public override bool Shoot (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			return false;
		}
	}
}
