using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class OtherworldlyMusicBox3 : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            MusicLoader.AddMusicBox(Mod,
                MusicLoader.GetMusicSlot(Mod, "Assets/Music/OtherwordlyOcram"),
                ModContent.ItemType<OtherworldlyMusicBox3>(), ModContent.TileType<Tiles.OtherworldlyMusicBox3>());
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes[Type] = false;
        }

        public override void SetDefaults() {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.OtherworldlyMusicBox3>(), 0);
            Item.maxStack = 1;
        }
    }
}