using Consolaria.Content.Buffs;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Consumables {
    public class Wiesnbrau : ModItem {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 5;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3] {
                new Color(255, 140, 0),
                new Color(255, 165, 0),
                new Color(255, 255, 224)
            };
            ItemID.Sets.IsFood[Type] = true;
        }

        public override void SetDefaults() {
            Item.DefaultToFood(18, 22, ModContent.BuffType<Drunk>(), 60 * 30, true, 17);

            Item.value = Item.buyPrice(silver: 20);
            Item.rare = ItemRarityID.Blue;
        }
    }
}