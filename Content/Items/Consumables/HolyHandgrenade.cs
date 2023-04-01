using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.Consumables {
    public class HolyHandgrenade : ModItem {
        public override void SetStaticDefaults () {
            // DisplayName.SetDefault("Holy Hand Grenade");
            // Tooltip.SetDefault("A huge explosion that will destroy most tiles" + "\n'The Lord's chosen weapon'");

            Item.ResearchUnlockCount = 99;
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

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.Dynamite, 5)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ItemID.BottledWater, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
				
				CreateRecipe()
                .AddIngredient(ItemID.Dynamite, 5)
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ItemID.BottledWater, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}