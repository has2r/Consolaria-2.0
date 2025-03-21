using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.Wings)]
    public class SparklyWings : ModItem {
        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Wing.Sets.Stats [Item.wingSlot] = new WingStats(160, 7f, 2f);

            if (!Main.dedServ) {
                WingsGlowmask.RegisterData(Item.wingSlot, new DrawLayerData() {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Glow")
                });
            }
        }

        public override void SetDefaults () {
            int width = 30; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Lime;

            Item.accessory = true;
        }

        public override Color? GetAlpha (Color lightColor)
            => Color.White;

        public override void VerticalWingSpeeds (Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
            ascentWhenFalling = 0.65f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.12f;
        }
    }
}