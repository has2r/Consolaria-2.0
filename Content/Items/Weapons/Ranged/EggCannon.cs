using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged {
    public class EggCannon : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			int width = 56; int height = 20;
			Item.Size = new Vector2(width, height);

			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;

			Item.knockBack = 0.5f;
			Item.noMelee = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = Item.useTime = 24;

			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 1);

			Item.shoot = ModContent.ProjectileType<EasterEgg>();
			Item.shootSpeed = 10f;
		}

		public override Vector2? HoldoutOffset ()
			=> new Vector2(-5, 0);

		public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += new Vector2(-_velocity.Y, _velocity.X) * (1.5f * player.direction);
		}
	}
}