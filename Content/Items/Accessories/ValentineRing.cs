using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.HandsOn)]
    public class ValentineRing : ModItem {
        private static string SAVEKEY => nameof(Consolaria) + "valentinering";

        private string _ownerName = string.Empty;

        public bool HasOwner => !_ownerName.Equals(string.Empty);

        public bool IsInOwnerInventory(Player owner) => _ownerName.Equals(owner.name);

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            if (!HasOwner) {
                return;
            }

            bool isInOwnerInventory = IsInOwnerInventory(Main.LocalPlayer);

            if (isInOwnerInventory) {
                tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isInOwnerInventory", Language.GetTextValue("Mods.Consolaria.ValentineRingTooltip1")));
                tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isInOwnerInventory2", Language.GetText("Mods.Consolaria.ValentineRingTooltip3").Format(_ownerName)));
                return;
            }

            tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isNotInOwnerInventory", Language.GetTextValue("Mods.Consolaria.ValentineRingTooltip2")));
            tooltips.Add(new TooltipLine(Mod, nameof(ValentineRing) + "isNotInOwnerInventory2", Language.GetText("Mods.Consolaria.ValentineRingTooltip4").Format(_ownerName)));
        }

        public override void SetDefaults() {
            int width = 30; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            bool isInOwnerInventory = IsInOwnerInventory(player);
            if (isInOwnerInventory) {
                return;
            }

            player.lifeRegen += 3;
            player.jumpSpeedBoost += 2.5f;
        }

        public override void SaveData(TagCompound tag) {
            tag[SAVEKEY] = _ownerName;
        }

        public override void LoadData(TagCompound tag) {
            _ownerName = tag.GetString(SAVEKEY);
        }

        public override void NetSend(BinaryWriter writer) {
            writer.Write(_ownerName);
        }

        public override void NetReceive(BinaryReader reader) {
            _ownerName = reader.ReadString();
        }

        public override void UpdateInventory(Player player) {
            if (HasOwner) {
                return;
            }

            _ownerName = player.name;
        }
    }
}