using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Consolaria.Content.Structures
{ 
    public class AntiarisWorld : ModSystem
    {
        public int StartPositionX = 0;
        public int StartPositionY = 0;
        public int CoveBlock1 = 0;
        public int CoveBlock2 = 1;
        private bool GenerateSnowHouse;
        public bool GeneratePirateCove;

        //0=air, 1=snow, 2=boreal wood, 3=wooden beam, 4=glass, 5=boreal wood platform, 6=cobweb
        static readonly byte[,] SnowHouseTiles =
        {
                                                          { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
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
                                                          { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        //0=old block, 1=dirt, 2=stone, 3=wood, 4=wood platforms, 5=rope, 6=cobweb, 7=air, 8=water, 9=gold coins
        private static readonly byte[,] PirateCoveTiles =
        {
            {0,0,0,0,0,0,0,0,0,2,2,2,2,2,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,2,2,2,2,8,8,8,8,8,8,1,1,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,2,2,2,2,8,8,8,8,8,8,8,8,1,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,2,2,2,2,8,8,8,8,8,8,8,8,8,1,1,1,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,2,1,1,8,8,8,8,8,8,8,8,8,8,1,1,1,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,1,1,1,8,8,8,8,8,8,8,8,8,8,8,8,1,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,8,8,8,8,8,8,8,8,8,8,8,8,8,1,1,1,1,1,1,1,1,0},
            {0,0,0,0,1,0,1,8,8,8,8,8,8,8,8,8,8,8,8,8,8,1,1,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,1,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,8,2,2,2,1,1},
            {0,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,8,8,8,8,1,1},
            {0,1,1,1,1,7,7,7,7,7,7,7,7,7,5,5,9,7,7,7,7,7,7,1,8,8,8,8,1,1},
            {0,2,1,3,7,7,7,7,7,7,7,7,7,7,5,5,7,7,7,7,7,7,7,7,7,7,7,2,2,2},
            {2,2,1,3,7,7,7,7,7,7,7,7,7,7,5,5,7,7,7,7,7,7,7,7,7,7,7,2,2,2},
            {2,2,7,7,7,7,7,7,7,7,7,7,7,7,5,5,7,7,7,7,7,7,7,7,7,7,1,1,2,2},
            {2,2,7,7,7,7,7,7,7,7,7,7,7,7,5,5,7,7,7,7,7,7,7,7,7,7,1,1,2,0},
            {2,2,7,7,7,7,7,7,7,7,7,7,7,7,5,5,5,7,7,7,7,7,7,7,7,7,1,1,2,0},
            {2,2,2,3,7,7,7,7,7,7,7,7,7,7,5,5,5,7,7,7,7,7,7,7,7,7,7,1,1,0},
            {2,2,8,3,7,7,7,7,7,7,7,7,7,5,5,5,5,7,7,7,7,7,7,7,7,7,7,1,1,0},
            {2,2,2,7,7,7,7,7,7,7,7,7,7,5,5,5,5,7,7,7,7,7,7,7,7,7,2,1,1,0},
            {2,2,2,6,6,7,7,7,7,7,7,7,7,5,5,5,5,7,7,7,7,7,7,7,7,7,2,1,1,0},
            {2,1,6,6,6,7,7,7,7,7,7,7,7,5,5,5,7,7,7,7,7,7,7,7,7,2,2,1,1,0},
            {1,1,3,3,3,7,7,7,7,3,3,6,7,3,3,3,3,7,7,7,7,7,7,7,7,2,1,1,0,0},
            {1,1,7,9,9,7,7,7,7,9,6,6,6,1,1,1,1,7,7,7,7,7,1,1,1,1,1,1,0,0},
            {1,1,1,1,1,1,7,7,6,7,7,6,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
            {0,1,1,1,1,1,1,6,6,1,1,2,2,2,2,2,2,1,1,1,1,1,1,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,1,1,1,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        //0=none, 1=wood, 2=grass, 3=old
        private static readonly byte[,] PirateCoveWalls =
        {
            {3,3,3,3,3,3,3,3,3,3,3,1,3,1,1,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,3,3,3,3,0,1,0,1,1,0,0,1,1,0,3,3,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,3,3,3,0,0,0,0,0,1,0,0,1,1,0,0,3,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,3,3},
            {3,3,3,3,3,0,0,0,0,0,0,0,0,1,1,2,2,0,1,1,1,1,0,0,0,0,0,0,0,3},
            {3,3,3,3,2,2,1,2,0,0,2,2,1,1,1,1,2,2,2,1,1,0,0,0,0,0,0,0,0,3},
            {3,3,0,1,1,1,1,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {3,3,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {3,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {3,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
            {3,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
            {3,0,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
            {3,0,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
            {3,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,3,3},
            {3,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,2,2,2,3,3,3},
            {3,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,1,1,0,0,2,2,2,2,2,2,2,3,3,3},
            {3,0,2,2,1,1,2,2,1,1,1,2,2,0,0,0,1,1,0,0,2,2,2,2,2,2,3,3,3,3},
            {3,0,2,2,2,0,0,2,2,0,1,2,2,2,0,0,1,1,2,2,2,2,2,2,2,0,3,3,3,3},
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3},
            {3,3,3,3,3,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            {3,3,3,3,3,3,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
        };

        //0=none, 1=bottom-left, 2=bottom-right, 3=top-left, 4=top-right, 5=half
        private static readonly byte[,] PirateCoveSlopes =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,3,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };    

        internal static readonly int[] SnowHouseGenTiles =
        {
            147,
            161,
            163,
            200
        };
       
        private int CheckTile = TileID.Dirt;
        private int HellLayer = Main.maxTilesY - 200;
        private int Size = WorldGen.genRand.Next(6, 10);
        private int SnowHousePositionX = 0;
        private int SnowHousePositionY = 0;

        public static int PirateCovePositionX = 0;
        public static int PirateCovePositionY = 0;


      /*  public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            var index = tasks.FindIndex(x => x.Name == "Micro Biomes");
            if (index != -1)
            tasks.Add(new PassLegacy("[Consolaria] Heart Shrine", AddSnowHouse));
        }*/

         void AddSnowHouse(GenerationProgress progress = null) {
            if (GenerateSnowHouse) return;
            
            bool Success = do_MakeSnowHouse(progress);
            if (Success) GenerateSnowHouse = true;       
        }

       
      /*  public static bool do_MakePirateCove(GenerationProgress progress)
        {
            string PirateCoveGen = Language.GetTextValue("Mods.Antiaris.PirateCoveGen");
            if (progress != null)
            {
                progress.Message = PirateCoveGen;
                progress.Set(0.33f);
            }
            int minX = (int)(Main.maxTilesX * 0.285f);
            int minY = (int)(Main.rockLayer);

            int maxTilesX = 30;
            int maxTilesY = 28;

            int maxX = (int)(Main.maxTilesX * 0.8f) - maxTilesX;
            int maxY = Main.maxTilesY - 200 - maxTilesY;

            bool generated = true;
            for (int i = -1; i < 0; i++)
                if (generated)
                {
                    PirateCovePositionX = WorldGen.genRand.Next(minX, maxX);
                    PirateCovePositionY = WorldGen.genRand.Next(minY, maxY);
                    generated = false;
                }
            for (int i = PirateCovePositionX; i < PirateCovePositionX - maxTilesX; i++)
            {
                for (int j = PirateCovePositionY; j < PirateCovePositionY - maxTilesY; j++)
                {
                    Tile tileSafely = Framing.GetTileSafely(i, j);
                    if (Main.tile[i, j].active())
                        goto Success;
                    else if (tileSafely.wall >= 231 || tileSafely.type >= 470 || tileSafely.type == TileID.LihzahrdBrick || Main.wallHouse[(int)tileSafely.wall] || WallID.Sets.Corrupt[(int)tileSafely.wall] || WallID.Sets.Crimson[(int)tileSafely.wall] || (tileSafely.active() && (TileID.Sets.BasicChest[(int)tileSafely.type] || TileID.Sets.BasicChest[(int)Framing.GetTileSafely(i, j - 1).type] || (Main.tileSolid[(int)tileSafely.type] && (TileID.Sets.Corrupt[(int)tileSafely.type] || TileID.Sets.Crimson[(int)tileSafely.type] || TileID.Sets.IcesSlush[(int)tileSafely.type] || TileID.Sets.JungleSpecial[(int)tileSafely.type] || TileID.Sets.GrassSpecial[(int)tileSafely.type])))))
                    {
                        PirateCovePositionX = WorldGen.genRand.Next(minX, maxX);
                        PirateCovePositionY = WorldGen.genRand.Next(minY, maxY);
                        generated = true;
                    }
                }
            }

            Success:
            goto GenerateBuild;
            AntiarisHelper.Log("Continue...");


			GenerateBuild:
			AntiarisHelper.Log("Generating pirate cove...");
			Mod mod = ModLoader.GetMod("Antiaris");
            for (var t = 0; t < 6; t++)
            {
                if (Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.SnowBlock || Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.IceBlock)
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 147;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 161;
                }
                else if (Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.HardenedSand)
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 397;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 396;
                }
                else if (Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.Marble)
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 367;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 367;
                }
                else if (Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.Granite)
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 368;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 368;
                }
                else if (Main.tile[PirateCovePositionX, PirateCovePositionY + t].type == TileID.Mud)
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 59;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 1;
                }
                else
                {
					mod.GetModWorld<AntiarisWorld>().CoveBlock1 = 0;
					mod.GetModWorld<AntiarisWorld>().CoveBlock2 = 1;
                }
            }
            for (var X = 0; X < PirateCoveTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < PirateCoveTiles.GetLength(0); Y++)
                {
                    var tile = Framing.GetTileSafely(PirateCovePositionX + X, PirateCovePositionY - Y);
                    switch (PirateCoveTiles[Y, X])
                    {
                        case 0:
                            break;
                        case 1:
                            tile.type = (ushort)mod.GetModWorld<AntiarisWorld>().CoveBlock1;
                            tile.active(true);
                            break;
                        case 2:
                            tile.type = (ushort)mod.GetModWorld<AntiarisWorld>().CoveBlock2;
                            tile.active(true);
                            break;  
                        case 3:
                            tile.type = 30;
                            tile.active(true);
                            break;
                        case 4:
                            WorldGen.KillTile(PirateCovePositionX + X, PirateCovePositionY - Y);
                            WorldGen.PlaceTile(PirateCovePositionX + X, PirateCovePositionY - Y, TileID.Platforms, false, false, -1, 0);
                            break;
                        case 5:
                            tile.type = 213;
                            tile.active(true);
                            break;
                        case 6:
                            tile.type = 51;
                            tile.active(true);
                            break;
                        case 7:
                            WorldGen.KillTile(PirateCovePositionX + X, PirateCovePositionY - Y, false, false, false);
							tile.liquid = 0;
                            break;
                        case 8:
                            WorldGen.KillTile(PirateCovePositionX + X, PirateCovePositionY - Y, false, false, false);
                            tile.liquid = 255;
                            break;
                        case 9:
                            tile.type = 332;
                            tile.active(true);
                            break;
                    }
                    switch (PirateCoveWalls[Y, X])
                    {
                        case 0:
                            tile.wall = 0;
                            break;
                        case 1:
                            tile.wall = 4;
                            break;
                        case 2:
                            tile.wall = 63;
                            break;
                        case 3:
                            break;
                    }
                    if (PirateCoveSlopes[Y, X] == 5)
                    {
                        tile.halfBrick(true);
                    }
                    else
                    {
                        tile.halfBrick(false);
                        tile.slope(PirateCoveSlopes[Y, X]);
                    }
                }
            }
            WorldGen.PlaceObject(PirateCovePositionX + 7, PirateCovePositionY - 10, 376, true);
            WorldGen.PlaceObject(PirateCovePositionX + 19, PirateCovePositionY - 10, 376, true);
            WorldGen.PlaceObject(PirateCovePositionX + 15, PirateCovePositionY - 1, 376, true);
            WorldGen.PlaceObject(PirateCovePositionX + 10, PirateCovePositionY - 20, 42, true, 2);
            WorldGen.PlaceObject(PirateCovePositionX + 10, PirateCovePositionY - 10, 93, true, 15);
            WorldGen.PlaceObject(PirateCovePositionX + 13, PirateCovePositionY - 10, 93, true, 15);
            WorldGen.PlaceObject(PirateCovePositionX + 8, PirateCovePositionY - 10, ModLoader.GetMod("Antiaris").TileType("GildedStrongbox"));
            WorldGen.PlaceObject(PirateCovePositionX + 12, PirateCovePositionY - 1, ModLoader.GetMod("Antiaris").TileType("GildedStrongbox"));
            WorldGen.PlaceObject(PirateCovePositionX + 7, PirateCovePositionY - 12, ModLoader.GetMod("Antiaris").TileType("GildedCrate"));
            WorldGen.PlaceObject(PirateCovePositionX + 21, PirateCovePositionY - 10, ModLoader.GetMod("Antiaris").TileType("GildeCrate"));
            WorldGen.PlaceObject(PirateCovePositionX + 17, PirateCovePositionY - 1, ModLoader.GetMod("Antiaris").TileType("GildedCrate"));
            WorldGen.PlaceObject(PirateCovePositionX + 21, PirateCovePositionY - 10, ModLoader.GetMod("Antiaris").TileType("GildedCrate"));
            WorldGen.PlaceObject(PirateCovePositionX + 18, PirateCovePositionY - 12, ModLoader.GetMod("Antiaris").TileType("GildedStrongbox"));
            WorldGen.PlaceObject(PirateCovePositionX + 2, PirateCovePositionY - 22, ModLoader.GetMod("Antiaris").TileType("GildedCup"));
            WorldGen.PlaceObject(PirateCovePositionX + 15, PirateCovePositionY - 3, ModLoader.GetMod("Antiaris").TileType("GildedCup"));
            WorldGen.PlaceObject(PirateCovePositionX + 26, PirateCovePositionY - 16, ModLoader.GetMod("Antiaris").TileType("GildedCup"));
            WorldGen.PlaceChest(PirateCovePositionX + 11, PirateCovePositionY - 10, (ushort)ModLoader.GetMod("Antiaris").TileType("GildedChest"), false, 2);
            var PirateChestIndex = Chest.FindChest(PirateCovePositionX + 11, PirateCovePositionY - 11);
            if (PirateChestIndex != -1)
            {
                do_PirateCoveLoot(Main.chest[PirateChestIndex].item);
            }
			mod.GetModWorld<AntiarisWorld>().GeneratePirateCove = true;
            return true;
        }*/

        bool do_MakeSnowHouse(GenerationProgress progress)
        {
			string SnowHouseGen = Language.GetTextValue("Adding some love...");
			if (progress != null)
			{
				progress.Message = SnowHouseGen;
				progress.Set(0.33f);
			}
			List<Point> list = new List<Point>();
			foreach (int k in SnowHouseGenTiles)
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
					Point point = list[WorldGen.genRand.Next(0, list.Count<Point>())];
					SnowHousePositionX = point.X; SnowHousePositionY = point.Y;
					goto GenerateBuild;
				}
			}
            return false;

            GenerateBuild:
            for (var X = 0; X < SnowHouseTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < SnowHouseTiles.GetLength(0); Y++)
                {
					var tile = Framing.GetTileSafely(SnowHousePositionX + X, SnowHousePositionY - Y);
                    switch (SnowHouseTiles[Y, X])
                    {
                        case 0:
                            break;
                        case 1:
                            WorldGen.KillTile(SnowHousePositionX + X, SnowHousePositionY + Y);
                            WorldGen.KillWall(SnowHousePositionX + X, SnowHousePositionY + Y);
                            WorldGen.paintTile(SnowHousePositionX + X, SnowHousePositionY + Y, PaintID.PinkPaint);
                            break;
                        case 2:
                            WorldGen.KillTile(SnowHousePositionX + X, SnowHousePositionY + Y);
                            WorldGen.KillWall(SnowHousePositionX + X, SnowHousePositionY + Y);
                            WorldGen.PlaceTile(SnowHousePositionX + X, SnowHousePositionY + Y, TileID.SnowBlock);
                            break;
                    }       
                }
            }
            WorldGen.AddLifeCrystal(SnowHousePositionX + 10, SnowHousePositionY + 10);
            WorldGen.AddLifeCrystal(SnowHousePositionX + 24, SnowHousePositionY + 10);

            WorldGen.PlaceChest(SnowHousePositionX + 16, SnowHousePositionY + 15, 21, false, 11);
            int chestIndex = Chest.FindChest(SnowHousePositionX + 16, SnowHousePositionY + 15);
            if (chestIndex != -1)
                AddHeartShrineLoot(Main.chest[chestIndex].item);
            return true;
        }

        private void AddHeartShrineLoot(Item[] chestInventory) {
            int CastlecurrentIndex = 0;
            chestInventory[CastlecurrentIndex].SetDefaults(ItemID.CopperCoin); chestInventory[CastlecurrentIndex].stack = 69; CastlecurrentIndex++;
        }
    }
}

