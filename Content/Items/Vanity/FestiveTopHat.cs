using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class FestiveTopHat : ModItem
	{
		public override void SetStaticDefaults() {

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			int width = 40; int height = 24;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Green;
			Item.buyPrice(gold: 5);

			Item.vanity = true;
		}
	}
}
