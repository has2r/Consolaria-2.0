using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Common.ModSystems;

namespace Consolaria.Common {
    class VanillaNPCShop : GlobalNPC {
        public static Condition thanksgivingCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringThanksgiving", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsThanksgiving()) || !SeasonalEvents.configEnabled);
        public static Condition oktoberfestCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringOktoberfest", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsOktoberfest()) || (!SeasonalEvents.configEnabled && Main.LocalPlayer.HeldItem.type == ItemID.Ale));
        public static Condition valentineDayCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringValentineDay", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsValentineDay()) || (!SeasonalEvents.configEnabled && Main.LocalPlayer.HasBuff(BuffID.Lovestruck)));
        public static Condition isMaleCondition = new Condition("Mods.Consolaria.Conditions.SellingIfMale", () => Main.LocalPlayer.Male & Main.bloodMoon);
        public static Condition womanMoment = new Condition("Mods.Consolaria.Conditions.SellingWhenWoman", () => !Main.LocalPlayer.Male & Main.bloodMoon);
        public static Condition wishboneCooldown = new Condition("Mods.Consolaria.Conditions.SellingWhenWoman", () => !WishbonePlayer.purchasedWishbone);

        public override void ModifyShop (NPCShop shop) {
            if (shop.NpcType == NPCID.Merchant) {
                shop.Add(ModContent.ItemType<Content.Items.Pets.TurkeyFeather>(), thanksgivingCondition);
                shop.Add(ModContent.ItemType<Content.Items.Consumables.Wiesnbrau>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Accessories.ValentineRing>(), valentineDayCondition);
                shop.Add(ModContent.ItemType<Content.Items.Weapons.Ammo.HeartArrow>(), valentineDayCondition);
                if (DownedBossSystem.downedTurkor)
                    shop.Add(ModContent.ItemType<Content.Items.Weapons.Ranged.SpicySauce>());

            }
            if (shop.NpcType == NPCID.TravellingMerchant) {
                shop.Add(ModContent.ItemType<Content.Items.Vanity.HornedGodMask>(), Condition.DownedMechBossAny, Condition.MoonPhaseFull);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.HornedGodRobe>(), Condition.DownedMechBossAny, Condition.MoonPhaseFull);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.HornedGodBoots>(), Condition.DownedMechBossAny, Condition.MoonPhaseFull);
            }
            if (shop.NpcType == NPCID.Clothier) {
                shop.Add(ModContent.ItemType<Content.Items.Pets.OldWalkingStick>());
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FestiveTopHat>(), Condition.Christmas);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.ShirenShirt>(), Condition.MoonPhaseNew);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.ShirenPants>(), Condition.MoonPhaseNew);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosHead>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosBody>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosLegs>(), Condition.BloodMoon);

                if (ModContent.GetInstance<ConsolariaConfig>().genderRestrictShopEnabled) {
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesHat>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoShirt>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoPants>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousRibbon>(), womanMoment);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousDress>(), womanMoment);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousSlippers>(), womanMoment);
                }
                else {
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesHat>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoShirt>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoPants>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousRibbon>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousDress>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousSlippers>(), Condition.BloodMoon);
                }

                shop.Add(ModContent.ItemType<Content.Items.Vanity.AlpineHat>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.Lederweste>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.Lederhosen>(), oktoberfestCondition);
                
                if (ModContent.GetInstance<ConsolariaConfig>().oktoberLocksEnabled) {
                shop.Add(ModContent.ItemType<Content.Items.Vanity.OktoberLocks>(), oktoberfestCondition);
                }

                shop.Add(ModContent.ItemType<Content.Items.Vanity.DirndlBlouse>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.DirndlSkirt>(), oktoberfestCondition);
            }
            if (shop.NpcType == NPCID.SantaClaus)
                shop.Add(ModContent.ItemType<Content.Items.Placeable.StarTopper4>());
            if (shop.NpcType == NPCID.Demolitionist)
                shop.Add(ModContent.ItemType<Content.Items.Weapons.Magic.RomanCandle>(), Condition.Christmas);
            if (shop.NpcType == NPCID.Cyborg)
                shop.Add(ModContent.ItemType<Content.Items.Pets.ShinyBlackSlab>());
            if (shop.NpcType == NPCID.Mechanic)
                shop.Add(ModContent.ItemType<Content.Items.Pets.MysteriousPackage>(), Condition.DownedMechBossAny);
            if (shop.NpcType == NPCID.SkeletonMerchant) {
                int wishBone = ModContent.ItemType<Content.Items.Consumables.Wishbone>();
                shop.Add(wishBone, wishboneCooldown);
            }
            if (shop.NpcType == NPCID.ArmsDealer) {
                if (DownedBossSystem.downedOcram)
                    shop.Add(ModContent.ItemType<Content.Items.Weapons.Ammo.SpectralArrow>());
            }
        }
    }
}