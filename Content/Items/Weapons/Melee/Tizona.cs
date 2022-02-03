using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Melee
{
	public class Tizona : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Tizona");
			Tooltip.SetDefault("Shoots a cursed skull that inflicts enemies with Shadowflames");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 48; int height = width;
			Item.Size = new Vector2(width, height);

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 18;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.knockBack = 5;
			Item.crit = 5;

			Item.value = Item.buyPrice(gold: 5, silver: 50);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item79;

			Item.shoot = ModContent.ProjectileType<DreadSkull>(); 
			Item.shootSpeed = 8f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.Next(4) == 0)
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame, 0, 0, 100, default, 0.9f);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Excalibur)
				.AddIngredient(ItemID.HellstoneBar, 15)
				.AddIngredient<SoulofBlight>(15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}