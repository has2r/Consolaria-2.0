using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]

	public class ShirenShirt : ModItem
	{
		public override void Load() {
			string capeTexture = "Consolaria/Content/Items/Vanity/ShirenShirt_Back";
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, capeTexture, EquipType.Back, this);
		}

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shiren Shirt");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 25);
			Item.vanity = true;
		}

        public override void EquipFrameEffects(Player player, EquipType type) {
			var capeSlot = ModContent.GetInstance<ShirenShirt>();
			player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, capeSlot.Name, EquipType.Back);
		}
	}
}
