using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged
{
	public class DragonBreath : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Dragon's Breath");
			Tooltip.SetDefault("Shoots Shadow Flames\n70% chance to not consume gel");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 28; int height = 30;
			Item.Size = new Vector2(width, height);

			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;

			Item.useTime = 3;
			Item.useAnimation = 18;

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.ShadowFlames>();
			Item.shootSpeed = 6.5f;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.useAmmo = AmmoID.Gel;

			Item.value = Item.sellPrice(gold: 5, silver: 60);
			Item.rare = ItemRarityID.Lime;

			Item.UseSound = SoundID.Item24;
			Item.autoReuse = true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += _velocity * 7;
			position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
		}

		public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int _randomCount = Main.rand.Next(1, 4);
			for (int i = 0; i < _randomCount; i++) {
				float _randomVel = Main.rand.Next(-15, 15) * 0.035f;
				velocity += new Vector2(_randomVel, _randomVel);
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override Vector2? HoldoutOffset()
			=> new Vector2(-6, 0);

		public override bool CanConsumeAmmo(Player player)
			=> Main.rand.NextFloat() >= 0.7f;	
	}
}
