using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class OtherworldlyMusicBox2 : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            MusicLoader.AddMusicBox(Mod,
                MusicLoader.GetMusicSlot(Mod, "Assets/Music/OtherwordlyTurkor"),
                ModContent.ItemType<OtherworldlyMusicBox2>(), ModContent.TileType<Tiles.OtherworldlyMusicBox2>());
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes[Type] = false;
        }

        public override void SetDefaults() {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.OtherworldlyMusicBox2>(), 0);
            Item.maxStack = 1;
        }
    }
}