using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class LepusMusicBox : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Lepus"), ModContent.ItemType<LepusMusicBox>(), ModContent.TileType<Tiles.LepusMusicBox>());
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes [Type] = false;
        }

		public override void SetDefaults () {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.LepusMusicBox>(), 0);
            Item.maxStack = 1;
        }
	}
}