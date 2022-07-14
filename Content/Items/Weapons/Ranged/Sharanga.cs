using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged {
	public class Sharanga : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Sharanga");
			Tooltip.SetDefault("Transforms any suitable ammo into Spectral Arrows");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			int width = 20; int height = 46;
			Item.Size = new Vector2(width, height);

			Item.damage = 36;
			Item.knockBack = 1.5f;
			Item.DamageType = DamageClass.Ranged;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = Item.useAnimation = 20;

			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 8f;

			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item5;

			Item.autoReuse = true;
			Item.noMelee = true;
		}

		public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<Projectiles.Friendly.SpectralArrow>();

			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += _velocity * 35;
			position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
		}

		public override void AddRecipes () {
			CreateRecipe()
				.AddIngredient(ItemID.MoltenFury)
				.AddIngredient(ItemID.DemonBow, 1)
				.AddTile(TileID.DemonAltar)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.MoltenFury)
				.AddIngredient(ItemID.TendonBow, 1)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}
