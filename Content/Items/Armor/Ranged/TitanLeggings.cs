using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Legs)]
    public class TitanLeggings : ModItem {
        public static Lazy<Asset<Texture2D>> leggingsGlowmask;
        public override void Unload () => leggingsGlowmask = null;

        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientTitanLeggings>();

            if (!Main.dedServ) {
                leggingsGlowmask = new(() => ModContent.Request<Texture2D>(Texture + "_Glow"));
                LegsGlowmask.RegisterData(Item.legSlot, new DrawLayerData() {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Legs_Glow")
                });
            }
        }

        public override void PostDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            => Item.BasicInWorldGlowmask(spriteBatch, leggingsGlowmask.Value.Value, new Color(255, 255, 255, 0) * 0.8f, rotation, scale);

        public override void SetDefaults () {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 13;
        }

        public override void UpdateEquip (Player player) {
            player.moveSpeed += 0.18f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedGreaves)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .DisableDecraft()
                .Register();
        }
    }
}