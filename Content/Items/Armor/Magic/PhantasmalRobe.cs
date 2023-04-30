using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Body)]
    public class PhantasmalRobe : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientPhantasmalRobe>();
        }

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 16;
        }

        public override void UpdateEquip (Player player) {
            player.statManaMax2 += 70;

            player.GetDamage(DamageClass.Magic) += 0.15f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedPlateMail)
                .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.MythrilAnvil)
                .DisableDecraft()
                .Register();
        }
    }
}