using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Consolaria.Content.Structures
{
    public class HeartShrine : ModSystem
    {
        internal static readonly int[] SnowGenTiles =
        {
            147,
            161,
            163,
            200
        };

        private int HeartShrinePositionX = 0;
        private int HeartShrinePositionY = 0;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (genIndex != -1)
            {
                tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
                tasks.Insert(genIndex + 0, new PassLegacy("[Consolaria] Heart Shrine", delegate
                {
                   /* int XX = Main.spawnTileX + 100;
                    int YY = 200;
                    while (WorldGen.TileEmpty(XX, YY)) YY++;
                    
                    YY -= 20;
                    int StartX = XX;
                    int StartY = YY;*/
                    int[,] literallyHeartShrine = new int[,] {{ 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                                                          { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                                                          { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 } ,
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                          { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                          { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                                                          { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },};



                    List<Point> list = new List<Point>();
                    foreach (int k in SnowGenTiles)
                    {
                        for (int i = Main.maxTilesX / 5; i < Main.maxTilesX / 5 * 4; i++)
                        {
                            int y = 200;
                            while (!WorldGen.SolidOrSlopedTile(i, y + 1))
                                y++;
                            bool canBeGenerated = true;
                            for (int j = 0; j < 33; j++)
                                if (!WorldGen.SolidOrSlopedTile(i + j, y + 1) || Main.tile[i + j, y + 1].TileType != k)
                                    canBeGenerated = false;
                            if (canBeGenerated)
                                for (int a = 0; a < 17; a++)
                                    if (WorldGen.SolidOrSlopedTile(i - 1, y - a) || WorldGen.SolidOrSlopedTile(i + 33, y - a))
                                        canBeGenerated = false;
                            if (canBeGenerated)
                                list.Add(new Point(i, y));
                        }
                        if (list.Count > 0)
                        {
                            Point point = list[WorldGen.genRand.Next(0, list.Count)];
                            HeartShrinePositionX = point.X;
                            HeartShrinePositionY = point.Y;
                            goto GenerateBuild;
                        }
                    }

                    GenerateBuild:
                    for (var X = 0; X < literallyHeartShrine.GetLength(1); X++)
                    {
                        for (var Y = 0; Y < literallyHeartShrine.GetLength(0); Y++)
                        {
                            var tile = Framing.GetTileSafely(HeartShrinePositionX + X, HeartShrinePositionY - Y);
                            switch (literallyHeartShrine[Y, X])
                            {
                                case 0:
                                    break;
                                case 1:
                                    WorldGen.KillTile(HeartShrinePositionX + X, HeartShrinePositionY + Y);
                                   // WorldGen.KillWall(HeartShrinePositionX + X, HeartShrinePositionY + Y);
                                    WorldGen.paintTile(HeartShrinePositionX + X, HeartShrinePositionY + Y, PaintID.PinkPaint);
                                    break;
                                case 2:
                                    WorldGen.KillTile(HeartShrinePositionX + X, HeartShrinePositionY + Y);
                                   // WorldGen.KillWall(HeartShrinePositionX + X, HeartShrinePositionY + Y);
                                    WorldGen.PlaceTile(HeartShrinePositionX + X, HeartShrinePositionY + Y, TileID.SnowBlock);
                                    break;
                            }
                        }
                    }
                    WorldGen.AddLifeCrystal(HeartShrinePositionX + 10, HeartShrinePositionY + 10);
                    WorldGen.AddLifeCrystal(HeartShrinePositionX + 24, HeartShrinePositionY + 10);

                    WorldGen.PlaceChest(HeartShrinePositionX + 16, HeartShrinePositionY + 15, 21, false, 11);
                    int chestIndex = Chest.FindChest(HeartShrinePositionX + 16, HeartShrinePositionY + 15);
                    if (chestIndex != -1)
                        AddHeartShrineLoot(Main.chest[chestIndex].item);
                }));
            }
        }

        private void AddHeartShrineLoot(Item[] chestInventory){
            int CastlecurrentIndex = 0;
            chestInventory[CastlecurrentIndex].SetDefaults(ItemID.CopperCoin); chestInventory[CastlecurrentIndex].stack = 69; CastlecurrentIndex++;
        }     
    }
}