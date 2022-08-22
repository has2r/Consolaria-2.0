using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using Consolaria.Content.Tiles;
using Consolaria.Common;

namespace Consolaria.Content.Structures {
    public class JungleSanctum : ModSystem {
        public override void ModifyWorldGenTasks (List<GenPass> tasks, ref float totalWeight) {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (index != -1) {
                tasks.Insert(index - 1, new JungleSanctumGeneration("Jungle Sanctum", 10f));
            }
        }
    }

    public class JungleSanctumGeneration : GenPass {
        public JungleSanctumGeneration (string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass (GenerationProgress progress, GameConfiguration configuration) {
            progress.Message = "Hiding treasures";
            GenerateJungleSanctum();
        }

        private int [,] literallyJungleSanctum = new int [,] { { 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0 },
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
                                                         { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 },
                                                         { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 },
                                                         { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1 },
                                                         { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1 },
                                                         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                                                         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },};
        private void GenerateJungleSanctum () {
            int JungleSanctumPositionX = 0;
            int JungleSanctumPositionY = 0;
            int width = literallyJungleSanctum.GetLength(0);
            int height = literallyJungleSanctum.GetLength(1);

            int structureCount = 0;
            do {
                if (WorldGenHelper.TryStructureLocation(new int [] { TileID.JungleGrass, TileID.Mud }, width, height, ref JungleSanctumPositionX, ref JungleSanctumPositionY))
                    return;

                structureCount++;
                for (int X = 0; X < height; X++) {
                    for (int Y = 0; Y < width; Y++) {
                        int posX = JungleSanctumPositionX + X;
                        int posY = JungleSanctumPositionY + Y;
                        switch (literallyJungleSanctum [Y, X]) {
                        case 1:
                        WorldGen.KillTile(posX, posY);
                        break;
                        case 2:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.PlaceTile(posX, posY, TileID.IridescentBrick);
                        break;
                        case 3:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.PlaceTile(posX, posY, TileID.Chain);
                        break;
                        }
                    }
                }
                // parlwood lamps
                WorldGen.Place1xX(JungleSanctumPositionX + 7, JungleSanctumPositionY + 13, TileID.Lamps, 7);
                WorldGen.Place1xX(JungleSanctumPositionX + 19, JungleSanctumPositionY + 13, TileID.Lamps, 7);

                // carrige lanterns
                WorldGen.Place1x2(JungleSanctumPositionX + 8, JungleSanctumPositionY + 6, (ushort) ModContent.TileType<SanctumLantern>(), 1);
                WorldGen.Place1x2(JungleSanctumPositionX + 10, JungleSanctumPositionY + 9, (ushort) ModContent.TileType<SanctumLantern>(), 1);
                WorldGen.Place1x2(JungleSanctumPositionX + 15, JungleSanctumPositionY + 9, (ushort) ModContent.TileType<SanctumLantern>(), 1);
                WorldGen.Place1x2(JungleSanctumPositionX + 17, JungleSanctumPositionY + 6, (ushort) ModContent.TileType<SanctumLantern>(), 1);

                // coin bags
                int randomCoinBag = WorldGen.genRand.Next(0, 3);
                ushort coinBagType = 0;
                switch (randomCoinBag) {
                case 0:
                coinBagType = TileID.CopperCoinPile;
                break;
                case 1:
                coinBagType = TileID.SilverCoinPile;
                break;
                case 2:
                coinBagType = TileID.GoldCoinPile;
                break;
                }

                WorldGen.Place2x2(JungleSanctumPositionX + 12, JungleSanctumPositionY + 10, coinBagType, 1);
                WorldGen.Place2x2(JungleSanctumPositionX + 16, JungleSanctumPositionY + 10, coinBagType, 1);

                // @TODO not done
                WorldGen.PlaceChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 10, 21, false, 1);
                int chestIndex = Chest.FindChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 12);
                if (chestIndex != -1)
                    AddJungleSanctumLoot(Main.chest [chestIndex].item);

                WorldGen.structures.AddStructure(new Rectangle(JungleSanctumPositionX, JungleSanctumPositionY, width, height), 2);
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