using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles {
    public class OtherworldlyMusicBox2 : ModTile {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);

            TileID.Sets.HasOutlines[Type] = true;

            AdjTiles = new int[] { TileID.MusicBoxes };

            AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.MusicBox"));
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) {
            if (Main.tile[i, j].TileFrameX == 36 && (int)Main.timeForVisualEffects % 7 == 0 && Main._rand.NextBool(3)) {
                int MusicNote = Main._rand.Next(570, 573);
                Vector2 SpawnPosition = new((float)(i * 16 + 8), (float)(j * 16 - 8));
                Vector2 NoteMovement = new(Main.WindForVisuals * 2f, -0.5f);
                NoteMovement.X *= 1f + (float)Main._rand.Next(-50, 51) * 0.01f;
                NoteMovement.Y *= 1f + (float)Main._rand.Next(-50, 51) * 0.01f;
                if (MusicNote == 572) {
                    SpawnPosition.X -= 8f;
                }
                if (MusicNote == 571) {
                    SpawnPosition.X -= 4f;
                }
                Gore.NewGore(new EntitySource_Misc(""), SpawnPosition, NoteMovement, MusicNote, 0.8f);
            }
        }

        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.OtherworldlyMusicBox2>();
        }
    }
}