using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged
{
	public class EggCannon : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Egg Cannon");
			Tooltip.SetDefault("'To kill a goblin, you have to break a few eggs'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 56; int height = 20;
			Item.Size = new Vector2(width, height);

			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.knockBack = 0.5f;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = Item.useTime = 22;
			Item.autoReuse = true;

			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.UseSound = SoundID.Item11;

			Item.shoot = ModContent.ProjectileType<EasterEgg>();
			Item.shootSpeed = 8f;
		}

		public override Vector2? HoldoutOffset()
		 => new Vector2(-8, 0);
		
		public override bool AltFunctionUse(Player player) {
			if (player.velocity.Y == 0) return true;
			else return false;		
		}

		public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (player.altFunctionUse == 2) {
				player.velocity.Y = -16;
				int _bonusDustCount = 0;
				for (int _dustPosition = (int)player.position.X - 20; _dustPosition < (int)player.position.X + player.width + 40; _dustPosition += 20) {
					for (int _dustCount = 0; _dustCount < 4; _dustCount = _bonusDustCount + 1) {
						int _dust = Dust.NewDust(new Vector2(player.position.X - 20f, player.position.Y + player.height), player.width + 20, 4, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
						Dust _dust2 = Main.dust[_dust];
						_dust2.velocity *= 0.2f;
						_bonusDustCount = _dustCount;
					}
					int _gore = Gore.NewGore(new Vector2((_dustPosition - 20), player.position.Y + player.height - 8f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Gore gore = Main.gore[_gore];
					gore.velocity *= 0.4f;
				}
				for (int k = 0; k < 200; k++) {
					if (Main.npc[k].Distance(player.Center) <= 140)
						Main.npc[k].StrikeNPCNoInteraction(damage * 3, 0.0f, 0, false, false, false);	
				}
				SoundEngine.PlaySound(SoundID.Item14, player.position);
				return false;
			}
			else {
				Item.damage = 20;
		    	Item.noMelee = true;
				type = ModContent.ProjectileType<EasterEgg>();
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
