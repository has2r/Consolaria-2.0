using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ammo
{
    public class VulcanBolt : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Vulcan Bolt");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults() {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.damage = 12;
            Item.knockBack = 8;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.VulcanBolt>();
            Item.shootSpeed = 14f;

            Item.value = Item.sellPrice(copper: 35);
            Item.rare = ItemRarityID.Lime;

            Item.ammo = AmmoID.Arrow;
        }
    }
}
