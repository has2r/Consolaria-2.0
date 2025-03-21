using Consolaria.Common;
using Consolaria.Content.Tiles;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Consolaria.Content.Structures {
    public class JungleSanctum : ModSystem {
        public override void ModifyWorldGenTasks (List<GenPass> tasks, ref double totalWeight) {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (index != -1) {
                tasks.Insert(index + 1, new JungleSanctumGeneration("Jungle Sanctum", 10f));
            }
        }
    }

    public class JungleSanctumGeneration : GenPass {
        public JungleSanctumGeneration (string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass (GenerationProgress progress, GameConfiguration configuration) {
            progress.Message = "Hiding treasures";
            GenerateJungleSanctum();
        }

        private int [,] literallyJungleSanctum = new int [,] {
                                                         { 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 2, 2, 2, 1, 1, 3, 1, 3, 1, 1, 1, 1, 1, 3, 1, 3, 1, 1, 2, 2, 2, 0, 0, 0 },
                                                         { 0, 0, 2, 2, 2, 1, 1, 1, 3, 1, 3, 1, 1, 1, 1, 1, 3, 1, 3, 1, 1, 1, 2, 2, 2, 0, 0 },
                                                         { 0, 2, 2, 2, 1, 1, 1, 1, 3, 1, 3, 1, 1, 1, 1, 1, 3, 1, 3, 1, 1, 1, 1, 2, 2, 2, 0 },
                                                         { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2 },
                                                         { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2 },
                                                         { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2 },
                                                         { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2 },
                                                         { 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2 },
                                                         { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
                                                         { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
                                                         { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
                                                         { 4, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 4 },
                                                         { 4, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 4 },
                                                         { 4, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 4 },
                                                         { 4, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 4 },
                                                         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                                                         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, };

        private int [,] sanctumWalls = new int [,] {
                                                         { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                                                         { 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0 },
                                                         { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0 },
                                                         { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0 },
                                                         { 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0 },
                                                         { 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0 },
                                                         { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                         { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                         { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, };

        private void GenerateJungleSanctum () {
        it:
            int JungleSanctumPositionX = 0;
            int JungleSanctumPositionY = 0;
            int width = literallyJungleSanctum.GetLength(0);
            int height = literallyJungleSanctum.GetLength(1);

            int structureCount = 0;
            do {
                while (!WorldGenHelper.TryStructureLocation(new int [] { TileID.JungleGrass }, width, height, ref JungleSanctumPositionX, ref JungleSanctumPositionY))
                    goto it;

                structureCount++;
                for (int X = 0; X < height; X++) {
                    for (int Y = 0; Y < width; Y++) {
                        int posX = JungleSanctumPositionX + X;
                        int posY = JungleSanctumPositionY + Y;
                        switch (literallyJungleSanctum [Y, X]) {
                        case 1:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.KillWall(posX, posY);
                        WorldGen.PlaceWall(posX, posY, 64);
                        break;
                        case 2:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.PlaceTile(posX, posY, TileID.IridescentBrick);
                        break;
                        case 3:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.PlaceTile(posX, posY, TileID.Chain);
                        WorldGen.KillWall(posX, posY);
                        WorldGen.PlaceWall(posX, posY, 64);
                        break;
                        case 4:
                        WorldGen.KillTile(posX, posY);
                        break;
                        }
                        switch (sanctumWalls [Y, X]) {
                        case 1:
                        WorldGen.KillWall(posX, posY);
                        WorldGen.PlaceWall(posX, posY, WallID.IridescentBrick);
                        break;
                        }
                    }
                }

                //lamps
                WorldGen.Place1xX(JungleSanctumPositionX + 7, JungleSanctumPositionY + 13, TileID.Lamps, 18);
                WorldGen.TryToggleLight(JungleSanctumPositionX + 7, JungleSanctumPositionY + 13, false, true);
                WorldGen.Place1xX(JungleSanctumPositionX + 19, JungleSanctumPositionY + 13, TileID.Lamps, 18);
                WorldGen.TryToggleLight(JungleSanctumPositionX + 19, JungleSanctumPositionY + 13, false, true);

                //carrige lanterns
                WorldGen.PlaceTile(JungleSanctumPositionX + 8, JungleSanctumPositionY + 7, (ushort) ModContent.TileType<SanctumLantern>(), true);
                WorldGen.PlaceTile(JungleSanctumPositionX + 10, JungleSanctumPositionY + 10, (ushort) ModContent.TileType<SanctumLantern>(), true);
                WorldGen.PlaceTile(JungleSanctumPositionX + 16, JungleSanctumPositionY + 10, (ushort) ModContent.TileType<SanctumLantern>(), true);
                WorldGen.PlaceTile(JungleSanctumPositionX + 18, JungleSanctumPositionY + 7, (ushort) ModContent.TileType<SanctumLantern>(), true);

                //coin bags
                int randomCoinBag = WorldGen.genRand.Next(0, 3);
                ushort coinBagType = 0;
                switch (randomCoinBag) {
                case 0:
                coinBagType = 16;
                break;
                case 1:
                coinBagType = 17;
                break;
                case 2:
                coinBagType = 18;
                break;
                }

                WorldGen.PlaceSmallPile(JungleSanctumPositionX + 10, JungleSanctumPositionY + 13, coinBagType, 1, 185);
                WorldGen.PlaceSmallPile(JungleSanctumPositionX + 16, JungleSanctumPositionY + 13, coinBagType, 1, 185);

                WorldGen.PlaceChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 13, 21, false, 10);
                int chestIndex = Chest.FindChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 12);
                if (chestIndex != -1)
                    AddJungleSanctumLoot(Main.chest [chestIndex].item);

                for (int X = 0; X < height; X++) {
                    for (int Y = 0; Y < width; Y++) {
                        int i = WorldGen.genRand.Next(0, 2);
                        WorldGen.PlaceSmallPile(JungleSanctumPositionX + X, JungleSanctumPositionY + Y, i == 0 ? WorldGen.genRand.Next(12, 28) : WorldGen.genRand.Next(7, 15), i, 185);
                    }
                }

                GenVars.structures.AddStructure(new Rectangle(JungleSanctumPositionX, JungleSanctumPositionY, width, height), 2);
            } while (structureCount != WorldGenHelper.GetWorldSize());
        }

        private void AddJungleSanctumLoot (Item [] chestInventory) {
            int JungleSanctumLootIndex = 0;
            chestInventory [JungleSanctumLootIndex].SetDefaults(ItemID.HoneyDispenser); chestInventory [JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
            chestInventory [JungleSanctumLootIndex].SetDefaults(ItemID.FiberglassFishingPole); chestInventory [JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
            chestInventory [JungleSanctumLootIndex].SetDefaults(ItemID.AnkletoftheWind); chestInventory [JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
            chestInventory [JungleSanctumLootIndex].SetDefaults(ItemID.Bottle); chestInventory [JungleSanctumLootIndex].stack = WorldGen.genRand.Next(14, 30); JungleSanctumLootIndex++;
            chestInventory [JungleSanctumLootIndex].SetDefaults(ItemID.SilverCoin); chestInventory [JungleSanctumLootIndex].stack = WorldGen.genRand.Next(5, 65);
        }
    }
}