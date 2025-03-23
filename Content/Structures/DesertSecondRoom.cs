using Consolaria.Common;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Consolaria.Content.Structures;

sealed class DesertSecondRoom : ILoadable {
    private class DesertSecondRoom_ReplaceVanillaPass : ModSystem {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
            int genIndexToReplace = tasks.FindIndex(genpass => genpass.Name.Equals("Pyramids"));
            tasks.RemoveAt(genIndexToReplace);
            tasks.Insert(genIndexToReplace, new PassLegacy("Pyramids", Pyramids, 0.3045f));
        }

        private void Pyramids(GenerationProgress progress, GameConfiguration passConfig) {
            Rectangle undergroundDesertLocation = GenVars.UndergroundDesertLocation;

            if (Main.tenthAnniversaryWorld || ModContent.GetInstance<ConsolariaConfig>().pyramidEnabled) {
                int x15 = undergroundDesertLocation.Center.X;
                int j4 = undergroundDesertLocation.Top - 10;
                WorldGen.Pyramid(x15, j4);
            }

            for (int num689 = 0; num689 < GenVars.numPyr; num689++) {
                int num690 = GenVars.PyrX[num689];
                int num691 = GenVars.PyrY[num689];
                if (num690 > 300 && num690 < Main.maxTilesX - 300 && (GenVars.dungeonSide >= 0 || !((double)num690 < (double)GenVars.dungeonX + (double)Main.maxTilesX * 0.15)) && (GenVars.dungeonSide <= 0 || !((double)num690 > (double)GenVars.dungeonX - (double)Main.maxTilesX * 0.15)) && (!Main.tenthAnniversaryWorld || !undergroundDesertLocation.Contains(num690, num691))) {
                    for (; !Main.tile[num690, num691].HasTile && (double)num691 < Main.worldSurface; num691++) {
                    }

                    if (!((double)num691 >= Main.worldSurface) && Main.tile[num690, num691].TileType == 53) {
                        int num692 = Main.maxTilesX;
                        for (int num693 = 0; num693 < num689; num693++) {
                            int num694 = Math.Abs(num690 - GenVars.PyrX[num693]);
                            if (num694 < num692)
                                num692 = num694;
                        }

                        int num695 = 220;
                        if (WorldGen.drunkWorldGen)
                            num695 /= 2;

                        if (num692 >= num695) {
                            num691--;
                            WorldGen.Pyramid(num690, num691);
                        }
                    }
                }
            }
        }
    }

    void ILoadable.Load(Mod mod) {
        On_WorldGen.Pyramid += On_WorldGen_Pyramid;
    }

    private bool On_WorldGen_Pyramid(On_WorldGen.orig_Pyramid orig, int i, int j) {
        UnifiedRandom genRand = WorldGen.genRand;

        ushort num = 151;
        if (Main.tile[i, j].TileType == 151 || Main.tile[i, j].WallType == 151) {
            return false;
        }

        // Base placement
        int num2 = j - genRand.Next(0, 7);
        int num3 = genRand.Next(9, 13);
        int num4 = 1;
        int num5 = j + genRand.Next(75, 125);
        for (int k = num2; k < num5; k++) {
            for (int l = i - num4; l < i + num4 - 1; l++) {
                Tile tile = Main.tile[l, k];
                tile.TileType = num;
                tile.HasTile = true;
                tile.IsHalfBlock = false;
                tile.Slope = 0;
            }

            num4++;
        }

        for (int m = i - num4 - 5; m <= i + num4 + 5; m++) {
            for (int n = j - 1; n <= num5 + 1; n++) {
                bool flag = true;
                for (int num6 = m - 1; num6 <= m + 1; num6++) {
                    for (int num7 = n - 1; num7 <= n + 1; num7++) {
                        if (Main.tile[num6, num7].TileType != num)
                            flag = false;
                    }
                }

                if (flag) {
                    Main.tile[m, n].WallType = 34;
                    WorldGen.SquareWallFrame(m, n);
                }
            }
        }

        // Entrance
        int direction = 1;
        if (genRand.Next(2) == 0)
            direction = -1;

        int x = i - num3 * direction;
        int num10 = j + num3;
        int num11 = genRand.Next(5, 8);
        bool flag2 = true;
        int num12 = genRand.Next(20, 30);
        while (flag2) {
            flag2 = false;
            bool flag3 = false;
            for (int num13 = num10; num13 <= num10 + num11; num13++) {
                int num14 = x;
                if (Main.tile[num14, num13 - 1].TileType == 53)
                    flag3 = true;

                if (Main.tile[num14, num13].TileType == num) {
                    Main.tile[num14, num13 + 1].WallType = 34;
                    Main.tile[num14 + direction, num13].WallType = 34;
                    Tile tile = Main.tile[num14, num13];
                    tile.HasTile = false;
                    flag2 = true;
                }

                if (flag3) {
                    Tile tile = Main.tile[num14, num13];
                    tile.TileType = 53;
                    tile.HasTile = true;
                    tile.IsHalfBlock = false;
                    tile.Slope = 0;
                }
            }

            x -= direction;
        }

        // Main tunnel
        x = i - num3 * direction;
        bool flag4 = true;
        bool flag5 = false;
        flag2 = true;
        while (flag2) {
            for (int num15 = num10; num15 <= num10 + num11; num15++) {
                int num16 = x;
                Tile tile = Main.tile[num16, num15];
                tile.HasTile = false;
            }

            x += direction;
            num10++;
            num12--;
            if (num10 >= num5 - num11 * 2)
                num12 = 10;

            if (num12 <= 0) {
                bool flag6 = false;
                if (!flag4 && !flag5) {
                    flag5 = true;
                    flag6 = true;

                    // Loot room size
                    int lootRoomSizeX = genRand.Next(7, 13);
                    int lootRoomSizeY = genRand.Next(23, 28);

                    int num19 = lootRoomSizeY;
                    int num20 = x;
                    while (lootRoomSizeY > 0) {
                        for (int y = num10 - lootRoomSizeX + num11; y <= num10 + num11; y++) {
                            if (lootRoomSizeY == num19 || lootRoomSizeY == 1) {
                                if (y >= num10 - lootRoomSizeX + num11 + 2) {
                                    // Entrance to main room
                                    Tile tile = Main.tile[x, y];
                                    tile.HasTile = false;
                                }
                            }
                            else if (lootRoomSizeY == num19 - 1 || lootRoomSizeY == 2 || lootRoomSizeY == num19 - 2 || lootRoomSizeY == 3) {
                                if (y >= num10 - lootRoomSizeX + num11 + 1) {
                                    Tile tile = Main.tile[x, y];
                                    tile.HasTile = false;
                                }
                            }
                            else {
                                // Loot room
                                Tile tile = Main.tile[x, y];
                                tile.HasTile = false;
                            }
                        }

                        lootRoomSizeY--;
                        x += direction;
                    }

                    // Second pyramid loot room
                    int lootRoomSizeX2 = genRand.Next(6, 8);
                    int lootRoomSizeY2 = genRand.Next(18, 25);
                    lootRoomSizeY2 -= genRand.Next(lootRoomSizeY2 / 4, lootRoomSizeY2 / 3);

                    int num19_2 = lootRoomSizeY2;
                    int xOffsetExtra = genRand.Next(50, 80);
                    int xOffset = xOffsetExtra * -direction;
                    int x2 = x + xOffset;
                    int yOffsetExtra = genRand.Next(num19_2 / 6, num19_2 - 5);
                    while (lootRoomSizeY2 > 0) {
                        for (int y = num10 - lootRoomSizeX2 + num11; y <= num10 + num11 + 1; y++) {
                            int y2 = y + yOffsetExtra;
                            if (lootRoomSizeY2 == num19_2 || lootRoomSizeY2 == 1) {
                                if (y >= num10 - lootRoomSizeX2 + num11 + 2) {
                                    // Entrance to main room
                                    Tile tile = Main.tile[x2, y2];
                                    tile.HasTile = false;
                                }
                            }
                            else if (lootRoomSizeY2 == num19_2 - 1 || lootRoomSizeY2 == 2 || lootRoomSizeY2 == num19_2 - 2 || lootRoomSizeY2 == 3) {
                                if (y >= num10 - lootRoomSizeX2 + num11 + 1) {
                                    Tile tile = Main.tile[x2, y2];
                                    tile.HasTile = false;
                                }
                            }
                            else {
                                // Loot room
                                Tile tile = Main.tile[x2, y2];
                                tile.HasTile = false;
                            }
                        }

                        lootRoomSizeY2--;
                        x2 += direction;
                    }

                    // Main loot room content
                    int num22 = x - direction;
                    int num23 = num22;
                    int num24 = num20;
                    if (num22 > num20) {
                        num23 = num20;
                        num24 = num22;
                    }

                    int num25 = genRand.Next(3);
                    if (num25 == 0)
                        num25 = genRand.Next(3);

                    if (Main.tenthAnniversaryWorld && num25 == 0)
                        num25 = 1;

                    switch (num25) {
                        case 0:
                            num25 = 848;
                            break;
                        case 1:
                            num25 = 857;
                            break;
                        case 2:
                            num25 = 934;
                            break;
                    }

                    WorldGen.AddBuriedChest((num23 + num24) / 2, num10, num25, notNearOtherChests: false, 1, trySlope: false, 0);
                    int num26 = genRand.Next(1, 10);
                    for (int num27 = 0; num27 < num26; num27++) {
                        int i2 = genRand.Next(num23, num24);
                        int j2 = num10 + num11;
                        WorldGen.PlaceSmallPile(i2, j2, genRand.Next(16, 19), 1, 185);
                    }

                    WorldGen.PlaceTile(num23 + 2, num10 - lootRoomSizeX + num11 + 1, 91, mute: true, forced: false, -1, genRand.Next(4, 7));
                    WorldGen.PlaceTile(num23 + 3, num10 - lootRoomSizeX + num11, 91, mute: true, forced: false, -1, genRand.Next(4, 7));
                    WorldGen.PlaceTile(num24 - 2, num10 - lootRoomSizeX + num11 + 1, 91, mute: true, forced: false, -1, genRand.Next(4, 7));
                    WorldGen.PlaceTile(num24 - 3, num10 - lootRoomSizeX + num11, 91, mute: true, forced: false, -1, genRand.Next(4, 7));
                    for (int num28 = num23; num28 <= num24; num28++) {
                        WorldGen.PlacePot(num28, num10 + num11, 28, genRand.Next(25, 28));
                    }

                    // Second loot room content
                    num22 = x2 - direction;
                    num23 = num22;
                    num24 = x2;
                    if (num22 > x2) {
                        num23 = x2;
                        num24 = num22;
                    }

                    int num25_2 = genRand.Next(3);
                    if (num25_2 == 0)
                        num25_2 = genRand.Next(3);

                    if (Main.tenthAnniversaryWorld && num25_2 == 0)
                        num25_2 = 1;

                    switch (num25_2) {
                        case 0:
                            num25_2 = 848;
                            break;
                        case 1:
                            num25_2 = 857;
                            break;
                        case 2:
                            num25_2 = 934;
                            break;
                    }

                    while (num25_2 == num25) {
                        num25_2 = genRand.Next(3);
                        switch (num25_2) {
                            case 0:
                                num25_2 = 848;
                                break;
                            case 1:
                                num25_2 = 857;
                                break;
                            case 2:
                                num25_2 = 934;
                                break;
                        }
                    }

                    int num10_2 = num10 + yOffsetExtra;
                    int num10_21 = num10_2;
                    int startX = (num23 + num24) / 2 + lootRoomSizeY2 * -direction * 2;
                    int halfSizeX = num19_2 * -direction / 2;
                    int centerX = startX + halfSizeX;
                    int endX = centerX + halfSizeX;
                    while (Main.tile[centerX, num10_2].HasTile) {
                        num10_2++;
                    }
                    int startY = num10_2;
                    int bottomY = startY;
                    while (!Main.tile[centerX, bottomY].HasTile) {
                        bottomY++;
                    }
                    bottomY -= 1;

                    int minX = Math.Min(startX, endX);
                    int maxX = Math.Max(startX, endX);
                    for (int checkX = minX - 3; checkX < maxX + 3; checkX++) {
                        for (int checkY = startY - 3; checkY < bottomY + 3; checkY++) {
                            Main.tile[checkX, checkY].LiquidAmount = 0;
                        }
                    }

                    WorldGen.AddBuriedChest(centerX + 1, bottomY, num25_2, notNearOtherChests: false, 1, trySlope: false, 0);

                    if (ModContent.GetInstance<ConsolariaConfig>().pyramidMessageLoot) {
                        int index = 0;
                        int getTypeToPlaceByType() {
                            switch (index) {
                                case 0:
                                    return ItemID.AlphabetStatueQ;
                                case 1:
                                    return ItemID.AlphabetStatueH;
                                case 2:
                                    return ItemID.AlphabetStatueE;
                                case 3:
                                    return ItemID.AlphabetStatueU;
                                case 4:
                                    return ItemID.AlphabetStatueL;
                                case 5:
                                    return ItemID.AlphabetStatueV;
                                case 6:
                                    return ItemID.AlphabetStatueW;
                                case 7:
                                    return ItemID.AlphabetStatueL;
                                case 8:
                                    return ItemID.AlphabetStatueH;
                                case 9:
                                    return ItemID.AlphabetStatueU;
                                case 10:
                                    return ItemID.AlphabetStatueL;
                                case 11:
                                    return ItemID.AlphabetStatueV;
                                case 12:
                                    return ItemID.AlphabetStatueI;
                                default:
                                    return -1;
                            }
                        }
                        int start = 9;
                        int chest = Chest.FindChestByGuessing(centerX - 1, bottomY - 1);
                        for (int invent = 0; invent < Main.chest[chest].item.Length; invent++) {
                            if (Main.chest[chest].item[invent].IsAir) {
                                start = Math.Max(10, invent + 1);
                                break;
                            }
                        }
                        for (int invent = start; invent < Main.chest[chest].item.Length; invent++) {
                            Item item = Main.chest[chest].item[invent];
                            int type = getTypeToPlaceByType();
                            if (type == -1) {
                                break;
                            }
                            if (item.IsAir) {
                                if ((!Main.chest[chest].item[invent - 1].IsAir || genRand.NextChance(0.25)) && genRand.NextChance(0.85)) {
                                    continue;
                                }
                                if (index == 0) {
                                    start = invent;
                                }
                                item.SetDefaults(type);
                                item.stack = 1;
                                index++;
                                if (genRand.NextChance(0.85)) {
                                    continue;
                                }
                            }
                        }

                        bool coin1 = false;
                        bool coin2 = false;
                        while (!coin1 && !coin2) {
                            index = 0;
                            for (int invent = start; invent < Main.chest[chest].item.Length; invent++) {
                                Item item = Main.chest[chest].item[invent];
                                if (index > 1) {
                                    break;
                                }
                                if (item.IsAir && genRand.NextChance(0.4)) {
                                    switch (index) {
                                        case 0:
                                            item.SetDefaults(ItemID.GoldCoin);
                                            item.stack = 4;
                                            index++;
                                            coin1 = true;
                                            break;
                                        case 1:
                                            bool coin3 = false;
                                            for (int checkInvent = invent - 10; checkInvent < invent + 1; checkInvent++) {
                                                if (Main.chest[chest].item[checkInvent].type == ItemID.GoldCoin) {
                                                    coin3 = true;
                                                    break;
                                                }
                                            }
                                            if (coin3) {
                                                continue;
                                            }
                                            item.SetDefaults(ItemID.GoldCoin);
                                            item.stack = 1;
                                            index++;
                                            coin2 = true;
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    WorldGen.PlaceTile(centerX + genRand.Next(-1, 3), bottomY - genRand.Next(5, 8) + 1, 4, mute: true, forced: false, -1, 10);

                    num26 = genRand.Next(1, 10);
                    for (int num27 = 0; num27 < num26; num27++) {
                        int i2 = genRand.Next(minX - 3, maxX + 3);
                        int j2 = bottomY;
                        WorldGen.PlaceSmallPile(i2, j2, genRand.Next(16, 19), 1, 185);
                    }
                    for (int num28 = minX - 1; num28 <= maxX + 1; num28++) {
                        if (!Main.tile[num28, bottomY].HasTile) {
                            WorldGen.PlacePot(num28, bottomY, 28, genRand.Next(25, 28));
                        }
                    }

                    int posX = centerX + 4;
                    int posY = num10_21 - lootRoomSizeX2 + num11;
                    posY += 1;
                    while (!Main.tile[posX + 1, posY].HasTile) {
                        posX++;
                    }
                    posX -= 1;
                    WorldGen.PlaceTile(posX, posY, 91, mute: true, forced: false, -1, genRand.Next(4, 7));

                    posX = centerX - 3;
                    while (!Main.tile[posX - 1, posY].HasTile) {
                        posX--;
                    }
                    posX += 1;
                    WorldGen.PlaceTile(posX, posY, 91, mute: true, forced: false, -1, genRand.Next(4, 7));
                }

                if (flag4) {
                    flag4 = false;
                    direction *= -1;
                    num12 = genRand.Next(15, 20);
                }
                else if (flag6) {
                    num12 = genRand.Next(10, 15);
                }
                else {
                    direction *= -1;
                    num12 = genRand.Next(20, 40);
                }
            }

            if (num10 >= num5 - num11)
                flag2 = false;
        }

        // Cave tunnel
        int num29 = genRand.Next(100, 200);
        int num30 = genRand.Next(500, 800);
        flag2 = true;
        int num31 = num11;
        num12 = genRand.Next(10, 50);
        if (direction == 1)
            x -= num31;

        int num32 = genRand.Next(5, 10);
        while (flag2) {
            num29--;
            num30--;
            num12--;
            for (int num33 = x - num32 - genRand.Next(0, 2); num33 <= x + num31 + num32 + genRand.Next(0, 2); num33++) {
                int num34 = num10;
                if (num33 >= x && num33 <= x + num31) {
                    Tile tile = Main.tile[num33, num34];
                    tile.HasTile = false;
                }
                else {
                    Tile tile = Main.tile[num33, num34];
                    tile.TileType = num;
                    tile.HasTile = true;
                    tile.IsHalfBlock = false;
                    tile.Slope = 0;
                }

                if (num33 >= x - 1 && num33 <= x + 1 + num31) {
                    Main.tile[num33, num34].WallType = 34;
                }
            }

            num10++;
            x += direction;
            if (num29 <= 0) {
                flag2 = false;
                for (int num35 = x + 1; num35 <= x + num31 - 1; num35++) {
                    if (Main.tile[num35, num10].HasTile)
                        flag2 = true;
                }
            }

            if (num12 < 0) {
                num12 = genRand.Next(10, 50);
                direction *= -1;
            }

            if (num30 <= 0)
                flag2 = false;
        }

        return true;
    }

    void ILoadable.Unload() { }
}
