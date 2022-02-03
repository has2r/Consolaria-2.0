using Terraria.ModLoader;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Lepus
{
    public class LepusTrophy : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lepus Trophy");
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.scale = 0.5f;
            Item.value = 10000;
            Item.createTile = ModContent.TileType<Tiles.LepusTrophy>();
            Item.placeStyle = 0;
        }
    }
}
