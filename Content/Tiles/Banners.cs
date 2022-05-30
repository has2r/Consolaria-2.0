using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Tiles
{
	public class Banners : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };

			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Banner"));
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			int placeStyle = frameX / 18;
			int itemType = 0;
			switch (placeStyle) {
				case 0:
					itemType = ModContent.ItemType<Items.Banners.ArchWyvernBanner>();
					break;
				case 1:
					itemType = ModContent.ItemType<Items.Banners.DragonSnatcherBanner>();
					break;
				case 2:
					itemType = ModContent.ItemType<Items.Banners.SpectralElementalBanner>();
					break;
				case 3:
					itemType = ModContent.ItemType<Items.Banners.SpectralGastropodBanner>();
					break;
				case 4:
					itemType = ModContent.ItemType<Items.Banners.ShadowMummyBanner>();
					break;
				case 5:
					itemType = ModContent.ItemType<Items.Banners.ShadowHammerBanner>();
					break;
				case 6:
					itemType = ModContent.ItemType<Items.Banners.VampireMinerBanner>();
					break;
				case 7:
					itemType = ModContent.ItemType<Items.Banners.OrcaBanner>();
					break;
				case 8:
					itemType = ModContent.ItemType<Items.Banners.AlbinoAntlionBanner>();
					break;
				case 9:
					itemType = ModContent.ItemType<Items.Banners.ShadowMummyBanner>();
					break;
				case 10:
					itemType = ModContent.ItemType<Items.Banners.DragonHornetBanner>();
					break;
				case 11:
					itemType = ModContent.ItemType<Items.Banners.ArchDemonBanner>();
					break;
				case 12:
					itemType = ModContent.ItemType<Items.Banners.ShadowSlimeBanner>();
					break;
				case 13:
					itemType = ModContent.ItemType<Items.Banners.DisasterBunnyBanner>();
					break;
				case 14:
					itemType = ModContent.ItemType<Items.Banners.MythicalWyvernBanner>();
					break;
				case 15:
					itemType = ModContent.ItemType<Items.Banners.DragonSkullBanner>();
					break;
				default:
					return;
			}
			if (itemType > 0)
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, itemType);
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			if (closer) {
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].TileFrameX / 18;
				int bannerType = 0;
				switch (style) {
					case 0:
						bannerType = ModContent.ItemType<Items.Banners.ArchWyvernBanner>();
						break;
					case 1:
						bannerType = ModContent.ItemType<Items.Banners.DragonSnatcherBanner>();
						break;
					case 2:
						bannerType = ModContent.ItemType<Items.Banners.SpectralElementalBanner>();
						break;
					case 3:
						bannerType = ModContent.ItemType<Items.Banners.SpectralGastropodBanner>();
						break;
					case 4:
						bannerType = ModContent.ItemType<Items.Banners.ShadowMummyBanner>();
						break;
					case 5:
						bannerType = ModContent.ItemType<Items.Banners.ShadowHammerBanner>();
						break;
					case 6:
						bannerType = ModContent.ItemType<Items.Banners.VampireMinerBanner>();
						break;
					case 7:
						bannerType = ModContent.ItemType<Items.Banners.OrcaBanner>();
						break;
					case 8:
						bannerType = ModContent.ItemType<Items.Banners.AlbinoAntlionBanner>();
						break;
					case 9:
						bannerType = ModContent.ItemType<Items.Banners.ShadowMummyBanner>();
						break;
					case 10:
						bannerType = ModContent.ItemType<Items.Banners.DragonHornetBanner>();
						break;
					case 11:
						bannerType = ModContent.ItemType<Items.Banners.ArchDemonBanner>();
						break;
					case 12:
						bannerType = ModContent.ItemType<Items.Banners.ShadowSlimeBanner>();
						break;
					case 13:
						bannerType = ModContent.ItemType<Items.Banners.DisasterBunnyBanner>();
						break;
					case 14:
						bannerType = ModContent.ItemType<Items.Banners.MythicalWyvernBanner>();
						break;
					case 15:
						bannerType = ModContent.ItemType<Items.Banners.DragonSkullBanner>();
						break;
					default:
						return;
				}
				//player.HasNPCBannerBuff(bannerType);
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;	
		}
	}
}
