using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials
{
    public class SoulofBlight : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Soul of Blight");
            Tooltip.SetDefault("'The essence of infected creatures'");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults() {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.rare = ItemRarityID.Lime;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void PostUpdate()  
          =>  Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.55f * Main.essScale);  
    }
}
