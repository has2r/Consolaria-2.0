using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Placeable {
    public class OcramMusicBox : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ocram"), ModContent.ItemType<OcramMusicBox>(), ModContent.TileType<Tiles.OcramMusicBox>());
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            ItemID.Sets.CanGetPrefixes[Type] = false;
        }

        public override void SetDefaults() {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.OcramMusicBox>(), 0);
            Item.maxStack = 1;
        }
    }
}