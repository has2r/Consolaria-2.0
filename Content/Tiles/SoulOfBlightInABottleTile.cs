using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles
{
    public class SoulOfBlightInABottleTile : ModTile {
        public override void SetStaticDefaults () {
            Main.tileLighted [Type] = true;
            Main.tileFrameImportant [Type] = true;
            Main.tileLavaDeath [Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int [] { 16, 18};
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
            TileObjectData.newAlternate.DrawYOffset = -8;
            TileObjectData.newAlternate.DrawFlipHorizontal = false;
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(Type);

            AdjTiles = new int [] { TileID.SoulBottles };

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(238, 145, 105), name);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileID.Sets.DisableSmartCursor [Type] = true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            bool intoRenderTargets = true;
            bool flag = intoRenderTargets || Main.LightingEveryFrame;

            if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 36 == 0 && flag)
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
            }

            return false;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
            if ((Framing.GetTileSafely(i, j - 1).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 1).TileType]) ||
                (Framing.GetTileSafely(i, j - 2).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 2).TileType]) ||
                (Framing.GetTileSafely(i, j - 3).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 3).TileType])) {
				offsetY += -10;
            }
        }

        private readonly int animationFrameWidth = 18;

        public override void ModifyLight (int i, int j, ref float r, ref float g, ref float b) {
            r = 0.8f;
            g = 0.7f;
            b = 0.75f;
        }

        public override void AnimateIndividualTile (int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
            int uniqueAnimationFrame = Main.tileFrame [Type] + i;
            if (i % 1 == 0)
                uniqueAnimationFrame += 2;
            if (i % 3 == 0)
                uniqueAnimationFrame += 2;
            uniqueAnimationFrame %= 3;

            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void AnimateTile (ref int frame, ref int frameCounter) {
            frameCounter++;
            if (frameCounter >= 7.2) {
                frameCounter = 0;
                if (++frame >= 3) {
                    frame = 0;
                }
            }
        }

        public override bool KillSound (int i, int j, bool fail) {
            if (!fail) {
                SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i, j).ToWorldCoordinates());
                return false;
            }
            return base.KillSound(i, j, fail);
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j){
            yield return new Item(ModContent.ItemType<Items.Placeable.SoulOfBlightInABottle>());
        }
    }
}