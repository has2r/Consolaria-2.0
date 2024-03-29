using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Armor.Melee {
    [AutoloadEquip(EquipType.Body)]
    public class DragonBreastplate : ModItem {
        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientDragonBreastplate>();
        }

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 24;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedPlateMail)
                .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.MythrilAnvil)
                .DisableDecraft()
                .Register();
        }
    }
}