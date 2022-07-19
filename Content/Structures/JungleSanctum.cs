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


// @TODO
// [ ] so apparently the lanterns cannot hang from chains anymore??
// [ ] maybe adapt the code from the noraml jungle shrines? because the sanctum likes to generate at the fringes sometimes
// [ ] it looks like there is a version difference, as the lamps may be more towards the chest in some versions
// [ ] coin bags
// [ ] skulls and gore

namespace Consolaria.Content.Structures
{
  public class JungleSanctum : ModSystem
  {
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
    {
      int index = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
      if (index != -1)
      {
        tasks.Insert(index - 1, new JungleSanctumGeneration("Jungle Sanctum", 10f));
      }
    }
  }

  public class JungleSanctumGeneration : GenPass
  {
    public JungleSanctumGeneration(string name, float loadWeight) : base(name, loadWeight) { }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
      progress.Message = "Hiding";
      GenerateJungleSanctum();
    }

    private int[,] literallyJungleSanctum = new int[,] { { 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0 },
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
                                                         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                                                         };
    private void GenerateJungleSanctum()
    {
      int JungleSanctumPositionX = 0;
      int JungleSanctumPositionY = 0;
      int width = literallyJungleSanctum.GetLength(0);
      int height = literallyJungleSanctum.GetLength(1);

      int structureCount = 0;
      do
      {
        if (!TryLocation(width, height, ref JungleSanctumPositionX, ref JungleSanctumPositionY))
          return;

        structureCount++;
        for (int X = 0; X < height; X++)
        {
          for (int Y = 0; Y < width; Y++)
          {
            int posX = JungleSanctumPositionX + X;
            int posY = JungleSanctumPositionY + Y;
            switch (literallyJungleSanctum[Y, X])
            {
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
        WorldGen.Place1x2(JungleSanctumPositionX + 8, JungleSanctumPositionY + 6, TileID.HangingLanterns, 1);
        WorldGen.Place1x2(JungleSanctumPositionX + 10, JungleSanctumPositionY + 9, TileID.HangingLanterns, 2);
        WorldGen.Place1x2Top(JungleSanctumPositionX + 15, JungleSanctumPositionY + 9, TileID.HangingLanterns, 3);
        WorldGen.Place1x2Top(JungleSanctumPositionX + 17, JungleSanctumPositionY + 6, TileID.HangingLanterns, 4);

        // @TODO not done
        WorldGen.PlaceChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 10, 21, false, 1);
        int chestIndex = Chest.FindChest(JungleSanctumPositionX + 13, JungleSanctumPositionY + 12);
        if (chestIndex != -1)
          AddJungleSanctumLoot(Main.chest[chestIndex].item);

        WorldGen.structures.AddStructure(new Rectangle(JungleSanctumPositionX, JungleSanctumPositionY, width, height), 2);
      } while (structureCount != GetWorldSize());
    }

    private void AddJungleSanctumLoot(Item[] chestInventory)
    {
      int JungleSanctumLootIndex = 0;
      // literally just copied from the heart shrine
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ItemID.HeartStatue); chestInventory[JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ModContent.ItemType<BrokenHeart>()); chestInventory[JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ModContent.ItemType<ValentineRing>()); chestInventory[JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ItemID.HeartLantern); chestInventory[JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ItemID.HeartreachPotion); chestInventory[JungleSanctumLootIndex].stack = 1; JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ModContent.ItemType<HeartArrow>()); chestInventory[JungleSanctumLootIndex].stack = WorldGen.genRand.Next(14, 30); JungleSanctumLootIndex++;
      // chestInventory[JungleSanctumLootIndex].SetDefaults(ItemID.CopperCoin); chestInventory[JungleSanctumLootIndex].stack = 69;
    }

    private bool TryLocation(int width, int height, ref int i, ref int j)
    {
      int minValue = int.MaxValue;
      int maxValue = int.MinValue;
      for (int i1 = 0; i1 < Main.maxTilesX; ++i1)
      {
        for (int j1 = 0; j1 < Main.maxTilesY; ++j1)
        {
          Tile tileSafely = Framing.GetTileSafely(i1, j1);
          if (tileSafely.TileType == TileID.JungleGrass)
          {
            if (i1 < minValue)
              minValue = i1;
            if (i1 > maxValue)
              maxValue = i1;
          }
        }
      }
      for (int index1 = 0; index1 < 1000; ++index1)
      {
        int x = WorldGen.genRand.Next(minValue, maxValue - width);
        int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, (int)WorldGen.rockLayer + 100);
        Rectangle rectangle = new Rectangle(x, y, width, height);
        if (WorldGen.structures.CanPlace(rectangle, 0))
        {
          bool flag = true;
          for (int index2 = 0; index2 < 1000; ++index2)
          {
            Chest chest;
            if ((chest = Main.chest[index2]) != null && rectangle.Contains(chest.x, chest.y))
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            i = x;
            j = y;
            return true;
          }
        }
      }
      return false;
    }

    private int GetWorldSize()
    {
      int worldSize = 1;
      if (Main.maxTilesX <= 4200) worldSize = 1;
      else if (Main.maxTilesX <= 6400) worldSize = 2;
      else if (Main.maxTilesX <= 8400) worldSize = 3;

      return worldSize;
    }
  }
}