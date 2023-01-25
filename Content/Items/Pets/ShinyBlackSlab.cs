using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets 
{
	public class ShinyBlackSlab : PetItem 
	{
		public override void SetStaticDefaults () 
		{
			DisplayName.SetDefault("Shiny Black Slab");
			Tooltip.SetDefault("Summons a pet android");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Content.Projectiles.Friendly.Pets.AndroidGuy>(), ModContent.BuffType<Content.Buffs.Android>());

			int width = 32; int height = 32;
			Item.Size = new Vector2(width, height);

			Item.rare = 5;
			Item.value = Item.sellPrice(gold: 10);
		}
	}
}