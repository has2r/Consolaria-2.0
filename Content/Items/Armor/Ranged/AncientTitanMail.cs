using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Body)]
    public class AncientTitanMail : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Ancient Titan Mail");
            Tooltip.SetDefault("15% increased ranged damage" + "\n10 % increased ranged critical strike chance" + "\n20% chance to not consume ammo");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 18;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.AncientHallowedPlateMail)
               .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofSight, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}