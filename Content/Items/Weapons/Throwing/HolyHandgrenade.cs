using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Throwing {
    public class HolyHandgrenade : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 99;
            ItemID.Sets.ShimmerTransformToItem [Type] = ModContent.ItemType<HolyHandgrenade2>();
        }

        public override void SetDefaults () {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Generic;
            Item.damage = 600;

            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = Item.useTime = 50;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Lime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.HolyHandgrenade>();
            Item.shootSpeed = 4f;
        }
    }
}