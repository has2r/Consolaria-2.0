using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientTitanLeggings : ModItem
    {    
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Titan Leggings");
            Tooltip.SetDefault("10% increased ranged damage" + "\n10% increased movement speed" + "\n15% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults() {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 13;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AncientHallowedGreaves)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
