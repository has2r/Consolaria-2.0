using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Consolaria.Content.Items.Weapons.Summon
{
	public class EternityStaff : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Eternity Staff");
			Tooltip.SetDefault("Summons an Eye of Eternity to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 36; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20; 

			Item.mana = 25;
			Item.noMelee = true;

			Item.DamageType = DamageClass.Summon; 
			Item.damage = 60; 
			Item.knockBack = 4f;

			Item.value = Item.sellPrice(gold: 5, silver: 75);
			Item.rare = ItemRarityID.Lime; 
			Item.UseSound = SoundID.Item44;

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.EyeOfEternity>();
		}

		public override bool AltFunctionUse(Player player)
			=> true;

        public override bool? UseItem(Player player)  {
			if (player.altFunctionUse == 2) {
				player.statMana += Item.mana;
				player.MinionNPCTargetAim(true);
			}
			return base.UseItem(player);
		}

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 spawnPos = Main.MouseWorld;
			var projectile = Projectile.NewProjectileDirect(source, spawnPos, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			if (player.altFunctionUse != 2) {
				player.AddBuff(ModContent.BuffType<Buffs.EyeOfEternity>(), 10);
				position = spawnPos;
			}
			return player.altFunctionUse != 2;
		}
    }
}
