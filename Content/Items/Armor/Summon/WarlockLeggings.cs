using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Summon {
    [AutoloadEquip(EquipType.Legs)]
    public class WarlockLeggings : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Warlock Leggings");
            Tooltip.SetDefault("Increases your max number of minions by 1" + "\n20% increased minion damage" + "\n15% increased movement speed");

            ArmorIDs.Legs.Sets.HidesBottomSkin [Item.legSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 22; int height = 18;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 8;
        }

        public override void UpdateEquip (Player player) {
            player.moveSpeed += 0.15f;
            player.maxMinions += 1;

            player.GetDamage(DamageClass.Summon) += 0.2f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedGreaves)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}