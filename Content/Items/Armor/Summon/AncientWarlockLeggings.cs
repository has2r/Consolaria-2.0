using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Summon {
    [AutoloadEquip(EquipType.Legs)]
    public class AncientWarlockLeggings : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WarlockLeggings>();
        }

        public override void SetDefaults() {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 8;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.15f;
            player.maxMinions += 1;

            player.GetDamage(DamageClass.Summon) += 0.2f;
        }
    }
}