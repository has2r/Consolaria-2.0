using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Materials;

namespace Consolaria.Content.Items.Armor.Summon
{
    [AutoloadEquip(EquipType.Body)]
    public class WarlockRobe : ModItem
    {
        public override void Load()  {
            string robeTexture = "Consolaria/Content/Items/Armor/Summon/WarlockRobe_Extension";
            if (Main.netMode != NetmodeID.Server)
                EquipLoader.AddEquipTexture(Mod, robeTexture, EquipType.Legs, this);   
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Warlock Robe");
            Tooltip.SetDefault("9% increased minion damage" + "\nIncreases your max number of minions");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 10;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes) {
            var robeSlot = ModContent.GetInstance<WarlockRobe>();
            equipSlot = EquipLoader.GetEquipSlot(Mod, robeSlot.Name, EquipType.Legs);
            robes = true;
        }

        public override void UpdateEquip(Player player) {
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.09f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedPlateMail)
                .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

