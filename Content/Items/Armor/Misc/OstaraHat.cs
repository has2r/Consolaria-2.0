using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.Localization;

namespace Consolaria.Content.Items.Armor.Misc {
    [AutoloadEquip(EquipType.Head)]
    public class OstaraHat : ModItem {
        public static LocalizedText SetBonusText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            ArmorIDs.Head.Sets.DrawHatHair [Item.headSlot] = true;
            SetBonusText = this.GetLocalization("SetBonus");
        }

        public override void SetDefaults () {
            int width = 20; int height = 24;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(silver: 12);
            Item.rare = ItemRarityID.Green;

            Item.defense = 2;
        }

        public override void UpdateEquip (Player player)
            => player.moveSpeed += 0.07f;

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => body.type == ModContent.ItemType<OstaraJacket>() && legs.type == ModContent.ItemType<OstaraBoots>();

        public override void UpdateArmorSet (Player player) {
            player.setBonus = SetBonusText.ToString();
            player.noFallDmg = true;
        }
    }

    public class OstarasPlayer : ModPlayer {
        public bool bunnyHop;
        private int hopsCount;

        public override void ResetEffects ()
         => bunnyHop = false;

        public override void PostUpdateEquips () {
            if (bunnyHop) {
                if (Math.Abs(Player.velocity.X) < 1) hopsCount = 0;
                if (Player.controlJump && Player.releaseJump) hopsCount++;
                switch (hopsCount) {
                case 0:
                Player.jumpSpeedBoost += 0f;
                break;
                case 1:
                Player.jumpSpeedBoost += 1f;
                break;
                case 2:
                Player.jumpSpeedBoost += 2f;
                break;
                case 3:
                Player.jumpSpeedBoost += 3f;
                break;
                case 4:
                Player.jumpSpeedBoost += 4f;
                break;
                case >= 5:
                Player.jumpSpeedBoost += 5f;
                break;
                }
            }
        }
    }
}