using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.HandsOn)]
    public class ValentineRing : ModItem {
        private bool unlockEffects;

        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
        }

        public override void ModifyTooltips (List<TooltipLine> tooltips) {
            if (!unlockEffects)
                tooltips.Add(new TooltipLine(Mod, "UnPickupped", "Give it to someone special!"));
            else
                tooltips.Add(new TooltipLine(Mod, "Pickupped", "Slowly regenerates life\nIncreases jump height"));
        }

        public override void SetDefaults () {
            int width = 30; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override void UpdateAccessory (Player player, bool hideVisual) {
            if (!unlockEffects)
                return;

            player.lifeRegen += 3;
            player.jumpSpeedBoost += 2.5f;
        }

        public override void SaveData (TagCompound tag) {
            tag.Add("pickuppedByPlayer", unlockEffects);
        }

        public override void LoadData (TagCompound tag) {
            unlockEffects = tag.GetBool("pickuppedByPlayer");
        }

        public override void NetSend (BinaryWriter writer) {
            writer.Write(unlockEffects);
        }

        public override void NetReceive (BinaryReader reader) {
            unlockEffects = reader.ReadBoolean();
        }

        public override bool OnPickup (Player player)
           => unlockEffects = true;
    }
}