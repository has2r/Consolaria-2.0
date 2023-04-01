using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Turkor
{	
	public class HornoPlenty : ModItem
	{		
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Horn o' Plenty");
            // Tooltip.SetDefault("It is filled with the inexhaustible gifts of celebratory fruits");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			int width = 30; int height = width;
			Item.Size = new Vector2(width, height);

			Item.useTime = Item.useAnimation = 36;
			Item.holdStyle = ItemHoldStyleID.HoldFront;

			Item.healLife = 100;
			Item.potion = true;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.consumable = false;
			Item.noMelee = true;

			Item.value = Item.sellPrice(gold: 1, silver: 80);
			Item.rare = ItemRarityID.Orange;
			Item.expert = true;

			Item.UseSound = SoundID.Item2;
		}

		public override void HoldStyle(Player player, Rectangle heldItemFrame) {
			player.itemLocation.X = player.MountedCenter.X + 4f * player.direction;
			player.itemLocation.Y = player.MountedCenter.Y + 14f;
			player.itemRotation = 0f;
		}

		public override bool ConsumeItem(Player player) => false;       
    }
}
