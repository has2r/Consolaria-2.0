using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged {
    public class VolcanicRepeater : ModItem {
		public override void SetStaticDefaults () {

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			int width = 18; int height = 56;
			Item.Size = new Vector2(width, height);

			Item.damage = 76;
			Item.DamageType = DamageClass.Ranged;

			Item.useTime = 12;
			Item.useAnimation = 12;
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
			Item.noMelee = true;
		}

		public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<Projectiles.Friendly.VulcanBolt>();

			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += _velocity * 50;
			position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
		}

		public override Vector2? HoldoutOffset ()
			=> new Vector2(-5, 0);
	}
}