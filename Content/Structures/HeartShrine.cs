using Consolaria.Common;
using Consolaria.Content.Items.Accessories;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Weapons.Ammo;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Consolaria.Content.Structures {
    public class HeartShrine : ModSystem {
        public override void ModifyWorldGenTasks (List<GenPass> tasks, ref double totalWeight) {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (index != -1) {
                tasks.Insert(index + 1, new HeartShrineGeneration("Heart Shrine", 10f));
            }
        }
    }

    public class HeartShrineGeneration : GenPass {
        public HeartShrineGeneration (string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass (GenerationProgress progress, GameConfiguration configuration) {
            if (SeasonalEvents.configEnabled && !SeasonalEvents.IsValentineDay())
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
        it:
            int heartShrinePositionX = 0;
            int heartShrinePositionY = 0;
            int width = literallyHeartShrine.GetLength(0);
            int height = literallyHeartShrine.GetLength(1);

            int structureCount = 0;
            do {
                while (!WorldGenHelper.TryStructureLocation(new int [] { TileID.SnowBlock, TileID.IceBlock }, width, height, ref heartShrinePositionX, ref heartShrinePositionY))
                    goto it;

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

                GenVars.structures.AddStructure(new Rectangle(heartShrinePositionX, heartShrinePositionY, width, height), 2);
            } while (structureCount != WorldGenHelper.GetWorldSize());
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
    }
}