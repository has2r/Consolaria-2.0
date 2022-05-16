using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee
{
    [AutoloadEquip(EquipType.Legs)]
    public class DragonGreaves : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dragon Greaves");
            Tooltip.SetDefault("15% increased movement speed");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
            => player.moveSpeed += 0.15f;

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedGreaves)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
