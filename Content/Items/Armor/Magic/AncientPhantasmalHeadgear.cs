using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Head)]
    public class AncientPhantasmalHeadgear : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PhantasmalHeadgear>();
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 12;
        }

        public override void UpdateEquip (Player player) {
            player.statManaMax2 += 50;

            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetDamage(DamageClass.Magic) += 0.1f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<PhantasmalRobe>() || body.type == ModContent.ItemType<AncientPhantasmalRobe>())
            && (legs.type == ModContent.ItemType<PhantasmalSubligar>() || legs.type == ModContent.ItemType<AncientPhantasmalSubligar>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawOutlines = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Drinking a mana potion unleashes a barrage of homing spirit bolts";
            player.GetModPlayer<SpectralPlayer>().spectralGuard = true;
        }
    }
}