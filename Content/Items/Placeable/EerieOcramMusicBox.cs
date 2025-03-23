using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class EerieOcramMusicBox : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/EerieOcram"), ModContent.ItemType<EerieOcramMusicBox>(), ModContent.TileType<Tiles.EerieOcramMusicBox>());
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes[Type] = false;
        }

        public override void SetDefaults() {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.EerieOcramMusicBox>(), 0);
            Item.maxStack = 1;
        }
    }
}