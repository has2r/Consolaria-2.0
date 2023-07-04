using Consolaria.Content.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable;

public class WormTombstoneItem : ModItem {
    public override string Texture
        => $"Consolaria/Content/Projectiles/WormTombstone";

    public override void SetDefaults() {
        Item.CloneDefaults(ItemID.Tombstone);
        Item.rare = ItemRarityID.Yellow;

        Item.DefaultToPlaceableTile(ModContent.TileType<WormTombstoneTile>());
    }
}