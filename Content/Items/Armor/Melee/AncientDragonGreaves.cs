using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee {
    [AutoloadEquip(EquipType.Legs)]
    public class AncientDragonGreaves : ModItem {
        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<DragonGreaves>();
        }

        public override void SetDefaults () {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetDamage(DamageClass.Melee) += 0.05f;
            player.moveSpeed += 0.2f;
        }
    }
}