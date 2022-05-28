using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Armor.Magic
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientPhantasmalRobe : ModItem
    {
        public override void Load() {
            string robeTexture = "Consolaria/Content/Items/Armor/Magic/AncientPhantasmalRobe_Extension";
            if (Main.netMode != NetmodeID.Server)
                EquipLoader.AddEquipTexture(Mod, robeTexture, EquipType.Legs, this);
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Phantasmal Robe");
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

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes) {
            var robeSlot = ModContent.GetInstance<AncientPhantasmalRobe>();
            equipSlot = EquipLoader.GetEquipSlot(Mod, robeSlot.Name, EquipType.Legs);
            robes = true;
        }

        public override void UpdateEquip(Player player) {
            player.statManaMax2 += 50;

            player.GetCritChance(DamageClass.Magic) += 4;
            player.GetDamage(DamageClass.Magic) += 0.07f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AncientHallowedPlateMail)
               .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}

