using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed partial class Eggplant : ModItem {
    public static string Path = "Consolaria/Content/Crossmod/RoA/DruidWeapons/Eggplant";

    public override bool IsLoadingEnabled(Mod mod) => RoACompat.IsRoAEnabled;

    public override void SetDefaults() {
        int width = 30, height = 26;
        Item.width = width; Item.height = height;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.noUseGraphic = false;
        Item.useTurn = false;
        Item.autoReuse = false;
        Item.UseSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/Eggplant");

        int baseDamage = 15;
        float knockBack = 3f;
        ushort potentialDamage = 25;
        float fillingRateModifier = 1f;
        Item.damage = baseDamage;
        Item.knockBack = knockBack;
        RoACompat.SetDruidicWeaponValues(Item, potentialDamage, fillingRateModifier);

        Item.shoot = ModContent.ProjectileType<Eggplant_Root>();
        Item.shootSpeed = 1f;
        Item.noMelee = true;

        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Blue;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        position = player.Center;
        Vector2 destination = player.GetViableMousePosition(240f, 150f);
        while (!Collision.SolidCollision(position, 4, 4)) {
            if (Vector2.Distance(position, destination) < 60f) {
                break;
            }
            position += velocity.SafeNormalize(Vector2.Zero);
        }
    }
}
