using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    class VanillaNPCShop : GlobalNPC {
        public override void ModifyShop (NPCShop shop) {
            Player player = Main.player [Main.myPlayer];
            var thanksgivingCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringThanksgiving", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsThanksgiving()) || !SeasonalEvents.configEnabled);
            var oktoberfestCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringOktoberfest", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsOktoberfest()) || (!SeasonalEvents.configEnabled && player.HeldItem.type == ItemID.Ale));
            var valentineDayCondition = new Condition("Mods.Consolaria.Conditions.SellingDuringValentineDay", () => (SeasonalEvents.configEnabled && SeasonalEvents.IsValentineDay()) || (!SeasonalEvents.configEnabled && player.HasBuff(BuffID.Lovestruck)));
            var isMaleCondition = new Condition("Mods.Consolaria.Conditions.SellingIfMale", () => player.Male);
            var womanMoment = new Condition("Mods.Consolaria.Conditions.SellingWhenWoman", () => !player.Male);

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
                shop.Add(ModContent.ItemType<Content.Items.Vanity.ShirenHat>(), Condition.MoonPhaseNew);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.ShirenShirt>(), Condition.MoonPhaseNew);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.ShirenPants>(), Condition.MoonPhaseNew);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosHead>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosBody>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.TorosLegs>(), Condition.BloodMoon);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesHat>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoShirt>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoPants>(), isMaleCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousRibbon>(), womanMoment);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousDress>(), womanMoment);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.FabulousSlippers>(), womanMoment);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.AlpineHat>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.Lederweste>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.Lederhosen>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.DirndlBlouse>(), oktoberfestCondition);
                shop.Add(ModContent.ItemType<Content.Items.Vanity.DirndlSkirt>(), oktoberfestCondition);
            }
            if (shop.NpcType == NPCID.SantaClaus) {
                shop.Add(ModContent.ItemType<Content.Items.Miscellaneous.StarTopper4>());
                shop.Add(ModContent.ItemType<Content.Items.Miscellaneous.Topper505>());
            }
            if (shop.NpcType == NPCID.Demolitionist)
                shop.Add(ModContent.ItemType<Content.Items.Weapons.Magic.RomanCandle>(), Condition.Christmas);
            if (shop.NpcType == NPCID.Cyborg)
                shop.Add(ModContent.ItemType<Content.Items.Pets.ShinyBlackSlab>());
            if (shop.NpcType == NPCID.Mechanic)
                shop.Add(ModContent.ItemType<Content.Items.Pets.MysteriousPackage>(), Condition.DownedMechBossAny);
            if (shop.NpcType == NPCID.SkeletonMerchant) {
                if (!WishbonePlayer.purchasedWishbone)
                    shop.Add(ModContent.ItemType<Content.Items.Consumables.Wishbone>());
            }
        }
    }
}