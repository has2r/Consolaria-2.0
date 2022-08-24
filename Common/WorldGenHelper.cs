using Microsoft.Xna.Framework;
using Terraria;

namespace Consolaria.Common {
    public static class WorldGenHelper {
        public static bool TryStructureLocation (int [] tileType, int width, int height, ref int i, ref int j) {
            int minValue = int.MaxValue;
            int maxValue = int.MinValue;
            for (int i1 = 0; i1 < Main.maxTilesX; ++i1) {
                for (int j1 = 0; j1 < Main.maxTilesY; ++j1) {
                    Tile tileSafely = Framing.GetTileSafely(i1, j1);
                    if (tileSafely.TileType == (int) tileType.GetValue(0) || (tileType.Length != 1 && tileSafely.TileType == (int)tileType.GetValue(1))) {
                        if (i1 < minValue)
                            minValue = i1;
                        if (i1 > maxValue)
                            maxValue = i1;
                    }
                }
            }
            for (int index1 = 0; index1 < 1000; ++index1) {
                int x = WorldGen.genRand.Next(minValue, maxValue - width);
                int y = WorldGen.genRand.Next((int) WorldGen.rockLayerLow, (int) WorldGen.rockLayer + 100);
                Rectangle rectangle = new(x, y, width, height);
                if (WorldGen.structures.CanPlace(rectangle, 0)) {
                    bool flag = true;
                    for (int index2 = 0; index2 < 1000; ++index2) {
                        Chest chest;
                        if ((chest = Main.chest [index2]) != null && rectangle.Contains(chest.x, chest.y)) {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) {
                        i = x;
                        j = y;
                        return true;
                    }
                }
            }
            return false;
        }

        public static int GetWorldSize () {
            int worldSize = 1;
            if (Main.maxTilesX <= 4200) worldSize = 1;
            else if (Main.maxTilesX <= 6400) worldSize = 2;
            else if (Main.maxTilesX <= 8400) worldSize = 3;

            return worldSize;
        }
    }
}