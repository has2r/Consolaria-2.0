using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientTitanHelmet : ModItem
    {
      
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Titan Helmet");
            Tooltip.SetDefault("15% increased ranged damage and critical strike chance" + "\n25% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
      
        public override void SetDefaults() {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip(Player player) {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) 
            => body.type == ModContent.ItemType<TitanMail>() || body.type == ModContent.ItemType<AncientTitanMail>()
            && legs.type == ModContent.ItemType<TitanLeggings>() || legs.type == ModContent.ItemType<AncientTitanLeggings>();

        public override void ArmorSetShadows(Player player)
            => player.armorEffectDrawOutlinesForbidden = true;
        
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Using ranged weapons emits strong repelling wave around you";
            player.GetModPlayer<TitanPlayer>().titanPower = true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AncientHallowedHelmet)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
