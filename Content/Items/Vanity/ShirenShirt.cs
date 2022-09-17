using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Vanity {
	[AutoloadEquip(EquipType.Body)]

	public class ShirenShirt : ModItem {
		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Shiren Shirt");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			int width = 30; int height = 18;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.White;
			Item.value = Item.buyPrice(gold: 25);
			Item.vanity = true;
		}
	}

	internal class ShirenCape : PlayerDrawLayer {
		private Asset<Texture2D> shirenCapeTexture;

		public override void Load ()
			=> shirenCapeTexture = ModContent.Request<Texture2D>("Consolaria/Content/Items/Vanity/ShirenShirt_Back");

		public override void Unload ()
			=> shirenCapeTexture = null;

		public override bool GetDefaultVisibility (PlayerDrawSet drawInfo) {
			if (((drawInfo.drawPlayer.armor [1].type == ModContent.ItemType<ShirenShirt>()) && Helper.CanDrawArmorLayer(drawInfo, 11)) ||
				(drawInfo.drawPlayer.armor [11].type == ModContent.ItemType<ShirenShirt>()))
				return true;
			return false;
		}

		public override Position GetDefaultPosition ()
			=> new AfterParent(PlayerDrawLayers.BackAcc);

		protected override void Draw (ref PlayerDrawSet drawInfo) {
			Player player = drawInfo.drawPlayer;
			if (player.dead || player.invis || player.back != -1) return;

			Texture2D texture = shirenCapeTexture.Value;
			Vector2 position = drawInfo.Position - Main.screenPosition + new Vector2(player.width / 2 - player.bodyFrame.Width / 2, player.height - player.bodyFrame.Height + 4f) + player.bodyPosition;
			Vector2 origin = drawInfo.bodyVect;

			DrawData drawData = new DrawData(texture, position.Floor() + origin, player.bodyFrame, drawInfo.colorArmorBody, player.bodyRotation, origin, 1f, drawInfo.playerEffect, 0);
			drawData.shader = player.cBody;
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}