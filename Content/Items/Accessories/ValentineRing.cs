using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.HandsOn)]
    public class ValentineRing : ModItem {
        private bool unlockEffects;

        public override void SetStaticDefaults () {
            Tooltip.SetDefault("Give it to someone special!" + "\n50% health regeneration\nIncreases jump height");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override bool OnPickup (Player player)
           => unlockEffects = true;

        public override void UpdateAccessory (Player player, bool hideVisual) {
            if (unlockEffects) {
                player.lifeRegen += player.lifeRegen / 2;
                player.jumpSpeedBoost += 2f;
            }
        }
    }
}