using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Turkor {
    public class FruitfulPlate : ModItem {
		public override void SetStaticDefaults () {
			// DisplayName.SetDefault("Fruitful Plate");
			// Tooltip.SetDefault("Summons a fruitful plate mount");

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = width;
			Item.Size = new Vector2(width, height);

			Item.useTime = Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.value = Item.sellPrice(gold: 5);
			Item.master = true;

			Item.UseSound = SoundID.Item2;

			Item.mountType = ModContent.MountType<Mounts.FruitfulPlate>();
		}
	}
}