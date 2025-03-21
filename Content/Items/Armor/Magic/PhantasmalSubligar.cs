using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Legs)]
    public class PhantasmalSubligar : ModItem {
        public override void SetStaticDefaults() {

            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientPhantasmalSubligar>();
        }

        public override void SetDefaults() {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 12;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.12f;
            player.statManaMax2 += 30;

            player.GetDamage(DamageClass.Magic) += 0.05f;
        }
    }
}