using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class GoldenSeaweed : PetItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.GoldenTurtle>(), ModContent.BuffType<Buffs.GoldenTurtle>());

			int width = 22; int height = 28;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 3);
		}
	}
}