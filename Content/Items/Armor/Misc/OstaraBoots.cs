using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Misc
{
    [AutoloadEquip(EquipType.Legs)]
    public class OstaraBoots : ModItem
    {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;

            Item.defense = 3;
        }

        public override void UpdateEquip(Player player) 
           => player.GetModPlayer<OstarasPlayer>().bunnyHop = true;    
    }
}
