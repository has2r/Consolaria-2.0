using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ammo {

    public class HeartArrow : ModItem {

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Heart Arrow"); 
            // Tooltip.SetDefault("'Legendary arrows from cupid himself'" + "\nHas a chance to stun enemies on hit");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults() {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.damage = 4;
            Item.knockBack = 3f;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.HeartArrow>();
            Item.shootSpeed = 3f;

            Item.value = Item.buyPrice(copper: 50);
            Item.rare = ItemRarityID.Blue;

            Item.ammo = AmmoID.Arrow;
        }  
    }
}
