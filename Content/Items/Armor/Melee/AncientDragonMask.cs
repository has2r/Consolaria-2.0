using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee {
    [AutoloadEquip(EquipType.Head)]
    public class AncientDragonMask : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<DragonMask>();
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 18;
        }

        public override void UpdateEquip (Player player) {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<DragonBreastplate>() || body.type == ModContent.ItemType<AncientDragonBreastplate>())
            && (legs.type == ModContent.ItemType<DragonGreaves>() || legs.type == ModContent.ItemType<AncientDragonGreaves>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawShadow = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Creates a burst of flames after taking damage";
            player.GetModPlayer<DragonPlayer>().dragonBurst = true;
        }
    }
}