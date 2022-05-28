using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]

	public class FabulousDress : ModItem
	{
		public override void Load() {
			string skirtTexture = "Consolaria/Content/Items/Vanity/FabulousDress_Skirt";
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, skirtTexture, EquipType.Front, this);
		}

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Fabulous Dress");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 15);
			Item.vanity = true;
		}

		public override void EquipFrameEffects(Player player, EquipType type) {
			var skirtSlot = ModContent.GetInstance<FabulousDress>();
			player.front = (sbyte)EquipLoader.GetEquipSlot(Mod, skirtSlot.Name, EquipType.Front);
		}

		/*public override void SetMatch(bool male, ref int equipSlot, ref bool robes) {
			robes = true;
			var robeSlot = ModContent.GetInstance<FabulousDress>();
			equipSlot = Mod.GetEquipSlot(robeSlot.Name, EquipType.Legs);
		}*/
	}
}
