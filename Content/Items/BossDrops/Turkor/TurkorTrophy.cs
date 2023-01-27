using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Turkor
{
    public class TurkorTrophy : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Turkor the Ungrateful Trophy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults() {
            int width = 32; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TurkorTrophy>());
        }
    }
}
