using Consolaria.Content.Items.Placeable;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles;

public class WormTombstoneTile : ModTile {
    public override void SetStaticDefaults() {
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        Main.tileSign[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        AddMapEntry(new Color(192, 192, 192), Language.GetText("ItemName.Tombstone"));

        DustType = DustID.Stone;

        AdjTiles = new int[] { TileID.Tombstones };

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.DrawYOffset = 2;

        TileObjectData.addTile(Type);
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        => true;

    public override bool RightClick(int i, int j)
        => true;

    public override void NumDust(int i, int j, bool fail, ref int num)
        => num = fail ? 1 : 3;

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
        => Sign.KillSign(i, j);

    public override IEnumerable<Item> GetItemDrops(int i, int j) {
        yield return new Item(ModContent.ItemType<WormTombstoneItem>());
    }

    public override void MouseOverFar(int i, int j)
        => MouseOver(i, j);
}