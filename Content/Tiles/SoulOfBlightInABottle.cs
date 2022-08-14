using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles {
    public class SoulOfBlightInABottle : ModTile {
        public override void SetStaticDefaults () {
            Main.tileLighted [Type] = true;
            Main.tileFrameImportant [Type] = true;
            Main.tileLavaDeath [Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int [] { 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);

            AdjTiles = new int [] { TileID.SoulBottles };

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Soul of Blight in a Bottle");
            AddMapEntry(new Color(238, 145, 105), name);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileID.Sets.DisableSmartCursor [Type] = true;
        }

        private readonly int animationFrameWidth = 18;

        public override void ModifyLight (int i, int j, ref float r, ref float g, ref float b) {
            r = 0.8f;
            g = 0.7f;
            b = 0.75f;
        }

        public override void SetSpriteEffects (int i, int j, ref SpriteEffects spriteEffects) {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
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
            if (frameCounter >= 6) {
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

        public override void KillMultiTile (int i, int j, int frameX, int frameY)
            => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Consumables.SoulOfBlightInABottle>());
    }
}