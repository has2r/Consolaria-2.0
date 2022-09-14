using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
	class VanillaNPCShop : GlobalNPC {
		public override void SetupShop (int type, Chest shop, ref int nextSlot) {
			Player player = Main.player [Main.myPlayer];
			if (type == NPCID.Merchant) {
				if (SeasonalEvents.isThanksgiving || !SeasonalEvents.enabled) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Pets.TurkeyFeather>());
					nextSlot++;
				}
				if (SeasonalEvents.isOktoberfest || !SeasonalEvents.enabled && player.HeldItem.type == ItemID.Ale) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Consumables.Wiesnbrau>());
					nextSlot++;
				}
				if (SeasonalEvents.isValentinesDay || !SeasonalEvents.enabled && player.HasBuff(BuffID.Lovestruck)) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Accessories.ValentineRing>());
					nextSlot++;
				}
				if (SeasonalEvents.isValentinesDay || !SeasonalEvents.enabled && player.HasBuff(BuffID.Lovestruck)) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Weapons.Ammo.HeartArrow>());
					nextSlot++;
				}
				if (DownedBossSystem.downedTurkor) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Weapons.Ranged.SpicySauce>());
					nextSlot++;
				}
			}
			if (type == NPCID.Demolitionist && Main.xMas) {
				shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Weapons.Magic.RomanCandle>());
				nextSlot++;
			}
			if (type == NPCID.TravellingMerchant && NPC.downedMechBossAny && Main.moonPhase == 4) {
				shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.HornedGodMask>());
				nextSlot++;
				shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.HornedGodRobe>());
				nextSlot++;
				shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.HornedGodBoots>());
				nextSlot++;
			}
			if (type == NPCID.Clothier) {
				if (Main.xMas) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.FestiveTopHat>());
					nextSlot++;
				}
				if (Main.moonPhase == 0) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.ShirenShirt>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.ShirenPants>());
					nextSlot++;
				}
				if (Main.bloodMoon) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.TorosHead>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.TorosBody>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.TorosLegs>());
					nextSlot++;

					if (player.Male) {
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.GeorgesHat>());
						nextSlot++;
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoShirt>());
						nextSlot++;
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.GeorgesTuxedoPants>());
						nextSlot++;
					}
					else {
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.FabulousRibbon>());
						nextSlot++;
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.FabulousDress>());
						nextSlot++;
						shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.FabulousSlippers>());
						nextSlot++;
					}
				}
				if (SeasonalEvents.isOktoberfest || !SeasonalEvents.enabled && player.HeldItem.type == ItemID.Ale) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.AlpineHat>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.Lederweste>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.Lederhosen>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.DirndlBlouse>());
					nextSlot++;
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Vanity.DirndlSkirt>());
					nextSlot++;
				}
			}
			if (type == NPCID.SkeletonMerchant) {
				if (Main.moonPhase == 0) {
					shop.item [nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Consumables.Wishbone>());
					nextSlot++;
				}
			}
		}
	}
}