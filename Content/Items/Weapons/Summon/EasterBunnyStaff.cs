using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Summon;

sealed class EasterBunnyStaff : ModItem {
    public override void SetStaticDefaults() {
        ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults() {
        int width = 40, height = width;
        Item.width = width; Item.height = height;

        Item.damage = 9;
        Item.DamageType = DamageClass.Summon;
        Item.noMelee = true;
        Item.mana = 10;
        Item.knockBack = 5f;
        Item.rare = ItemRarityID.Blue;

        Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.EasterBunny>();
        Item.shootSpeed = 10f;
        Item.buffType = ModContent.BuffType<Buffs.EasterBunny>();

        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item44;
        Item.useAnimation = 36;
        Item.useTime = 36;
        Item.autoReuse = true;
        Item.reuseDelay = 2;

        Item.value = Item.sellPrice(gold: 1);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        player.AddBuff(Item.buffType, 2);
        player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

        return false;
    }
}
