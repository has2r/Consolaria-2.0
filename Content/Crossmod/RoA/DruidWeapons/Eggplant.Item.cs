using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed partial class Eggplant : ModItem {
    public static string Path = "Consolaria/Content/Crossmod/RoA/DruidWeapons/Eggplant";

    public override bool IsLoadingEnabled(Mod mod) => RoACompat.IsRoAEnabled;

    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetDefaults() {
        int width = 30, height = 26;
        Item.width = width; Item.height = height;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = Item.useAnimation = 80;
        Item.noUseGraphic = false;
        Item.useTurn = false;
        Item.autoReuse = false;
        Item.UseSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/Eggplant");

        int baseDamage = 4;
        float knockBack = 3f;
        ushort potentialDamage = 10;
        float fillingRateModifier = 0.5f;
        Item.damage = baseDamage;
        Item.knockBack = knockBack;
        RoACompat.SetDruidicWeaponValues(Item, potentialDamage, fillingRateModifier);

        Item.shoot = ModContent.ProjectileType<Eggplant_Root>();
        Item.shootSpeed = 1f;
        Item.noMelee = true;

        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Blue;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        UpdateMaxEggplant(player.whoAmI, type);

        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

    public void UpdateMaxEggplant(int whoAmI, int eggplantType) {
        if (Main.myPlayer != whoAmI)
            return;

        List<Projectile> list = new List<Projectile>();
        for (int i = 0; i < 1000; i++) {
            if (Main.projectile[i].owner == whoAmI && Main.projectile[i].active && Main.projectile[i].type == eggplantType)
                list.Add(Main.projectile[i]);
        }

        int num = 0;
        int maxTurrets = 2;
        while (list.Count > maxTurrets - 1 && ++num < 1000) {
            Projectile projectile = list[0];
            for (int j = 1; j < list.Count; j++) {
                if (list[j].timeLeft < projectile.timeLeft)
                    projectile = list[j];
            }

            for (int i = 0; i < 1000; i++) {
                if (Main.projectile[i].owner == whoAmI && Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<Eggplant_Stem>() && (int)Main.projectile[i].ai[0] == Projectile.GetByUUID(projectile.owner, projectile.whoAmI)) {
                    Main.projectile[i].Kill();
                }
            }

            projectile.Kill();
            list.Remove(projectile);
        }
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        position = player.Center;
        Vector2 destination = Helper.GetLimitedPosition(player.Center, player.GetViableMousePosition(), 200f);
        while (!Collision.SolidCollision(position, 4, 4)) {
            if (Vector2.Distance(position, destination) < 60f) {
                break;
            }
            position += velocity.SafeNormalize(Vector2.Zero);
        }
    }
}
