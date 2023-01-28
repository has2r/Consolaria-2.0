using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Weapons.Melee {
    public class AlbinoMandible : ModItem {
		public override void SetStaticDefaults () {
			Tooltip.SetDefault("'Surprisingly aerodynamic...'");
			SacrificeTotal = 1;
		}

		public override void SetDefaults () {
			int width = 32; int height = 16;
			Item.Size = new Vector2(width, height);

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 14;

			Item.autoReuse = true;
			Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 14;

			Item.knockBack = 4;
			Item.noMelee = true;

			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Green;

			Item.UseSound = SoundID.Item1;

			Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.AlbinoMandible>();
			Item.shootSpeed = 14f;
		}

		public override bool CanUseItem (Player player)
			=> player.ownedProjectileCounts [Item.shoot] < 1;
	}
}