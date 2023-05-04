using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.BossDrops.Ocram {
    public class EerieOcramMusicBox : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/EerieOcram"), ModContent.ItemType<EerieOcramMusicBox>(), ModContent.TileType<Tiles.EerieOcramMusicBox>());
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
		}

		public override void SetDefaults () {
			int width = 24; int height = width;
			Item.Size = new Vector2(width, height);

			Item.accessory = true;
			Item.hasVanityEffects = true;

			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 1);
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.EerieOcramMusicBox>());
		}
	}
}