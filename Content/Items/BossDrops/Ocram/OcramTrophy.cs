using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Ocram
{
    public class OcramTrophy : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ocram Trophy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults() {
            int width = 32; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.OcramTrophy>());
        }
    }
}
