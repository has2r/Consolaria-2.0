using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable.Banners;

public sealed class AlbinoSwarmerBanner : ModItem {
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners>(), 20);
        Item.width = 12;
        Item.height = 28;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
    }
}
