using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Armor.Summon {
    [AutoloadEquip(EquipType.Body)]
    public class AncientWarlockRobe : ModItem {
        public override void Load () {
            string robeTexture = "Consolaria/Content/Items/Armor/Summon/AncientWarlockRobe_Extension";
            if (Main.netMode != NetmodeID.Server)
                EquipLoader.AddEquipTexture(Mod, robeTexture, EquipType.Legs, this);
        }

        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WarlockRobe>();
        }

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 10;
        }

        public override void SetMatch (bool male, ref int equipSlot, ref bool robes) {
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            robes = true;
        }

        public override void UpdateEquip (Player player) {
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.2f;
        }
    }
}