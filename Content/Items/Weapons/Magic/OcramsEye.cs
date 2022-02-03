using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Magic
{
    public class OcramsEye : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Eye of Ocram");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 34; int height = width;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Magic;
            Item.damage = 60;
            Item.knockBack = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 20;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.mana = 8;
            Item.UseSound = SoundID.Item33;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PurpleLaser;
            Item.shootSpeed = 20f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.altFunctionUse == 2) {
                Item.useAnimation = 2;
                Item.useTime = 2;
                int z = Projectile.NewProjectile(source, position.X, position.Y, velocity.X + Main.rand.Next(-8, 8), velocity.Y + Main.rand.Next(-8, 8), type, damage, knockback, player.whoAmI);
                Main.projectile[z].penetrate = 1;
                Main.projectile[z].hostile = false;
                Main.projectile[z].friendly = true;
                return false;
            }
            else {
                Item.useAnimation = 16;
                Item.useTime = 16;
                int a = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
                Main.projectile[a].penetrate = 8;
                Main.projectile[a].hostile = false;
                Main.projectile[a].friendly = true;
            }
            return false;
        }
    }
}
