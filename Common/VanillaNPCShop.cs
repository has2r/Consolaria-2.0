using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common
{
	class VanillaNPCShop : GlobalNPC
	{
        public override void SetupShop(int type, Chest shop, ref int nextSlot) {
			if (type == NPCID.Merchant) {
				if (SeasonalEvents.isThanksgiving || !SeasonalEvents.enabled) {
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Pets.TurkeyFeather>());
					nextSlot++;
				}
			}
			if (type == NPCID.Demolitionist && Main.xMas) {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Weapons.Magic.RomanCandle>());
				nextSlot++;
			}
		}
	}
}
