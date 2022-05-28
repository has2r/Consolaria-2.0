using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]

	public class GeorgesTuxedoShirt : ModItem
	{
		public override void Load() {
			string caneTexture = "Consolaria/Content/Items/Vanity/GeorgesTuxedoShirt_Cane";
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, caneTexture, EquipType.Front, this);
		}

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Georges Tuxedo Shirt");
			Tooltip.SetDefault("'Oh myyy!'");

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
			var caneSlot = ModContent.GetInstance<GeorgesTuxedoShirt>();
			player.front = (sbyte)EquipLoader.GetEquipSlot(Mod, caneSlot.Name, EquipType.Front);
		}
	}
}
