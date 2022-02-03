using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ammo
{
    public class SpectralArrow : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spectral Arrow"); 
            Tooltip.SetDefault("Can pierce three enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults() {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.damage = 16;
            Item.knockBack = 0f;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = 999;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.SpectralArrow>();
            Item.shootSpeed = 1.5f;

            Item.value = Item.sellPrice(copper: 20);
            Item.rare = ItemRarityID.Orange;

            Item.ammo = AmmoID.Arrow;
        }  
    }
}
