using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.BossDrops.Turkor {
	public class TurkorMusicBox : ModItem {
		public override void SetStaticDefaults () {
			// DisplayName.SetDefault("Music Box (Turkor the Ungrateful)");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Turkor"), ModContent.ItemType<TurkorMusicBox>(), ModContent.TileType<Tiles.TurkorMusicBox>());
		}

		public override void SetDefaults () {
			int width = 24; int height = width;
			Item.Size = new Vector2(width, height);

			Item.accessory = true;
			Item.hasVanityEffects = true;

			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 1);
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TurkorMusicBox>());
		}
	}
}