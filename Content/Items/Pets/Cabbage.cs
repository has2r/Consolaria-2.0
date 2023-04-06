using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class Cabbage : PetItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			Item.CloneDefaults(ItemID.Carrot); Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.GuineaPig>(), ModContent.BuffType<Buffs.GuineaPig>());

			int width = 30; int height = width;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 3);
		}
	}
}