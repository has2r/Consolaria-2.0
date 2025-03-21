using Consolaria.Content.Items.Pets;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class TileDrops : GlobalTile {
        public override void Drop (int i, int j, int type) {
            if (Main.xMas && type == TileID.Trees) {
                if (Main.rand.NextBool(750)) {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<HolidayBauble>());
                }
            }
        }
    }
}