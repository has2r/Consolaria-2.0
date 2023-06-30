using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles {
	public class SanctumLantern : ModTile {
		public override void SetStaticDefaults () {
			Main.tileFrameImportant [Type] = true;
			Main.tileLavaDeath [Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int [] { 16, 16 };
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(238, 145, 105), name);

			TileID.Sets.DisableSmartCursor [Type] = true;
		}

		public override void NumDust (int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override bool KillSound (int i, int j, bool fail) {
			if (!fail) {
				SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i, j).ToWorldCoordinates());
				return false;
			}
			return base.KillSound(i, j, fail);
		}

		public override void KillMultiTile (int i, int j, int TileFrameX, int TileFrameY) {
			int item = Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 34, 38, ItemID.CarriageLantern, 1, false, 0, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
		}
	}
}