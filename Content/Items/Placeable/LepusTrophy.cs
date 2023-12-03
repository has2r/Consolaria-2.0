using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Consolaria.Content.Items.Placeable
{
    public class LepusTrophy : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 32; int height = 40;
            Item.Size = new Vector2(width, height);

            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LepusTrophy>());

            Item.maxStack = 9999;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
        }
    }
}
