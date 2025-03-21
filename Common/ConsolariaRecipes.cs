using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Consolaria.Content.Items.Placeable;
using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Accessories;
using Consolaria.Content.Items.Armor.Melee;
using Consolaria.Content.Items.Armor.Ranged;
using Consolaria.Content.Items.Armor.Magic;
using Consolaria.Content.Items.Armor.Summon;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;

namespace Consolaria.Common;

sealed class ConsolariaRecipes : ModSystem {
    public override void AddRecipes() {
        // soul of blight
        Recipe item = Recipe.Create(ModContent.ItemType<SoulOfBlightInABottle>(), 1);
        item.AddIngredient(ItemID.Bottle, 1);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 1);
        item.AddTile(TileID.WorkBenches);
        item.SortAfterFirstRecipesOf(ItemID.SoulBottleFright);
        item.Register();

        item = Recipe.Create(ModContent.ItemType<SparklyWings>(), 1);
        item.AddIngredient(ItemID.SoulofFlight, 20);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddIngredient(ItemID.SoulofLight, 10);
        item.AddIngredient(ItemID.SoulofNight, 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfterFirstRecipesOf(ItemID.BeeWings);
        item.Register();

        item = Recipe.Create(ModContent.ItemType<Tonbogiri>(), 1);
        item.AddIngredient(ItemID.Gungnir, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfterFirstRecipesOf(ItemID.Gungnir);
        item.Register();

        item = Recipe.Create(ModContent.ItemType<VolcanicRepeater>(), 1);
        item.AddIngredient(ItemID.HallowedRepeater, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfterFirstRecipesOf(ItemID.HallowedRepeater);
        item.Register();

        // hallowed
        // melee
        item = Recipe.Create(ModContent.ItemType<DragonMask>(), 1);
        item.AddIngredient(ItemID.HallowedMask, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofMight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfterFirstRecipesOf(ItemID.HallowedGreaves);
        item.Register();
        Recipe temp = item;
        item = Recipe.Create(ModContent.ItemType<DragonBreastplate>(), 1);
        item.AddIngredient(ItemID.HallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofMight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<DragonGreaves>(), 1);
        item.AddIngredient(ItemID.HallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofMight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();

        // ranged
        temp = item;
        item = Recipe.Create(ModContent.ItemType<TitanHelmet>(), 1);
        item.AddIngredient(ItemID.HallowedHelmet, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofSight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<TitanMail>(), 1);
        item.AddIngredient(ItemID.HallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofSight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<TitanLeggings>(), 1);
        item.AddIngredient(ItemID.HallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofSight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();

        // magic
        temp = item;
        item = Recipe.Create(ModContent.ItemType<PhantasmalHeadgear>(), 1);
        item.AddIngredient(ItemID.HallowedHeadgear, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofFright, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<PhantasmalRobe>(), 1);
        item.AddIngredient(ItemID.HallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofFright, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<PhantasmalSubligar>(), 1);
        item.AddIngredient(ItemID.HallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofFright, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();

        // summon
        temp = item;
        item = Recipe.Create(ModContent.ItemType<WarlockHood>(), 1);
        item.AddIngredient(ItemID.HallowedHood, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofNight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<WarlockRobe>(), 1);
        item.AddIngredient(ItemID.HallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofNight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<WarlockLeggings>(), 1);
        item.AddIngredient(ItemID.HallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofNight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.MythrilAnvil);
        item.SortAfter(temp);
        item.Register();

        // ancient hallowed
        // melee
        item = Recipe.Create(ModContent.ItemType<AncientDragonMask>(), 1);
        item.AddIngredient(ItemID.AncientHallowedMask, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofMight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfterFirstRecipesOf(ItemID.AncientHallowedGreaves);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientDragonBreastplate>(), 1);
        item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofMight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientDragonGreaves>(), 1);
        item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofMight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();

        // ranged
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientTitanHelmet>(), 1);
        item.AddIngredient(ItemID.AncientHallowedHelmet, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofSight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientTitanMail>(), 1);
        item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofSight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientTitanLeggings>(), 1);
        item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofSight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();

        // magic
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientPhantasmalHeadgear>(), 1);
        item.AddIngredient(ItemID.AncientHallowedHeadgear, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofFright, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientPhantasmalRobe>(), 1);
        item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofFright, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientPhantasmalSubligar>(), 1);
        item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofFright, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();

        // summon
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientWarlockHood>(), 1);
        item.AddIngredient(ItemID.AncientHallowedHood, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofNight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientWarlockRobe>(), 1);
        item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 12);
        item.AddIngredient(ItemID.SoulofNight, 15);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<AncientWarlockLeggings>(), 1);
        item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
        item.AddRecipeGroup(RecipeGroups.Titanium, 10);
        item.AddIngredient(ItemID.SoulofNight, 10);
        item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();
    }
}
