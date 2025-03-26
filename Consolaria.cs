using Consolaria.Common;
using Consolaria.Content.Items.Placeable;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Projectiles.Friendly;
using Consolaria.Content.Projectiles.Friendly.Pets;
using Consolaria.Content.Tiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Reflection;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

using static Terraria.Graphics.FinalFractalHelper;

namespace Consolaria {
    public partial class Consolaria : Mod {
        public override void Load() {
            if (Main.dedServ) {
                return;
            }

            Helper.Load(); //We load and unload the wind reflections to make sure they are properly reflected

            TextureAssets.XmasTree[3] = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Tiles/Xmas_3");

            var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            if (ModContent.GetInstance<ConsolariaConfig>().tizonaZenithIntegrationEnabled)
                fractalProfiles.Add(ModContent.ItemType<Tizona>(), new FinalFractalProfile(70f, new Color(132, 122, 224))); //Color up for debate

            On_Player.DropTombstone += On_Player_DropTombstone;
            On_WorldGen.dropXmasTree += On_WorldGen_dropXmasTree;
            On_SceneMetrics.ExportTileCountsToMain += On_SceneMetrics_ExportTileCountsToMain;

            On_TileDrawing.DrawMultiTileVinesInWind += On_TileDrawing_DrawMultiTileVinesInWind;
        }


        public override void Unload() {

            Helper.Unload();

            var fractalProfiles = (Dictionary<int, FinalFractalProfile>)typeof(FinalFractalHelper).GetField("_fractalProfiles", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            fractalProfiles.Remove(ModContent.ItemType<Tizona>());

            On_Player.DropTombstone -= On_Player_DropTombstone;
            On_WorldGen.dropXmasTree -= On_WorldGen_dropXmasTree;
            On_SceneMetrics.ExportTileCountsToMain -= On_SceneMetrics_ExportTileCountsToMain;

            On_TileDrawing.DrawMultiTileVinesInWind -= On_TileDrawing_DrawMultiTileVinesInWind;
        }

        //We reflect this hook so we are able to list the length/width of our blowing wind tile, Chandiliers, haning bottles, banners and more use this (Vines, double tall grass, ect do NOT use this. Dont attempt to fit them here)
        private void On_TileDrawing_DrawMultiTileVinesInWind(On_TileDrawing.orig_DrawMultiTileVinesInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
            if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<Banners>()) //Visit each of these tiles to see how predraw, draws them blowing in the wind
            {
                sizeX = 1;
                sizeY = 3;
            }
            else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<SanctumLantern>()) {
                sizeX = 1;
                sizeY = 2;
            }
            else if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<SoulOfBlightInABottleTile>()) {
                sizeX = 1;
                sizeY = 2;
            }
            orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
        }

        private void On_SceneMetrics_ExportTileCountsToMain(On_SceneMetrics.orig_ExportTileCountsToMain orig, SceneMetrics self) {
            orig(self);

            Point point = Main.LocalPlayer.Center.ToTileCoordinates();
            int extraTombstones = 0;
            Rectangle tileRectangle = new(point.X - Main.buffScanAreaWidth / 2, point.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight);
            tileRectangle = WorldUtils.ClampToWorld(tileRectangle);
            for (int i = tileRectangle.Left; i < tileRectangle.Right; i++) {
                for (int j = tileRectangle.Top; j < tileRectangle.Bottom; j++) {
                    if (!tileRectangle.Contains(i, j))
                        continue;

                    if (Main.tile[i, j].TileType == ModContent.TileType<WormTombstoneTile>()) {
                        extraTombstones++;
                    }
                }
            }

            self.GraveyardTileCount += extraTombstones;

            if (self.GraveyardTileCount > SceneMetrics.GraveyardTileMin) {
                self.HasSunflower = false;
            }
        }

        private void On_Player_DropTombstone(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                return;
            }
            if (self.GetModPlayer<WormData>().IsWormPetActive) {
                Vector2 GetRandomTombstoneVelocity(int hitDirection) {
                    float num;
                    for (num = Main.rand.Next(-35, 36) * 0.1f; num < 2f && num > -2f; num += Main.rand.Next(-30, 31) * 0.1f) {
                    }
                    return new Vector2(Main.rand.Next(10, 30) * 0.1f * hitDirection + num,
                                       Main.rand.Next(-40, -20) * 0.1f);
                }

                int projectile = Projectile.NewProjectile(new EntitySource_Death(self), self.Center, GetRandomTombstoneVelocity(hitDirection), ModContent.ProjectileType<WormTombstone>(), 0, 0f, Main.myPlayer);
                DateTime now = DateTime.Now;
                string str = now.ToString("D");
                if (GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) {
                    str = now.ToString("MMMM d, yyy");
                }
                string miscText = deathText.ToString() + "\n" + str;
                Main.projectile[projectile].miscText = miscText;
                return;
            }
            orig(self, coinsOwned, deathText, hitDirection);
        }

        private void On_WorldGen_dropXmasTree(On_WorldGen.orig_dropXmasTree orig, int x, int y, int obj) {
            int num = x;
            int num2 = y;
            if (Main.tile[x, y].TileFrameX < 10) {
                num -= Main.tile[x, y].TileFrameX;
                num2 -= Main.tile[x, y].TileFrameY;
            }

            int num3 = 0;
            if ((Main.tile[num, num2].TileFrameY & 1) == 1)
                num3++;

            if ((Main.tile[num, num2].TileFrameY & 2) == 2)
                num3 += 2;

            if ((Main.tile[num, num2].TileFrameY & 4) == 4)
                num3 += 4;

            int num4 = 0;
            if ((Main.tile[num, num2].TileFrameY & 8) == 8)
                num4++;

            if ((Main.tile[num, num2].TileFrameY & 0x10) == 16)
                num4 += 2;

            if ((Main.tile[num, num2].TileFrameY & 0x20) == 32)
                num4 += 4;

            int num5 = 0;
            if ((Main.tile[num, num2].TileFrameY & 0x40) == 64)
                num5++;

            if ((Main.tile[num, num2].TileFrameY & 0x80) == 128)
                num5 += 2;

            if ((Main.tile[num, num2].TileFrameY & 0x100) == 256)
                num5 += 4;

            if ((Main.tile[num, num2].TileFrameY & 0x200) == 512)
                num5 += 8;

            int num6 = 0;
            if ((Main.tile[num, num2].TileFrameY & 0x400) == 1024)
                num6++;

            if ((Main.tile[num, num2].TileFrameY & 0x800) == 2048)
                num6 += 2;

            if ((Main.tile[num, num2].TileFrameY & 0x1000) == 4096)
                num6 += 4;

            if ((Main.tile[num, num2].TileFrameY & 0x2000) == 8192)
                num6 += 8;

            if (obj == 0 && num3 > 0) {
                int number = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, num3 == 5 ? ModContent.ItemType<Topper505>() : num3 == 6 ? ModContent.ItemType<StarTopper4>() : 1874 + num3 - 1);
                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, number, 1f);
            }
            else if (obj == 1 && num4 > 0) {
                int number2 = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, 1878 + num4 - 1);
                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, number2, 1f);
            }
            else if (obj == 2 && num5 > 0) {
                int number3 = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, 1884 + num5 - 1);
                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, number3, 1f);
            }
            else if (obj == 3 && num6 > 0) {
                int number4 = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, 1895 + num6 - 1);
                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, number4, 1f);
            }
        }
    }
}