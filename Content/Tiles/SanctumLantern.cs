using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles {
	public class SanctumLantern : ModTile {
		public override void SetStaticDefaults () {
			Main.tileFrameImportant [Type] = true;

			TileObjectData.addTile(Type);

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int [] { 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Carriage Lantern");
			AddMapEntry(new Color(238, 145, 105), name);

			TileID.Sets.DisableSmartCursor [Type] = true;
		}

		public override void ModifyLight (int i, int j, ref float r, ref float g, ref float b) {
			Tile tile = Main.tile [i, j];
			if (tile.TileFrameX < 18) {
				r = 0.8f;
				g = 0.6f;
				b = 0.7f;
			}
		}

		public override void NumDust (int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void KillMultiTile (int i, int j, int TileFrameX, int TileFrameY) {
			int item = Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 34, 38, ItemID.CarriageLantern, 1, false, 0, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
		}

		public override void HitWire (int i, int j) {
			int x = i - Main.tile [i, j].TileFrameX / 18 % 1;
			int y = j - Main.tile [i, j].TileFrameY / 18 % 2;
			for (int k = x; k < x + 1; k++) {
				for (int l = y; l < y + 2; l++) {
					if (Main.tile [k, l].HasTile && Main.tile [k, l].TileType == Type) {
						if (Main.tile [k, l].TileFrameX != 36) {
							Tile tile = Main.tile [k, l];
							tile.TileFrameX += 18;
						}
						if (Main.tile [k, l].TileFrameX >= 36) {
							Tile tile2 = Main.tile [k, l];
							tile2.TileFrameX -= 36;
						}
					}
				}
			}

			if (Wiring.running) {
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3, TileChangeType.None);
		}
	}
}