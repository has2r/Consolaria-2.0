using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Body)]
    public class PhantasmalRobe : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientPhantasmalRobe>();
        }

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 16;
        }

        public override void UpdateEquip (Player player) {
            player.statManaMax2 += 70;

            player.GetDamage(DamageClass.Magic) += 0.15f;
        }
    }
}