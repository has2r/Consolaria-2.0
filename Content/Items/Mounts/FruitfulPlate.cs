using Consolaria.Content.Mounts;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Mounts {
    public class FruitfulPlate : ModItem {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.useTime = Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.value = Item.sellPrice(gold: 5);
            Item.master = true;

            Item.UseSound = SoundID.Item2;

            Item.mountType = ModContent.MountType<FruitfulPlateMount>();
        }
    }
}