using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Pets
{
	public class ShinyBlackSlab : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiny Black Slab");
			Tooltip.SetDefault("Summons a pet android");
		}

		public override void SetDefaults() 
		{
			Item.useStyle = 1;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 2;
			Item.damage = 0;
			Item.stack = 1;
			Item.noMelee = true;
			Item.rare = 5;
			Item.value = Item.buyPrice(gold: 10);
			Item.UseSound = SoundID.Item2;
			Item.shoot = ModContent.ProjectileType<Content.Projectiles.Friendly.Pets.AndroidGuy>();
			Item.buffType = ModContent.BuffType<Content.Buffs.Android>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) 
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) 
			{
				player.AddBuff(Item.buffType, 3600);
			}
		}

		public override void AddRecipes()
		{

		}
	}
}