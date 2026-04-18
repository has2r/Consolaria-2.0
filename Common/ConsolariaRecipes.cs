using Consolaria.Content.Crossmod.Thorium.Armor;
using Consolaria.Content.Items.Accessories;
using Consolaria.Content.Items.Armor.Magic;
using Consolaria.Content.Items.Armor.Melee;
using Consolaria.Content.Items.Armor.Ranged;
using Consolaria.Content.Items.Armor.Summon;
using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Placeable;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Throwing;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common;

sealed class ConsolariaRecipes : ModSystem {
    public override void AddRecipes() {
        // thread vanity
        Recipe item = Recipe.Create(ModContent.ItemType<MonokumaHead>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.BlackThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfterFirstRecipesOf(ItemID.SuperHeroTights);
        item.Register();
        Recipe temp = item;
        item = Recipe.Create(ModContent.ItemType<MonokumaBody>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.BlackThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<MonokumaLegs>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.BlackThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfter(temp);
        item.Register();

        temp = item;
        item = Recipe.Create(ModContent.ItemType<MonomiHead>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.PinkThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<MonomiBody>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.PinkThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfter(temp);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<MonomiLegs>(), 1);
        item.AddIngredient(ItemID.Silk, 20);
        item.AddIngredient(ModContent.ItemType<WhiteThread>(), 3);
        item.AddIngredient(ItemID.PinkThread, 3);
        item.AddTile(TileID.Loom);
        item.SortAfter(temp);
        item.Register();

        if (ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
            item = Recipe.Create(ModContent.ItemType<AncientHerosHat>(), 1);
            item.AddIngredient(ItemID.Silk, 20);
            item.AddIngredient(ModContent.ItemType<PurpleThread>(), 3);
            item.AddTile(TileID.Loom);
            item.SortAfterFirstRecipesOf(ItemID.HerosPants);
            item.Register();
            temp = item;
            item = Recipe.Create(ModContent.ItemType<AncientHerosShirt>(), 1);
            item.AddIngredient(ItemID.Silk, 20);
            item.AddIngredient(ModContent.ItemType<PurpleThread>(), 3);
            item.AddTile(TileID.Loom);
            item.SortAfter(temp);
            item.Register();
            temp = item;
            item = Recipe.Create(ModContent.ItemType<AncientHerosPants>(), 1);
            item.AddIngredient(ItemID.Silk, 20);
            item.AddIngredient(ModContent.ItemType<PurpleThread>(), 3);
            item.AddTile(TileID.Loom);
            item.SortAfter(temp);
            item.Register();
        }

        // ocram summon
        item = Recipe.Create(ModContent.ItemType<SuspiciousLookingSkull>(), 1);
        item.AddIngredient(ItemID.Bone, 15);
        item.AddIngredient(ItemID.Ectoplasm, 5);
        item.AddIngredient(ItemID.SoulofFright, 5);
        item.AddIngredient(ItemID.SoulofMight, 5);
        item.AddIngredient(ItemID.SoulofSight, 5);
        item.AddTile(TileID.DemonAltar);
        item.SortAfterFirstRecipesOf(ItemID.MechanicalSkull);
        item.Register();

        // threads
        item = Recipe.Create(ModContent.ItemType<WhiteThread>(), 1);
        item.AddIngredient(ItemID.ShiverthornSeeds, 3);
        item.AddTile(TileID.Bottles);
        item.SortAfterFirstRecipesOf(ItemID.GreenThread);
        item.Register();
        temp = item;
        if (ModContent.GetInstance<ConsolariaConfig>().originalAncientHeroSetRecipeEnabled) {
            item = Recipe.Create(ModContent.ItemType<PurpleThread>(), 1);
            item.AddIngredient(ItemID.DeathweedSeeds, 3);
            item.AddTile(TileID.Bottles);
            item.SortAfter(temp);
            item.Register();
        }

        // rainbow pieces
        item = Recipe.Create(ItemID.RainbowBrick, 10);
        item.AddIngredient(ItemID.StoneBlock, 10);
        item.AddIngredient(ModContent.ItemType<RainbowPiece>(), 1);
        item.AddTile(TileID.Furnaces);
        item.SortAfterFirstRecipesOf(ItemID.GrayBrick);
        item.Register();

        item = Recipe.Create(ModContent.ItemType<PotOfGold>(), 1);
        item.AddIngredient(ModContent.ItemType<RainbowPiece>(), 5);
        item.AddTile(TileID.Anvils);
        item.SortAfterFirstRecipesOf(ItemID.ManaCrystal);
        item.Register();

        // soul of blight
        item = Recipe.Create(ModContent.ItemType<SoulOfBlightInABottle>(), 1);
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
        temp = item;
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

        string thoriumModName = "ThoriumMod";
        if (ModLoader.HasMod(thoriumModName)) {
            Mod thoriumMod = ModLoader.GetMod(thoriumModName);
            // thrower
            if (thoriumMod.TryFind("HallowedGuise", out ModItem hallowedGuise)) {
                temp = item;
                item = Recipe.Create(ModContent.ItemType<ViperHelmet>(), 1);
                item.AddIngredient(hallowedGuise.Type, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofLight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<ViperChestplate>(), 1);
                item.AddIngredient(ItemID.HallowedPlateMail, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                item.AddIngredient(ItemID.SoulofLight, 15);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<ViperLegs>(), 1);
                item.AddIngredient(ItemID.HallowedGreaves, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofLight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
            }
            // healer
            if (thoriumMod.TryFind("HallowedCowl", out ModItem hallowedCowl)) {
                temp = item;
                item = Recipe.Create(ModContent.ItemType<SeraphimHelmet>(), 1);
                item.AddIngredient(hallowedCowl.Type, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofFlight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<SeraphimChestplate>(), 1);
                item.AddIngredient(ItemID.HallowedPlateMail, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                item.AddIngredient(ItemID.SoulofFlight, 15);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<SeraphimLegs>(), 1);
                item.AddIngredient(ItemID.HallowedGreaves, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofFlight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
            }
            // bard
            if (thoriumMod.TryFind("HallowedChapeau", out ModItem hallowedChapeau)) {
                if (thoriumMod.TryFind("SoulofPlight", out ModItem soulofPlight)) {
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<SirenHelmet>(), 1);
                    item.AddIngredient(hallowedChapeau.Type, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                    item.AddIngredient(soulofPlight.Type, 10);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<SirenChestplate>(), 1);
                    item.AddIngredient(ItemID.HallowedPlateMail, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                    item.AddIngredient(soulofPlight, 15);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<SirenLegs>(), 1);
                    item.AddIngredient(ItemID.HallowedGreaves, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                    item.AddIngredient(soulofPlight, 10);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                }
            }
        }

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

        if (ModLoader.HasMod(thoriumModName)) {
            Mod thoriumMod = ModLoader.GetMod(thoriumModName);
            // thrower
            if (thoriumMod.TryFind("AncientHallowedGuise", out ModItem ancientHallowedGuise)) {
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldViperHelmet>(), 1);
                item.AddIngredient(ancientHallowedGuise.Type, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofLight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldViperChestplate>(), 1);
                item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                item.AddIngredient(ItemID.SoulofLight, 15);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldViperLegs>(), 1);
                item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofLight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
            }
            // healer
            if (thoriumMod.TryFind("AncientHallowedCowl", out ModItem ancientHallowedCowl)) {
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldSeraphimHelmet>(), 1);
                item.AddIngredient(ancientHallowedCowl.Type, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofFlight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldSeraphimChestplate>(), 1);
                item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                item.AddIngredient(ItemID.SoulofFlight, 15);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
                temp = item;
                item = Recipe.Create(ModContent.ItemType<OldSeraphimLegs>(), 1);
                item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
                item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                item.AddIngredient(ItemID.SoulofFlight, 10);
                item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                item.AddTile(TileID.MythrilAnvil);
                item.SortAfter(temp);
                item.Register();
            }
            // bard
            if (thoriumMod.TryFind("AncientHallowedChapeau", out ModItem ancientHallowedChapeau)) {
                if (thoriumMod.TryFind("SoulofPlight", out ModItem soulofPlight)) {
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<OldSirenHelmet>(), 1);
                    item.AddIngredient(ancientHallowedChapeau.Type, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                    item.AddIngredient(soulofPlight.Type, 10);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<OldSirenChestplate>(), 1);
                    item.AddIngredient(ItemID.AncientHallowedPlateMail, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 12);
                    item.AddIngredient(soulofPlight, 15);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 15);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                    temp = item;
                    item = Recipe.Create(ModContent.ItemType<OldSirenLegs>(), 1);
                    item.AddIngredient(ItemID.AncientHallowedGreaves, 1);
                    item.AddRecipeGroup(RecipeGroups.Titanium, 10);
                    item.AddIngredient(soulofPlight, 10);
                    item.AddIngredient(ModContent.ItemType<SoulofBlight>(), 10);
                    item.AddTile(TileID.MythrilAnvil);
                    item.SortAfter(temp);
                    item.Register();
                }
            }
        }

        // sharanga
        item = Recipe.Create(ModContent.ItemType<Sharanga>(), 1);
        item.AddIngredient(ItemID.MoltenFury, 1);
        item.AddIngredient(ItemID.DemonBow, 1);
        item.AddTile(TileID.DemonAltar);
        item.SortAfterFirstRecipesOf(ItemID.MoltenFury);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<Sharanga>(), 1);
        item.AddIngredient(ItemID.MoltenFury, 1);
        item.AddIngredient(ItemID.TendonBow, 1);
        item.AddTile(TileID.DemonAltar);
        item.SortAfter(temp);
        item.Register();

        // holy grenade
        item = Recipe.Create(ModContent.ItemType<HolyHandgrenade>(), 1);
        item.AddIngredient(ItemID.Dynamite, 5);
        item.AddIngredient(ItemID.GoldBar, 2);
        item.AddIngredient(ItemID.BottledWater, 2);
        item.AddTile(TileID.WorkBenches);
        item.SortBeforeFirstRecipesOf(ItemID.TNTBarrel);
        item.Register();
        temp = item;
        item = Recipe.Create(ModContent.ItemType<HolyHandgrenade>(), 1);
        item.AddIngredient(ItemID.Dynamite, 5);
        item.AddIngredient(ItemID.PlatinumBar, 2);
        item.AddIngredient(ItemID.BottledWater, 2);
        item.AddTile(TileID.WorkBenches);
        item.SortAfter(temp);
        item.Register();
    }
}
