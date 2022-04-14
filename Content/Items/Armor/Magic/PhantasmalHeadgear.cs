using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic
{
    [AutoloadEquip(EquipType.Head)]
    public class PhantasmalHeadgear : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phantasmal Headgear");
            Tooltip.SetDefault("5% increased magical damage" + "\n5% increased magical critical strike chance" + "\nIncreases maximum mana by 70");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 12;
        }

        public override void UpdateEquip(Player player) {
            player.statManaMax2 += 70;

            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) 
            => body.type == ModContent.ItemType<PhantasmalRobe>() && legs.type == ModContent.ItemType<PhantasmalSubligar>();

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Drinking a mana potion unleashes a barrage of homing spirit bolts";
            player.GetModPlayer<SpectralPlayer>().spectralGuard = true;
            Lighting.AddLight((int)((player.position.X) / 16.0), (int)((player.position.Y) / 16.0), 0.4f, 0.4f, 0.9f);
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedMask)
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class SpectralPlayer : ModPlayer
    {
        public bool spectralGuard;

        public override void ResetEffects()
            => spectralGuard = false;
    }

    internal class ManaPotion : GlobalItem
    {
        public override void OnConsumeItem(Item item, Player player) {
            if (player.GetModPlayer<SpectralPlayer>().spectralGuard) {
                int projectilesCount = Main.rand.Next(3, 6);
                Vector2 velocity = new(0, -3);
                if (item.type == ItemID.LesserManaPotion || item.type == ItemID.ManaPotion || item.type == ItemID.GreaterManaPotion || item.type == ItemID.SuperManaPotion || item.healMana > 0)
                    for (int i = 0; i < projectilesCount; i++) {
                        Vector2 position = new(player.position.X + Main.rand.Next(-50, 51), player.position.Y + Main.rand.Next(-40, 41));
                        Projectile.NewProjectile(player.GetItemSource_Misc(-1), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SpectralSpirit>(), 45, 2.5f, player.whoAmI);
                    }
            }
        }
    }
}
