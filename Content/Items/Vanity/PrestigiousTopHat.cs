using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]

	public class PrestigiousTopHat : ModItem
	{
		public override void SetStaticDefaults() {

			Item.ResearchUnlockCount = 1;
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetDefaults() {
			int width = 20; int height = 16;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 50);
			Item.vanity = true;
		}
    }
}

