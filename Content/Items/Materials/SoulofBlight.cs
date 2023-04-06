using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials {
    public class SoulofBlight : ModItem {
        public override void SetStaticDefaults () {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul [Item.type] = true;
            ItemID.Sets.ItemIconPulse [Item.type] = true;
            ItemID.Sets.ItemNoGravity [Item.type] = true;

            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults () {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);

            Item.width = refItem.width;
            Item.height = refItem.height;

            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 1, silver: 50);

            Item.maxStack = 9999;
        }

        public override void PostUpdate ()
            => Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.45f * Main.essScale);

        public override Color? GetAlpha (Color lightColor)
            => Color.White;
    }
}