using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace Consolaria.Content.Items.Armor.Misc
{
	[AutoloadEquip(EquipType.Head)]
	public class OstaraHat : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hat of Ostara");
			Tooltip.SetDefault("5% increased movement speed");
		}

		public override void SetDefaults() {
			int width = 20; int height = 24;
			Item.Size = new Vector2(width, height);
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.defense = 4;
			Item.rare = ItemRarityID.Green;
		}

        public override void UpdateEquip(Player player)        
			=> player.moveSpeed += 0.1f;

		//public override bool IsArmorSet(Item head, Item body, Item legs)
		//	=> body.type == ModContent.ItemType<OstaraChainmail>() && legs.type == ModContent.ItemType<OstaraBoots>();

		public override void UpdateArmorSet(Player player) {
			player.setBonus = "Negates fall damage";
			player.noFallDmg = true;
		}
	}
}
