using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace Consolaria.Content.Items.BossDrops.Turkor {
	public class TurkorMusicBox : ModItem {
		public override void SetStaticDefaults () {
			Item.ResearchUnlockCount = 1;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Turkor"), ModContent.ItemType<TurkorMusicBox>(), ModContent.TileType<Tiles.TurkorMusicBox>());
			ItemID.Sets.ShimmerTransformToItem [Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes [Type] = false;
        }

		public override void SetDefaults () {
			Item.DefaultToMusicBox(ModContent.TileType<Tiles.TurkorMusicBox>(), 0);
            Item.maxStack = 1;
        }
	}
}