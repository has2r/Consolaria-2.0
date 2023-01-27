using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Consolaria.Content.Items.Vanity;

namespace Consolaria.Content.Items.Consumables {
    public class RedEnvelope : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Red Envelope");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 3;
        }

        public override void SetDefaults () {
            int width = 24; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.rare = ItemRarityID.Orange;
        }

        public override bool CanRightClick ()
            => true;

        public override void RightClick (Player player) {
            int mainDrops = Main.rand.Next(2);
            if (mainDrops == 0) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 35));
            if (mainDrops == 1) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));

            if (Main.rand.NextBool(25)) {
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>());
            }

            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.SilverCoin, Main.rand.Next(5, 15));
        }
    }
}