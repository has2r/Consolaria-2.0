using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Consolaria.Content.Items.Accessories;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Weapons.Ammo;
using Terraria.IO;
using Consolaria.Common;

namespace Consolaria.Content.Structures {
    public class HeartShrine : ModSystem {
        public override void ModifyWorldGenTasks (List<GenPass> tasks, ref float totalWeight) {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (index != -1) {
                tasks.Insert(index - 1, new HeartShrineGeneration("Heart Shrine", 10f));
            }
        }
    }

    public class HeartShrineGeneration : GenPass {
        public HeartShrineGeneration (string name, float loadWeight) : base(name, loadWeight) {}

        protected override void ApplyPass (GenerationProgress progress, GameConfiguration configuration) {
            if (SeasonalEvents.enabled && !SeasonalEvents.isValentinesDay)
                return;
            progress.Message = "Adding some love";
            GenerateHeartShrine();
        }

        private int [,] literallyHeartShrine = new int [,] {{ 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
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
                                                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
        private void GenerateHeartShrine () {
            int heartShrinePositionX = 0;
            int heartShrinePositionY = 0;
            int width = literallyHeartShrine.GetLength(0);
            int height = literallyHeartShrine.GetLength(1);

            int structureCount = 0;
            do {
                if (!TryLocation(width, height, ref heartShrinePositionX, ref heartShrinePositionY))
                    return;

                structureCount++;
                for (int X = 0; X < height; X++) {
                    for (int Y = 0; Y < width; Y++) {
                        int posX = heartShrinePositionX + X;
                        int posY = heartShrinePositionY + Y;
                        switch (literallyHeartShrine [Y, X]) {
                        case 1:
                        WorldGen.KillTile(posX, posY);
                        break;
                        case 2:
                        WorldGen.KillTile(posX, posY);
                        WorldGen.PlaceTile(posX, posY, TileID.SnowBlock);
                        break;
                        }
                    }
                }

                WorldGen.AddLifeCrystal(heartShrinePositionX + 10, heartShrinePositionY + 10);
                WorldGen.AddLifeCrystal(heartShrinePositionX + 24, heartShrinePositionY + 10);

                WorldGen.PlaceChest(heartShrinePositionX + 16, heartShrinePositionY + 15, 21, false, 11);
                int chestIndex = Chest.FindChest(heartShrinePositionX + 16, heartShrinePositionY + 14);
                if (chestIndex != -1)
                    AddHeartShrineLoot(Main.chest [chestIndex].item);

                WorldGen.structures.AddStructure(new Rectangle(heartShrinePositionX, heartShrinePositionY, width, height), 2);
            } while (structureCount != GetWorldSize());
        }

        private void AddHeartShrineLoot (Item [] chestInventory) {
            int heartShrineLootIndex = 0;
            chestInventory [heartShrineLootIndex].SetDefaults(ItemID.HeartStatue); chestInventory [heartShrineLootIndex].stack = 1; heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ModContent.ItemType<BrokenHeart>()); chestInventory [heartShrineLootIndex].stack = 1; heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ModContent.ItemType<ValentineRing>()); chestInventory [heartShrineLootIndex].stack = 1; heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ItemID.HeartLantern); chestInventory [heartShrineLootIndex].stack = 1; heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ItemID.HeartreachPotion); chestInventory [heartShrineLootIndex].stack = 1; heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ModContent.ItemType<HeartArrow>()); chestInventory [heartShrineLootIndex].stack = WorldGen.genRand.Next(14, 30); heartShrineLootIndex++;
            chestInventory [heartShrineLootIndex].SetDefaults(ItemID.CopperCoin); chestInventory [heartShrineLootIndex].stack = 69;
        }

        private bool TryLocation (int width, int height, ref int i, ref int j) {
            int minValue = int.MaxValue;
            int maxValue = int.MinValue;
            for (int i1 = 0; i1 < Main.maxTilesX; ++i1) {
                for (int j1 = 0; j1 < Main.maxTilesY; ++j1) {
                    Tile tileSafely = Framing.GetTileSafely(i1, j1);
                    if (tileSafely.TileType == TileID.SnowBlock || tileSafely.TileType == TileID.IceBlock) {
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
                Rectangle rectangle = new Rectangle(x, y, width, height);
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

        private int GetWorldSize () {
            int worldSize = 1;
            if (Main.maxTilesX <= 4200) worldSize = 1;
            else if (Main.maxTilesX <= 6400) worldSize = 2;
            else if (Main.maxTilesX <= 8400) worldSize = 3;

            return worldSize;
        }
    }
}