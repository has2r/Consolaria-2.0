using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class PetriDish : PetItem {
		public override void SetStaticDefaults () {
			Tooltip.SetDefault("Summons a pet Slime");
			SacrificeTotal = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.Slime>(), ModContent.BuffType<Buffs.Slime>());

			int width = 22; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);
		}
	}
}