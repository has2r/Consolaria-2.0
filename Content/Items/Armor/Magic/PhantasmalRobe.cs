using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Armor.Magic
{
    [AutoloadEquip(EquipType.Body)]
    public class PhantasmalRobe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phantasmal Robe");
            Tooltip.SetDefault("7% increased magic damage" + "\n4% increased magic critical strike chance" + "\nIncreases maximum mana by 50");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip(Player player) {
            player.statManaMax2 += 50;

            player.GetCritChance(DamageClass.Magic) += 4;
            player.GetDamage(DamageClass.Magic) += 0.07f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedPlateMail)
               .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

