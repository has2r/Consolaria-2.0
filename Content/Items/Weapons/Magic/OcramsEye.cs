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
           	Item.useTime = 8;
			Item.useAnimation = 24;
			Item.reuseDelay = 24;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.mana = 8;
            Item.UseSound = SoundID.Item33;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PurpleLaser;
            Item.shootSpeed = 16f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
            position += _velocity * 5;
            position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            float _randomVel = Main.rand.Next(-15, 16) * 0.035f;
            velocity += new Vector2(_randomVel, _randomVel);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
