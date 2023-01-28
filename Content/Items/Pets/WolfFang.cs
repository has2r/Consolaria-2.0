using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class WolfFang : PetItem {
		public override void SetStaticDefaults () {
			Tooltip.SetDefault("Summons a pet Werewolf");
			SacrificeTotal = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.Werewolf>(), ModContent.BuffType<Buffs.Werewolf>());

			int width = 30; int height = width;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 3);
		}
	}
}