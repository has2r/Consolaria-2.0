using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Armor.Misc
{
    [AutoloadEquip(EquipType.Body)]
    public class OstaraJacket : ModItem
    {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = 20;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(silver: 16);
            Item.rare = ItemRarityID.Green;

            Item.defense = 3;
        }

        public override void UpdateEquip(Player player) 
            => player.moveSpeed += 0.07f;
    }
}

