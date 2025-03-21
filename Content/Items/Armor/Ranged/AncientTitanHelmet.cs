using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Head)]
    public class AncientTitanHelmet : ModItem {
        public override void SetStaticDefaults ()
            => ItemID.Sets.ShimmerTransformToItem [Type] = ModContent.ItemType<TitanHelmet>();

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<TitanMail>() || body.type == ModContent.ItemType<AncientTitanMail>())
            && (legs.type == ModContent.ItemType<TitanLeggings>() || legs.type == ModContent.ItemType<AncientTitanLeggings>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawOutlinesForbidden = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = TitanHelmet.SetBonusText.ToString();
            player.GetModPlayer<TitanPlayer>().titanPower = true;
        }
    }
}