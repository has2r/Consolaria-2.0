using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Head)]
    public class PhantasmalHeadgear : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Phantasmal Headgear");
            Tooltip.SetDefault("10% increased magic damage and critical strike chance" + "\nIncreases maximum mana by 70");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 12;
        }

        public override void UpdateEquip (Player player) {
            player.statManaMax2 += 70;

            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetDamage(DamageClass.Magic) += 0.1f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
           => (body.type == ModContent.ItemType<PhantasmalRobe>() || body.type == ModContent.ItemType<AncientPhantasmalRobe>())
           && (legs.type == ModContent.ItemType<PhantasmalSubligar>() || legs.type == ModContent.ItemType<AncientPhantasmalSubligar>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawOutlines = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Drinking a mana potion unleashes a barrage of homing spirit bolts";
            player.GetModPlayer<SpectralPlayer>().spectralGuard = true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedHeadgear)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofFright, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class SpectralPlayer : ModPlayer {
        public bool spectralGuard;
        public bool spectralAura;

        public override void ResetEffects ()
            => spectralGuard = false;

        public override void PreUpdate () {
            if (!spectralGuard) return;

            Main.NewText(Player.ownedProjectileCounts [ModContent.ProjectileType<SpectralSpirit>()]);
            if (Player.ownedProjectileCounts [ModContent.ProjectileType<SpectralSpirit>()] >= 6)
                spectralAura = true;
            else spectralAura = false;
        }
    }

    internal class ManaPotion : GlobalItem {
        public override void OnConsumeItem (Item item, Player player) {
            if (!player.GetModPlayer<SpectralPlayer>().spectralGuard || item.healMana < 0)
                return;

            if (!player.GetModPlayer<SpectralPlayer>().spectralAura && player.whoAmI == Main.myPlayer) {
                int randomPosition = Main.rand.Next(-20, 21);
                for (int i = 0; i < 6; i++)
                    Projectile.NewProjectile(player.GetSource_ItemUse(item), player.MountedCenter.X + randomPosition, player.MountedCenter.Y + randomPosition, 0f, 0f, ModContent.ProjectileType<SpectralSpirit>(), 0, 0f, player.whoAmI, 1 * i, 0);
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, player.Center);
            }
        }
    }
}