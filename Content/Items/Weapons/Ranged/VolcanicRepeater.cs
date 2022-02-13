using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged
{
	public class VolcanicRepeater : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Vulcan Repeater");
			Tooltip.SetDefault("Transforms any suitable ammo into Vulcan Bolts");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 18; int height = 56;
			Item.Size = new Vector2(width, height);

			Item.damage = 45;
			Item.DamageType = DamageClass.Ranged;

			Item.useTime = 6;
			Item.useAnimation = 18;
			Item.reuseDelay = 20;

			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 15;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.useAmmo = AmmoID.Arrow;

			Item.value = Item.sellPrice(gold: 5, silver: 55);
			Item.rare = ItemRarityID.Lime;

			Item.UseSound = SoundID.Item70;
			Item.autoReuse = true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<Projectiles.Friendly.VulcanBolt>();

			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += _velocity * 50;
			position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
		}

		public override Vector2? HoldoutOffset()	
			=>  new Vector2(-5, 0);

        public override bool CanConsumeAmmo(Player player)  
			=> player.itemAnimation < Item.useAnimation - 3;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.HallowedRepeater)
				.AddIngredient(ItemID.HellstoneBar, 15)
				.AddIngredient<SoulofBlight>(15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
